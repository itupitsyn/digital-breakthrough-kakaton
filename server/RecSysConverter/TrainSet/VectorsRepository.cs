namespace RecSysConverter.TrainSet
{
    internal class VectorsRepository
        : BaseSqliteDB<vectors>
    {
        public VectorsRepository() : base("video_emb")
        {
        }

        protected override void DisposeStorageData()
        {
        }
    }
}
