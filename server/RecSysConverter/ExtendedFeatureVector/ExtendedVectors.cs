using SQLite;

namespace RecSysConverter.ExtendedFeatureVector
{
    internal class ExtendedVectors
    {
        [Unique, PrimaryKey]
        public long video_id {  get; set; }
        public byte[] vector {  get; set; }
    }
}
