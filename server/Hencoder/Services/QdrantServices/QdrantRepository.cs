using Hencoder.Services.Repositories;
using Qdrant.Client.Grpc;
using ZeroLevel;
using ZeroLevel.Services.Collections;

namespace Hencoder.Services.QdrantServices
{
    public class vectors
    {
        public long video_id { get; set; }
        public byte[] vector { get; set; }
    }

    public enum QdrantResult
    {
        Success,
        QdrantFailure,
        DataFailure
    }
    public record QdrantFillResult(ulong PrevRecordsCount, ulong CurrentRecordsCount, long RecordsCount, QdrantResult Result);

    public sealed class QdrantRepository
        : BaseSqliteDB<vectors>
    {
        private readonly string _collectionName;
        private readonly string _connectionString;
        private const int VECTOR_SIZE = 312;
        public QdrantRepository(string connectionString, string collectionName)
            : base("video_emb")
        {
            _connectionString = connectionString;
            _collectionName = collectionName;
        }

        public async Task<QdrantFillResult> Fill()
        {
            Log.Debug("[Qdrant.Fill]");
            long total = 0;
            ulong prevRecordsCount;
            ulong currentRecordsCount;
            using (var proxy = new QdrantProxy(_collectionName))
            {
                currentRecordsCount = prevRecordsCount = await proxy.GetCollectionPointsCount();
                if (prevRecordsCount < 1000000)
                {
                    currentRecordsCount = 0;
                    await proxy.DeleteCollection();
                    // 1. Создание коллекции
                    try
                    {
                        await proxy.CreateCollection(Distance.Cosine, VECTOR_SIZE);
                    }
                    catch (Exception ex)
                    {
                        Console.Write($"[QdrantRepository.Fill] Fault create collection. {ex.Message}");
                        return new QdrantFillResult(0, 0, 0, QdrantResult.QdrantFailure);
                    }
                    try
                    {
                        // 2. Заполнение коллекции
                        var count = Count();
                        using (var processor = new BatchProcessor<QPoint>(1000, async points => await proxy.Upsert(points)))
                        {
                            foreach (var record in SelectAll())
                            {
                                var embedding = new float[312];
                                for (int i = 0; i < embedding.Length; i++)
                                {
                                    embedding[i] = BitConverter.ToSingle(record.vector, i * 4);
                                }
                                total++;
                                if (total % 5000 == 0)
                                {
                                    Log.Info($"[Qdrant.Fill] Proceed {total} / {count}");
                                }
                                processor.Add(new QPoint { Id = (ulong)record.video_id, Vector = embedding });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new QdrantFillResult(0, 0, 0, QdrantResult.DataFailure);
                    }
                    currentRecordsCount = await proxy.GetCollectionPointsCount();
                }
            }

            return new QdrantFillResult(prevRecordsCount, currentRecordsCount, total, QdrantResult.Success);
        }

        public Task<IEnumerable<long>> Recommend(ulong N, float scoreThreshold, IEnumerable<long> positivePoints, IEnumerable<long> negativePoints, IEnumerable<long> notAllowedPoints = null)
        {
            using (var proxy = new QdrantProxy(_collectionName))
            {
                return proxy.Recommend(N, scoreThreshold, positivePoints, negativePoints, notAllowedPoints);
            }
        }

        protected override void DisposeStorageData()
        {
        }
    }
}
