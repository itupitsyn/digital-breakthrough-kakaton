using SQLite;

namespace RecSysConverter.VideoEncoder
{
    internal class EncodedVideoInfo
    {
        [PrimaryKey, NotNull]
        public long video_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public byte[] vector { get; set; }
    }
}