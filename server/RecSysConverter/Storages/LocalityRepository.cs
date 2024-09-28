namespace RecSysConverter.Storages
{
    internal class LocalityRepository
        : BaseSqliteDB<LocalityEntry>
    {
        private readonly Dictionary<string, long> _cachee = new Dictionary<string, long>();
        private long _counter = 1;

        public LocalityRepository() : base("localities")
        {
            CreateTable();
            foreach (var e in SelectAll())
            {
                _cachee[e.Locality] = e.id;
            }
        }

        public long GetLocalityId(string region, string city)
        {
            var locality = $"{region}.{city}";
            if (string.IsNullOrWhiteSpace(locality)) return -1;
            var entry = new LocalityEntry(locality);
            Append(entry);
            var exists = Single(e => e.Locality == locality);
            _cachee[exists.Locality] = exists.id;
            return exists.id;
        }
        
        protected override void DisposeStorageData()
        {
        }
    }
}
