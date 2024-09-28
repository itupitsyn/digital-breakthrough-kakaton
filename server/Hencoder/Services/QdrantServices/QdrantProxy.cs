using Qdrant.Client;
using Qdrant.Client.Grpc;


namespace Hencoder.Services.QdrantServices
{
    public class QdrantProxy
        : IDisposable
    {
        private readonly string _collectionName;
        private readonly string _url;
        public QdrantProxy(string collectionName)
        {
            _collectionName = collectionName;
        }

        public async Task<ulong> GetCollectionPointsCount()
        {
            using var client = new QdrantClient("localhost", 6334);
            return (await client.GetCollectionInfoAsync(_collectionName)).PointsCount;
        }

        public async Task CreateCollection(Distance distance, ulong size)
        {
            using var client = new QdrantClient("localhost", 6334);
            await client.CreateCollectionAsync(_collectionName,
                new VectorParams
                {
                    Distance = distance,
                    Size = size,
                });
        }

        public async Task Upsert(IEnumerable<QPoint> points)
        {
            using var client = new QdrantClient("localhost", 6334);
            await client.UpsertAsync(_collectionName, points.Select(p => new PointStruct
            {
                Id = p.Id,
                Vectors = p.Vector.ToArray(),
            }).ToList());
        }

        public async Task<IEnumerable<long>> Recommend(ulong N, float scoreThresholdValue, IEnumerable<long> positivePoints, IEnumerable<long> negativePoints, IEnumerable<long> notAllowedPoints = null)
        {
            Filter excludedPointsFilter = null;
            if (notAllowedPoints != null && notAllowedPoints.Any())
            {
                var condition = new Condition();
                condition.HasId = new HasIdCondition();
                foreach (var p in notAllowedPoints)
                {
                    condition.HasId.HasId.Add(new PointId { Num = (ulong)p });
                }
                excludedPointsFilter = new Filter();
                excludedPointsFilter.MustNot.Add(condition);
            }

            var query = new RecommendInput { Strategy = RecommendStrategy.BestScore };
            foreach (var p in positivePoints)
            {
                query.Positive.Add(new PointId { Num = (ulong)p });
            }
            foreach (var p in negativePoints)
            {
                query.Negative.Add(new PointId { Num = (ulong)p });
            }
            using var client = new QdrantClient("localhost", 6334);
            var points = await client.QueryAsync(
                collectionName: _collectionName,
                query: query,
                filter: excludedPointsFilter,
                limit: N,
                scoreThreshold: scoreThresholdValue
            );
            var set = new HashSet<long>();
            foreach (var point in points)
            {
                set.Add((long)point.Id.Num);
            }
            return set;
        }

        public async Task DeleteCollection()
        {
            using var client = new QdrantClient("localhost", 6334);
            await client.DeleteCollectionAsync(_collectionName);
        }

        public void Dispose()
        {
        }
    }
}
