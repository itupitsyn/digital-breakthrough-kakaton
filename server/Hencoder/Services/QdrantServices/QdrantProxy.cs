using Qdrant.Client;
using Qdrant.Client.Grpc;


namespace Hencoder.Services.QdrantServices
{
    public class QdrantProxy
        : IDisposable
    {
        private readonly string _collectionName;
        private readonly string _url;
        private readonly QdrantClient _client;
        public QdrantProxy(string collectionName)
        {
            _collectionName = collectionName;
            _client = new QdrantClient("localhost", 6334);
        }

        public async Task<ulong> GetCollectionPointsCount()
        {
            if (await _client.CollectionExistsAsync(_collectionName))
            {
                return (await _client.GetCollectionInfoAsync(_collectionName)).PointsCount;
            }
            return 0;
        }

        public async Task CreateCollection(Distance distance, ulong size)
        {
            if (await _client.CollectionExistsAsync(_collectionName) == false)
            {
                await _client.CreateCollectionAsync(_collectionName,
                    new VectorParams
                    {
                        Distance = distance,
                        Size = size,
                    });
            }
        }

        public async Task Upsert(IEnumerable<QPoint> points)
        {
            if (await _client.CollectionExistsAsync(_collectionName))
            {
                await _client.UpsertAsync(_collectionName, points.Select(p => new PointStruct
                {
                    Id = p.Id,
                    Vectors = p.Vector.ToArray(),
                }).ToList());
            }
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
            var set = new HashSet<long>();
            if (await _client.CollectionExistsAsync(_collectionName))
            {
                var points = await _client.RecommendAsync(_collectionName, positive: positivePoints.Select(p => (ulong)p).ToArray(), negative: negativePoints.Select(p => (ulong)p).ToArray(), limit: N, filter: excludedPointsFilter, strategy: RecommendStrategy.AverageVector);
                foreach (var point in points)
                {
                    set.Add((long)point.Id.Num);
                }
            }
            return set;
        }

        public async Task DeleteCollection()
        {
            if (await _client.CollectionExistsAsync(_collectionName))
            {
                await _client.DeleteCollectionAsync(_collectionName);
            }

        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
