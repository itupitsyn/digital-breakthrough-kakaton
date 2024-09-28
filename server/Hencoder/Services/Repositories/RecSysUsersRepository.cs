using Hencoder.Services.RecomendationSystem;
using ZeroLevel;

namespace Hencoder.Services.Repositories
{
    public class RecSysUsersRepository
        : BaseSqliteDB<RSUser>
    {
        private readonly Dictionary<string, long> _cachee = new Dictionary<string, long>();

        public RecSysUsersRepository() : base("rs_users")
        {
            CreateTable();
            foreach (var item in SelectAll())
            {
                _cachee[item.token] = item.id;
            }
        }

        public long GetId(string token) => _cachee.TryGetValue(token, out long id) ? id : -1;

        public bool Create(string token)
        {
            if (Append(new RSUser { token = token }) == 1)
            {
                var created = Single(r=>r.token == token);
                _cachee[token] = created.id;
                return true;
            }
            Log.Warning("[RecSysUsersRepository.Create] Fault create user.");
            return false;
        }

        protected override void DisposeStorageData()
        {
        }
    }
}
