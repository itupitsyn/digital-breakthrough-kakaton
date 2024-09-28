namespace Hencoder.Services.QdrantServices
{
    public class QPoint
    {
        public ulong Id { get; set; }
        public IEnumerable<float> Vector { get; set; }
    }
}
