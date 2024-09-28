namespace RecSysConverter.Storages
{
    internal class UUID2IDVideosRepository : BaseSqliteDB<Uuid2IdEntry>
    {
        private readonly Dictionary<string, long> _cachee = new Dictionary<string, long>();

        public UUID2IDVideosRepository() : base("videos_ids")
        {
            CreateTable();
            foreach (var e in SelectAll())
            {
                _cachee[e.uuid] = e.id;
            }
        }

        public long GetNormalId(string uuid)
        {
            if (string.IsNullOrWhiteSpace(uuid)) return -1;
            if (_cachee.TryGetValue(uuid, out var id)) return id;
            var entry = new Uuid2IdEntry { uuid = uuid };
            Append(entry);
            var exists = Single(e => e.uuid == uuid);
            _cachee[exists.uuid] = exists.id;
            return exists.id;
        }

        protected override void DisposeStorageData() { }
    }
}
