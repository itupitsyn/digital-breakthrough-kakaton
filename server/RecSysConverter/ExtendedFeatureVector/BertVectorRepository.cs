namespace RecSysConverter.ExtendedFeatureVector
{
    internal class BertVectorRepository
        : BaseSqliteDB<vectors>
    {
        public BertVectorRepository() : base("video_emb")
        {
        }

        protected override void DisposeStorageData()
        {
        }
    }
}
