namespace RecSysConverter.ExtendedFeatureVector
{
    internal class ExtendedVectorRepository
        : BaseSqliteDB<ExtendedVectors>
    {
        public ExtendedVectorRepository() : base("extended")
        {
            CreateTable();
        }

        protected override void DisposeStorageData()
        {
        }
    }
}
