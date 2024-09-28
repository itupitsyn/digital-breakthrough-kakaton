using RecSysConverter.VideoStatsConvert;

namespace RecSysConverter.TrainSet
{
    internal class TrainSetBuilder
    {
        public static void Build()
        {
            var _ds = new DataSetRepository();
            var dataset = new List<DataSet>();

            // 1. Отбираем подходящие наборы
            var preset = new PresetRepository();
            var preset_dictionary = preset.SelectAll().GroupBy(d => d.user_id).ToDictionary(g => g.Key, g => g.ToList());
            preset.Dispose();

            var used_videos = new HashSet<long>();
            var stat = new VideoStatRepository();
            var vectors = new VectorsRepository();
            var userSet = new Dictionary<long, int>();
            foreach (var entry in preset_dictionary)
            {
                var candidates = new List<TrainPreSet>();
                userSet.Clear();
                foreach (var e in entry.Value)
                {
                    if (userSet.ContainsKey(e.video_id) == false)
                    {
                        userSet[e.video_id] = 1;
                    }
                    else
                    {
                        userSet[e.video_id] += 1;
                    }
                }
                // Проработка просмотров пользователя
                foreach (var vid in userSet.OrderBy(s => s.Value).Select(p => p.Key))
                {
                    var candidate = new TrainPreSet
                    {
                        user_id = entry.Key,
                        video_id = vid,
                        watchtime = entry.Value.Where(r => r.video_id == vid).Select(r => r.watchtime).Sum(),
                    };
                    var r = stat.Single(r => r.video_id == vid);
                    if (r != null)
                    {
                        candidate.likes = (long)r.v_likes;
                        candidate.dislikes = (long)r.v_dislikes;
                        candidate.duration = (long)r.v_duration;
                    }
                    candidates.Add(candidate);
                }
                if (candidates.Count < 3) continue;
                // Поиск элементов для TrippleLoss
                var pos_candidates = new List<TrainPreSet>();
                var neg_candidates = new List<TrainPreSet>();
                foreach (var candidate in candidates)
                {
                    if (used_videos.Add(candidate.video_id))
                    {
                        bool is_long_view = (candidate.duration > 300) ? (candidate.watchtime > 0.25 * candidate.duration) : candidate.watchtime > 30;
                        if (is_long_view)
                        {
                            pos_candidates.Add(candidate);
                        }
                        else
                        {
                            neg_candidates.Add(candidate);
                        }
                    }
                }
                if (candidates.Count >= 3 && neg_candidates.Count == 0)
                {
                    double max_neg = -1;
                    TrainPreSet negativeCandidate = null;
                    foreach (var candidate in candidates)
                    {
                        var totalReview = candidate.likes + candidate.dislikes;
                        var negative = candidate.dislikes / totalReview;
                        if (max_neg < negative)
                        {
                            max_neg = negative;
                            negativeCandidate = candidate;
                        }
                    }
                    if (negativeCandidate != null)
                    {
                        neg_candidates.Add(negativeCandidate);
                        pos_candidates.Remove(negativeCandidate);
                    }
                }
                else if (candidates.Count >= 3 && pos_candidates.Count == 0)
                {
                    double max_pos = -1;
                    TrainPreSet positiveCandidate = null;
                    foreach (var candidate in candidates)
                    {
                        var totalReview = candidate.likes + candidate.dislikes;
                        var positive = candidate.likes / totalReview;
                        if (max_pos < positive)
                        {
                            max_pos = positive;
                            positiveCandidate = candidate;
                        }
                    }
                    if (positiveCandidate != null)
                    {
                        pos_candidates.Add(positiveCandidate);
                        neg_candidates.Remove(positiveCandidate);
                    }
                }

                var fill_vectors = new Func<DataSet, DataSet>(data =>
                {
                    data.negative_video_bert = vectors.Single(v => v.video_id == data.negative_video_id)?.vector;
                    data.positive_video1_bert = vectors.Single(v => v.video_id == data.positive_video_id1)?.vector;
                    data.positive_video2_bert = vectors.Single(v => v.video_id == data.positive_video_id2)?.vector;
                    return data;
                });

                if (pos_candidates.Count >= 2 && neg_candidates.Count >= 1)
                {
                    if (pos_candidates.Count >= 4 && neg_candidates.Count >= 2)
                    {
                        var ds1 = new DataSet
                        {
                            user_id = entry.Key,
                            negative_video_id = neg_candidates[0].video_id,
                            positive_video_id1 = pos_candidates[0].video_id,
                            positive_video_id2 = pos_candidates[1].video_id,
                        };
                        ds1 = fill_vectors(ds1);
                        if (ds1.negative_video_bert != null && ds1.positive_video1_bert != null && ds1.positive_video2_bert != null)
                        {
                            _ds.Append(ds1);
                        }
                        var ds2 = new DataSet
                        {
                            user_id = entry.Key,
                            negative_video_id = neg_candidates[1].video_id,
                            positive_video_id1 = pos_candidates[2].video_id,
                            positive_video_id2 = pos_candidates[3].video_id,
                        };
                        ds2 = fill_vectors(ds2);
                        if (ds2.negative_video_bert != null && ds2.positive_video1_bert != null && ds2.positive_video2_bert != null)
                        {
                            _ds.Append(ds2);
                        }
                        used_videos.Add(ds1.positive_video_id1);
                        used_videos.Add(ds1.positive_video_id2);
                        used_videos.Add(ds1.negative_video_id);
                        used_videos.Add(ds2.positive_video_id1);
                        used_videos.Add(ds2.positive_video_id2);
                        used_videos.Add(ds2.negative_video_id);
                    }
                    else
                    {
                        var ds = new DataSet
                        {
                            user_id = entry.Key,
                            negative_video_id = neg_candidates[0].video_id,
                            positive_video_id1 = pos_candidates[0].video_id,
                            positive_video_id2 = pos_candidates[1].video_id,
                        };
                        ds = fill_vectors(ds);
                        if (ds.negative_video_bert != null && ds.positive_video1_bert != null && ds.positive_video2_bert != null)
                        {
                            _ds.Append(ds);
                        }
                        used_videos.Add(ds.positive_video_id1);
                        used_videos.Add(ds.positive_video_id2);
                        used_videos.Add(ds.negative_video_id);
                    }
                }
            }
            _ds.Dispose();
        }
    }
}
