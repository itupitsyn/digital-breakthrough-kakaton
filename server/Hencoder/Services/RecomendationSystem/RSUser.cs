using SQLite;

namespace Hencoder.Services.RecomendationSystem
{
    public class RSUser
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public long id { get; set; }
        public string token { get; set; }
    }

    public enum RSUserActionType : int
    {
        None = 0,
        Like = 1,
        Dislike = 2,
        Ignored = 3,
    }

    public class RSUserAction
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public long id { get; set; }
        [Indexed, NotNull]
        public long user_id { get; set; }
        [Indexed, NotNull]
        public long video_id { get; set; }
        public int action_type { get; set; }
        public long timestamp { get; set; }
    }
}
