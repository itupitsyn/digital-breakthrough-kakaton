namespace Hencoder.Models
{
    public class VideoStatEntry
    {
        /// <summary>
        /// уникальный ID видео
        /// </summary>
        public long video_id { get; set; }
        /// <summary>
        ///  заголовок видео
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// описание видео
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// категория видео
        /// </summary>
        public string category { get; set; }
        /// <summary>
        /// Идентификатор категории
        /// </summary>
        public long category_id { get; set; }
        /// <summary>
        /// дата публикации видео на платформе.Момент, когда видео стало доступно на Rutube в публичном доступе, округление до дня
        /// </summary>
        public long v_pub_datetime { get; set; }
        /// <summary>
        /// число положительных эмоций на видео в данный момент времени
        /// </summary>
        public ulong v_likes { get; set; }
        /// <summary>
        /// число отрицательных эмоций на видео в данный момент времени
        /// </summary>
        public ulong v_dislikes { get; set; }
        /// <summary>
        /// длительность видео
        /// </summary>
        public double v_duration { get; set; }
    }
}
