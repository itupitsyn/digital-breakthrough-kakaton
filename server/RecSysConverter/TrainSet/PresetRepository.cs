namespace RecSysConverter.TrainSet
{
    internal class PresetRepository
        : BaseSqliteDB<TrainPreSet>
    {
        public PresetRepository() : base("videostat")
        {
        }

        protected override void DisposeStorageData()
        {
        }
    }
}
