using Hencoder.Models;

namespace Hencoder.Services.Repositories
{
    internal class UUID2LongCachee
       : BaseSqliteDB<Uuid2IdEntry>
    {
        private readonly Dictionary<string, long> _cachee = new Dictionary<string, long>();

        public UUID2LongCachee(string name)
            : base(name)
        {
            foreach (var e in SelectAll())
            {
                _cachee[e.uuid] = e.id;
            }
        }

        public IEnumerable<KeyValuePair<string, long>> ReadAll() => _cachee;

        protected override void DisposeStorageData()
        {

        }
    }
}
