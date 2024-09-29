using Hencoder.Services.RecomendationSystem;

namespace Hencoder.Services.Repositories
{
    /// <summary>
    /// Показанные пользователю записи
    /// </summary>
    public class UserActionsRepository
        : BaseSqliteDB<RSUserAction>
    {
        public UserActionsRepository() : base("showed")
        {
            CreateTable();
        }

        protected override void DisposeStorageData()
        {
        }
    }
}
