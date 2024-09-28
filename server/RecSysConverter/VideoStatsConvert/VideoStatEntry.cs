using SQLite;

namespace RecSysConverter.VideoStatsConvert
{
    internal class VideoStatEntry
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public long id { get; set; }
        /// <summary>
        /// уникальный ID видео
        /// </summary>
        [Indexed]
        public long video_id { get; set; }
        /// <summary>
        /// дата публикации видео на платформе.Момент, когда видео стало доступно на Rutube в публичном доступе, округление до дня
        /// </summary>
        public long v_pub_datetime { get; set; }
        /// <summary>
        /// количество комментариев под видео
        /// </summary>
        public ulong v_total_comments { get; set; }
        /// <summary>
        /// полное число плеер стартов на видео
        /// </summary>
        public ulong v_year_views { get; set; }
        /// <summary>
        /// число плеер стартов на видео за последний месяц
        /// </summary>
        public ulong v_month_views { get; set; }
        /// <summary>
        /// число плеер стартов на видео за последнюю неделю
        /// </summary>
        public ulong v_week_views { get; set; }
        /// <summary>
        /// число плеер стартов на видео за последний день
        /// </summary>
        public ulong v_day_views { get; set; }
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
        /// <summary>
        /// конверсия плеер старта в положительную эмоцию за последние 7 дней
        /// </summary>
        public double v_cr_click_like_7_days { get; set; }
        /// <summary>
        /// конверсия плеер старта в отрицательную эмоцию за последние 7 дней
        /// </summary>
        public double v_cr_click_dislike_7_days { get; set; }
        /// <summary>
        /// конверсия плеер старта в нажатие «втоп» за последние 7 дней
        /// </summary>
        public double v_cr_click_vtop_7_days { get; set; }
        /// <summary>
        /// конверсия плеер старта в долгий просмотр за последние 7 дней
        /// </summary>
        public double v_cr_click_long_view_7_days { get; set; }
        /// <summary>
        /// конверсия плеер старта в комментарий за последние 7 дней
        /// </summary>
        public double v_cr_click_comment_7_days { get; set; }
        /// <summary>
        /// конверсия плеер старта в положительную эмоцию за последние 30 дней
        /// </summary>
        public double v_cr_click_like_30_days { get; set; }
        /// <summary>
        /// конверсия плеер старта в отрицательную эмоцию за последние 30 дней
        /// </summary>
        public double v_cr_click_dislike_30_days { get; set; }
        /// <summary>
        /// конверсия плеер старта в нажатие «втоп» за последние 30 дней
        /// </summary>
        public double v_cr_click_vtop_30_days { get; set; }
        /// <summary>
        /// конверсия плеер старта в долгий просмотр за последние 30 дней
        /// </summary>
        public double v_cr_click_long_view_30_days { get; set; }
        /// <summary>
        /// конверсия плеер старта в комментарий за последние 30 дней
        /// </summary>
        public double v_cr_click_comment_30_days { get; set; }
        /// <summary>
        /// конверсия плеер старта в положительную эмоцию за последний день
        /// </summary>
        public double v_cr_click_like_1_days { get; set; }
        /// <summary>
        /// конверсия плеер старта в отрицательную эмоцию за последний день
        /// </summary>
        public double v_cr_click_dislike_1_days { get; set; }
        /// <summary>
        /// конверсия плеер старта в нажатие «втоп» за последний день
        /// </summary>
        public double v_cr_click_vtop_1_days { get; set; }
        /// <summary>
        /// конверсия плеер старта в долгий просмотр за последний день
        /// </summary>
        public double v_cr_click_long_view_1_days { get; set; }
        /// <summary>
        /// конверсия плеер старта в комментарий за последний день
        /// </summary>
        public double v_cr_click_comment_1_days { get; set; }
        /// <summary>
        ///  заголовок видео
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// описание видео
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// { get; set; }
        /// </summary>
        public double v_avg_watchtime_1_day { get; set; }
        /// <summary>
        /// средний watchtime на видео за 7 дней
        /// </summary>
        public double v_avg_watchtime_7_day { get; set; }
        /// <summary>
        /// средний watchtime на видео за 30 дней
        /// </summary>
        public double v_avg_watchtime_30_day { get; set; }
        /// <summary>
        /// средний watchtime на видео за 1 день в секундах делить на длительность видео в секундах
        /// </summary>
        public double v_frac_avg_watchtime_1_day_duration { get; set; }
        /// <summary>
        /// средний watchtime на видео за 7 дней делить на длительность видео в секундах
        /// </summary>
        public double v_frac_avg_watchtime_7_day_duration { get; set; }
        /// <summary>
        /// средний watchtime на видео за 30 дней делить на длительность видео в секундах
        /// </summary>
        public double v_frac_avg_watchtime_30_day_duration { get; set; }
        /// <summary>
        /// доля плеер стартов категории данного видео относительно всех плеер стартов за последние 7 дней
        /// </summary>
        public double v_category_popularity_percent_7_days { get; set; }
        /// <summary>
        /// доля плеер стартов категории данного видео относительно всех плеер стартов за последние 30 дней
        /// </summary>
        public double v_category_popularity_percent_30_days { get; set; }
        /// <summary>
        /// число плеер стартов с последующим "долгим" просмотром за 1 день
        /// </summary>
        public ulong v_long_views_1_days { get; set; }
        /// <summary>
        ///  число плеер стартов с последующим "долгим" просмотром за 7 дней
        /// </summary>
        public ulong v_long_views_7_days { get; set; }
        /// <summary>
        /// число плеер стартов с последующим "долгим" просмотром за 30 дней
        /// </summary>
        public ulong v_long_views_30_days { get; set; }
        /// <summary>
        /// ID автора видео
        /// </summary>
        [Indexed]
        public long author_id { get; set; }
        /// <summary>
        /// ID категории видео
        /// </summary>
        [Indexed]
        public long category_id { get; set; }
    }
}
