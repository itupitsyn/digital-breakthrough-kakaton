namespace RecSysConverter.LogsConvert
{
    internal class LogRepository
        : BaseSqliteDB<LogEntry>
    {
        public LogRepository() : base("logs")
        {
            CreateTable();
        }

        protected override void DisposeStorageData()
        {
        }
    }
}
