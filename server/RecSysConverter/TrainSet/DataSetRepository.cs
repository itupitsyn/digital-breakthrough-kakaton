namespace RecSysConverter.TrainSet
{
    internal class DataSetRepository
        : BaseSqliteDB<DataSet>
    {
        public DataSetRepository() : base("trainset")
        {
            CreateTable();
        }

        protected override void DisposeStorageData()
        {
        }
    }
}
