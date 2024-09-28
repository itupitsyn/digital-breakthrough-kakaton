using Hencoder.Models;

namespace Hencoder.Services.Repositories
{
    internal class VideoStatRepository : BaseSqliteDB<VideoStatEntry>
    {
        public VideoStatRepository() : base("videostat")
        {
        }

        protected override void DisposeStorageData()
        {
        }
    }
}
