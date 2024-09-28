using SQLite;

namespace RecSysConverter.LogsConvert
{
    internal class LogEntry
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public long id { get; set; }
        /// <summary>
        /// таймстемп события
        /// </summary>
        public long event_timestamp { get; set; }
        /// <summary>
        /// уникальный ID видео
        /// </summary>
        [Indexed]
        public long video_id { get; set; }
        /// <summary>
        /// время просмотра в секундах
        /// </summary>
        public long watchtime { get; set; }
        /// <summary>
        /// регион расположения пользователя
        /// </summary>
        public string region { get; set; }
        /// <summary>
        /// город расположения пользователя
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// уникальный ID пользователя
        /// </summary>
        [Indexed]
        public long user_id { get; set; }
        /// <summary>
        /// Идентификатор месторасположения ползователя
        /// </summary>
        [Indexed]
        public long locality_id { get; set; }
    }
}
