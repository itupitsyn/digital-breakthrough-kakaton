using Hencoder.Services.RecomendationSystem;

namespace Hencoder.Services.Repositories
{
    /// <summary>
    /// Показанные пользователю записи
    /// </summary>
    public class ShowedRepository
        : BaseSqliteDB<RSUserAction>
    {
        public ShowedRepository() : base("showed")
        {
            CreateTable();
        }

        protected override void DisposeStorageData()
        {
        }
    }
}
