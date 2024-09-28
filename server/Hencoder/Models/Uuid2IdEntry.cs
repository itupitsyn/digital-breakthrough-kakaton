using SQLite;

namespace Hencoder.Models
{
    internal class Uuid2IdEntry
    {
        /// <summary>
        /// Нормальный идентификатор
        /// </summary>
        [PrimaryKey, AutoIncrement, NotNull]
        public long id { get; set; }
        /// <summary>
        /// Идентификатор в виде uuid
        /// </summary>
        public string uuid { get; set; }
    }
}
