namespace RecSysConverter.VideoEncoder
{
    internal class EncodedVideoInfoRepository
        : BaseSqliteDB<EncodedVideoInfo>
    {
        public EncodedVideoInfoRepository() : base("vectors")
        {
        }

        protected override void DisposeStorageData()
        {
        }
    }
}
