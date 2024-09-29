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

    
}
