using SQLite;

namespace RecSysConverter.Storages
{
    internal class LocalityEntry
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public long id { get; set; }
        [Indexed, NotNull]
        public string Locality { get; set; }

        public LocalityEntry() { }

        public LocalityEntry(string locality) { Locality = locality; }
    }
}
