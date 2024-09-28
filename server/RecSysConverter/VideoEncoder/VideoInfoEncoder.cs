namespace RecSysConverter.VideoEncoder
{
    internal static class VideoInfoEncoder
    {
        private class VideoStatEntry
        {
            public long video_id { get; set; }
            public string title { get; set; }
            public string description { get; set; }
        }

        private class VideoEntryReadOnlyRepository :
            BaseSqliteDB<VideoStatEntry>
        {
            public VideoEntryReadOnlyRepository() : base("videostat")
            {
            }

            protected override void DisposeStorageData()
            {
            }
        }

        public static async Task Encode()
        {
            var readRepository = new VideoEntryReadOnlyRepository();

            foreach (var entry in readRepository.SelectAll())
            {
                
            }
        }
    }
}
