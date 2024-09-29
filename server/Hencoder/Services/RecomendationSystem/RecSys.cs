using Hencoder.Models;
using Hencoder.Services.QdrantServices;
using Hencoder.Services.Repositories;
using ZeroLevel;

namespace Hencoder.Services.RecomendationSystem
{
    public class RecSysTest
    {
        public IEnumerable<string> Users { get; set; }
        public long VideosCount { get; set; }
    }

    public class VIDEO
    {
        public long video_id { get; set; }
    }

    public class RecSys
    {
        /// <summary>
        /// Кэш с категориями
        /// </summary>
        private static UUID2LongCachee _categories = new UUID2LongCachee("categories");
        /// <summary>
        /// Просмотренные пользователем записи
        /// </summary>
        private static UserActionsRepository _userActionsRepository = new UserActionsRepository();
        /// <summary>
        /// Пользователи рекомендательной системы
        /// </summary>
        private static RecSysUsersRepository _users = new RecSysUsersRepository();
        /// <summary>
        /// Таблица с данными по видео
        /// </summary>
        private static VideoStatRepository _videoSource = new VideoStatRepository();

        private readonly QdrantRepository _qdrantRepository;

        public RecSys(QdrantRepository qdrantRepository)
        {
            _qdrantRepository = qdrantRepository;
        }

        public RecSysTest GetTestData()
        {
            var data = new RecSysTest
            {
                Users = _users.SelectAll().Select(r => r.token),
                VideosCount = _videoSource.Count(),
            };
            return data;
        }

        public void SaveUserRecomendation(string token, IEnumerable<VideoStatEntry> entries)
        {
            var user_id = _users.GetId(token);
            var records = entries.Select(e => new RSUserAction
            {
                action_type = (int)RSUserActionType.None,
                timestamp = Timestamp.UtcNow,
                user_id = user_id,
                video_id = e.video_id,
            });
            _userActionsRepository.Append(records);
        }

        public int SaveUserAction(string token, RSUserAction action)
        {
            var user_id = _users.GetId(token);
            action.user_id = user_id;
            var exist = _userActionsRepository.Single(r => r.user_id == action.user_id && r.video_id == action.video_id);
            if (exist != null)
            {
                exist.action_type = action.action_type;
                exist.timestamp = action.timestamp;
                return _userActionsRepository.Update(exist);
            }
            else
            {
                Log.Warning($"[RecSys.SaveUserAction] User '{action.user_id}' performed action '{action.action_type}' for a record '{action.video_id}' that was not issued to him.");
            }
            return 0;
        }

        public bool VerifyToken(string token)
        {
            return _users.Single(u => u.token == token) != null;
        }

        public async Task<IEnumerable<VideoStatEntry>> GetNextRecomendation(string token)
        {
            var userId = _users.GetId(token);
            long[] showedIds = new long[0];
            if (userId >= 0)
            {
                // 1. Получить идентификаторы видео которые уже были отображены пользователю
                var records = _userActionsRepository.SelectBy(r => r.user_id == userId);
                if (records != null && records.Any())
                {
                    showedIds = records.Select(r => r.video_id).ToArray();
                    Log.Debug($"[RecSys.GetNextRecomendation] Detected {showedIds.Length} records that have been shown to user {userId} previously.");
                }
                else
                {
                    Log.Debug($"[RecSys.GetNextRecomendation] No detected records that have been shown to user {userId} previously.");
                }
                // 2. Найти записи которые понравились пользователю
                var positive = records?.Where(r => r.action_type == (int)RSUserActionType.Like)?.Select(r => r.video_id)?.ToArray() ?? new long[0];
                // 3. Найти записи которые не понравились пользователю
                var negative = records?.Where(r => r.action_type == (int)RSUserActionType.Dislike)?.Select(r => r.video_id)?.ToArray() ?? new long[0];
                // 4. Получить 10 записей для пользователя
                var recomendation = await Search(userId, showedIds, positive, negative);
                // 5. Долить категории
                foreach (var r in recomendation)
                {
                    r.category = _categories.Single(c => c.id == r.category_id)?.uuid ?? string.Empty;
                }
                return recomendation;
            }
            else
            {
                Log.Warning($"[RecSys.GetNextRecomendation] No user record detected by token '{token}'.");
            }
            return Enumerable.Empty<VideoStatEntry>();
        }

        private async Task<IEnumerable<VideoStatEntry>> Search(long userId, long[] excluded, long[] positive, long[] negative)
        {
            if (positive.Length == 0)
            {
                if (TOP_COMMENTED == null)
                {
                    TOP_COMMENTED = _videoSource.SelectByQuery<VIDEO>(TOP_COMMENTED_QUERY).Select(t=>t.video_id).ToArray();
                }
                positive = TOP_COMMENTED;
            }
            try
            {
                var recommendedVideos = (await _qdrantRepository.Recommend(10, 0.0f, positive, negative, excluded)).ToArray();
                if (recommendedVideos != null && recommendedVideos.Length == 10)
                {
                    return _videoSource.Query(string.Format(QUERY, string.Join(", ", recommendedVideos)));
                }
                else
                {
                    Log.Warning($"[RecSys.Search] Qdrant search fault. Found {recommendedVideos?.Length??0} recomendations. Use sqlite.");
                }
            }
            catch (Exception ex)
            {
                Log.Warning($"[RecSys.Search] Qdrant not working: {ex.Message}");
            }
            if (excluded.Length > 0)
            {
                if (positive.Length == 0 && negative.Length == 0)
                {
                    // Холодный старт, знаем только то что пользователь глянул
                }
                else
                {
                    // Прохладный старт, знаем что пользователь смотрел, и есть информация по лайкам или дизлайкам.
                }
                return _videoSource.Query(string.Format(COLD_QUERY, string.Join(", ", excluded)));
            }
            // Ледяной старт, информации нет вообще никакой
            return _videoSource.Query(ICE_QUERY);
        }

        private static long[] TOP_COMMENTED = null;

        const string QUERY = "SELECT video_id, title, description, v_pub_datetime, v_likes, v_dislikes, v_duration, category_id FROM VideoStatEntry WHERE video_id IN({0});";

        const string ICE_QUERY = "SELECT video_id, title, description, v_pub_datetime, v_likes, v_dislikes, v_duration, category_id FROM VideoStatEntry ORDER BY RANDOM() LIMIT 10;";

        const string COLD_QUERY = "SELECT video_id, title, description, v_pub_datetime, v_likes, v_dislikes, v_duration, category_id FROM VideoStatEntry WHERE video_id NOT IN({0}) ORDER BY RANDOM() LIMIT 10;";

        const string TOP_COMMENTED_QUERY = "SELECT video_id FROM VideoStatEntry ORDER BY v_total_comments DESC LIMIT 10";

        public string CreateAccount()
        {
            var token = Guid.NewGuid().ToString();
            if (_users.Create(token))
                return token;
            return string.Empty;
        }
    }
}
