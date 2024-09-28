using RecSysConverter.VideoStatsConvert;
using ZeroLevel;
using ZeroLevel.Services.Collections;

namespace RecSysConverter.ExtendedFeatureVector
{
    internal class ExtendedVectorBuilder
    {
        private static float[] ParseBertVector(byte[] bytes)
        {
            var bert_embedding = new float[312];
            for (int i = 0; i < bert_embedding.Length; i++)
            {
                bert_embedding[i] = BitConverter.ToSingle(bytes, i * 4);
            }
            return bert_embedding;
        }

        public static void Build()
        {
            var extended = new ExtendedVectorRepository();
            var stat = new VideoStatRepository();
            var bert = new BertVectorRepository();

            var bert_map = bert.SelectAll().ToDictionary(r => r.video_id, r => ParseBertVector(r.vector));
            int proceed = 0;
            using (var processor = new BatchProcessor<ExtendedVectors>(5000, records => extended.Append(records)))
            {
                foreach (var item in stat.SelectAll())
                {
                    var ex_vector = new float[349];
                    var bert_embedding = bert_map[item.video_id];
                    Array.Copy(bert_embedding, ex_vector, bert_embedding.Length);
                    var index = 312;
                    ex_vector[index++] = item.v_pub_datetime;
                    ex_vector[index++] = item.v_total_comments;
                    ex_vector[index++] = item.v_year_views;
                    ex_vector[index++] = item.v_month_views;
                    ex_vector[index++] = item.v_week_views;
                    ex_vector[index++] = item.v_day_views;
                    ex_vector[index++] = item.v_likes;
                    ex_vector[index++] = item.v_dislikes;
                    ex_vector[index++] = (float)item.v_duration;
                    ex_vector[index++] = (float)item.v_cr_click_like_7_days;
                    ex_vector[index++] = (float)item.v_cr_click_dislike_7_days;
                    ex_vector[index++] = (float)item.v_cr_click_vtop_7_days;
                    ex_vector[index++] = (float)item.v_cr_click_long_view_7_days;
                    ex_vector[index++] = (float)item.v_cr_click_comment_7_days;
                    ex_vector[index++] = (float)item.v_cr_click_like_30_days;
                    ex_vector[index++] = (float)item.v_cr_click_dislike_30_days;
                    ex_vector[index++] = (float)item.v_cr_click_vtop_30_days;
                    ex_vector[index++] = (float)item.v_cr_click_long_view_30_days;
                    ex_vector[index++] = (float)item.v_cr_click_comment_30_days;
                    ex_vector[index++] = (float)item.v_cr_click_like_1_days;
                    ex_vector[index++] = (float)item.v_cr_click_dislike_1_days;
                    ex_vector[index++] = (float)item.v_cr_click_vtop_1_days;
                    ex_vector[index++] = (float)item.v_cr_click_long_view_1_days;
                    ex_vector[index++] = (float)item.v_cr_click_comment_1_days;
                    ex_vector[index++] = (float)item.v_avg_watchtime_1_day;
                    ex_vector[index++] = (float)item.v_avg_watchtime_7_day;
                    ex_vector[index++] = (float)item.v_avg_watchtime_30_day;
                    ex_vector[index++] = (float)item.v_frac_avg_watchtime_1_day_duration;
                    ex_vector[index++] = (float)item.v_frac_avg_watchtime_7_day_duration;
                    ex_vector[index++] = (float)item.v_frac_avg_watchtime_30_day_duration;
                    ex_vector[index++] = (float)item.v_category_popularity_percent_7_days;
                    ex_vector[index++] = (float)item.v_category_popularity_percent_30_days;
                    ex_vector[index++] = item.v_long_views_1_days;
                    ex_vector[index++] = item.v_long_views_7_days;
                    ex_vector[index++] = item.v_long_views_30_days;
                    ex_vector[index++] = item.author_id;
                    ex_vector[index++] = item.category_id;

                    var ex_vector_bytes = new byte[ex_vector.Length * 4];
                    for (int i = 0; i < ex_vector.Length; i++)
                    {
                        Array.Copy(BitConverter.GetBytes(ex_vector[i]), 0, ex_vector_bytes, i * 4, 4);
                    }

                    processor.Add(new ExtendedVectors { video_id = item.video_id, vector = ex_vector_bytes });

                    proceed++;
                    if (proceed % 5000 == 0)
                    {
                        Log.Info(proceed.ToString());
                    }
                }
            }
        }
    }
}
