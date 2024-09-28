namespace RecSysConverter.VideoStatsConvert
{
    internal class VideoStatRepository : BaseSqliteDB<VideoStatEntry>
    {
        public VideoStatRepository() : base("videostat")
        {
            CreateTable();
        }

        protected override void DisposeStorageData()
        {
        }
    }
}
