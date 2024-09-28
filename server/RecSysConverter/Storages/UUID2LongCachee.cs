using ZeroLevel.Services.Collections;

namespace RecSysConverter.Storages
{
    internal class UUID2LongCachee
        : BaseSqliteDB<Uuid2IdEntry>
    {
        private readonly Dictionary<string, long> _cachee = new Dictionary<string, long>();
        private long _counter = 1;

        public UUID2LongCachee(string name, bool preload = false)
            : base(name)
        {
            if (!preload)
            {
                DropTable();
            }
            CreateTable();
            if (preload)
            {
                long max = 1;
                foreach (var e in SelectAll())
                {
                    _cachee[e.uuid] = e.id;
                    if (max < e.id) max = e.id;
                }
                _counter = max + 1;
            }
        }

        public UUID2LongCachee(string name, UUID2LongCachee preloadFrom)
            : base(name)
        {
            DropTable();
            CreateTable();
            long max = 1;
            foreach (var e in preloadFrom.ReadAll())
            {
                _cachee[e.Key] = e.Value;
                if (max < e.Value) max = e.Value;
            }
            _counter = max + 1;
        }

        public IEnumerable<KeyValuePair<string, long>> ReadAll() => _cachee;

        public long GetNormalId(string uuid)
        {
            if (string.IsNullOrWhiteSpace(uuid)) return -1;
            if (_cachee.TryGetValue(uuid, out var id)) return id;
            _counter++;
            _cachee[uuid] = _counter;
            return _counter;
        }

        public void Flush()
        {
            DropTable();
            CreateTable();
            using (BatchProcessor<Uuid2IdEntry> _processor = new BatchProcessor<Uuid2IdEntry>(1000, records => Append(records)))
            {
                foreach (var pair in _cachee)
                {
                    _processor.Add(new Uuid2IdEntry { uuid = pair.Key, id = pair.Value });
                }
            }
        }

        protected override void DisposeStorageData()
        {

        }
    }
}
