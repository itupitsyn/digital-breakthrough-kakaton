using Hencoder.Services;
using Parquet;
using Parquet.Schema;
using RecSysConverter.Storages;
using ZeroLevel;
using ZeroLevel.Services.Collections;

namespace RecSysConverter.VideoStatsConvert
{
    internal static class VideoStatConverter
    {
        private static VideoStatRepository _stat = new VideoStatRepository();
        private static UUID2LongCachee _videos = new UUID2LongCachee("videos", true);
        private static UUID2LongCachee _categories = new UUID2LongCachee("categories", true);
        private static UUID2LongCachee _authors = new UUID2LongCachee("authors", new UUID2LongCachee("users", true));

        #region Group data
        /// <summary>
        /// уникальный ID видео
        /// </summary>
        private static string[] video_id { get; set; }
        /// <summary>
        /// дата публикации видео на платформе.Момент, когда видео стало доступно на Rutube в публичном доступе, округление до дня
        /// </summary>
        private static DateTime?[] v_pub_datetime { get; set; }
        /// <summary>
        /// количество комментариев под видео
        /// </summary>
        private static ulong?[] v_total_comments { get; set; }
        /// <summary>
        /// полное число плеер стартов на видео
        /// </summary>
        private static ulong?[] v_year_views { get; set; }
        /// <summary>
        /// число плеер стартов на видео за последний месяц
        /// </summary>
        private static ulong?[] v_month_views { get; set; }
        /// <summary>
        /// число плеер стартов на видео за последнюю неделю
        /// </summary>
        private static ulong?[] v_week_views { get; set; }
        /// <summary>
        /// число плеер стартов на видео за последний день
        /// </summary>
        private static ulong?[] v_day_views { get; set; }
        /// <summary>
        /// число положительных эмоций на видео в данный момент времени
        /// </summary>
        private static ulong?[] v_likes { get; set; }
        /// <summary>
        /// число отрицательных эмоций на видео в данный момент времени
        /// </summary>
        private static ulong?[] v_dislikes { get; set; }
        /// <summary>
        /// длительность видео
        /// </summary>
        private static double?[] v_duration { get; set; }
        /// <summary>
        /// конверсия плеер старта в положительную эмоцию за последние 7 дней
        /// </summary>
        private static double?[] v_cr_click_like_7_days { get; set; }
        /// <summary>
        /// конверсия плеер старта в отрицательную эмоцию за последние 7 дней
        /// </summary>
        private static double?[] v_cr_click_dislike_7_days { get; set; }
        /// <summary>
        /// конверсия плеер старта в нажатие «втоп» за последние 7 дней
        /// </summary>
        private static double?[] v_cr_click_vtop_7_days { get; set; }
        /// <summary>
        /// конверсия плеер старта в долгий просмотр за последние 7 дней
        /// </summary>
        private static double?[] v_cr_click_long_view_7_days { get; set; }
        /// <summary>
        /// конверсия плеер старта в комментарий за последние 7 дней
        /// </summary>
        private static double?[] v_cr_click_comment_7_days { get; set; }
        /// <summary>
        /// конверсия плеер старта в положительную эмоцию за последние 30 дней
        /// </summary>
        private static double?[] v_cr_click_like_30_days { get; set; }
        /// <summary>
        /// конверсия плеер старта в отрицательную эмоцию за последние 30 дней
        /// </summary>
        private static double?[] v_cr_click_dislike_30_days { get; set; }
        /// <summary>
        /// конверсия плеер старта в нажатие «втоп» за последние 30 дней
        /// </summary>
        private static double?[] v_cr_click_vtop_30_days { get; set; }
        /// <summary>
        /// конверсия плеер старта в долгий просмотр за последние 30 дней
        /// </summary>
        private static double?[] v_cr_click_long_view_30_days { get; set; }
        /// <summary>
        /// конверсия плеер старта в комментарий за последние 30 дней
        /// </summary>
        private static double?[] v_cr_click_comment_30_days { get; set; }
        /// <summary>
        /// конверсия плеер старта в положительную эмоцию за последний день
        /// </summary>
        private static double?[] v_cr_click_like_1_days { get; set; }
        /// <summary>
        /// конверсия плеер старта в отрицательную эмоцию за последний день
        /// </summary>
        private static double?[] v_cr_click_dislike_1_days { get; set; }
        /// <summary>
        /// конверсия плеер старта в нажатие «втоп» за последний день
        /// </summary>
        private static double?[] v_cr_click_vtop_1_days { get; set; }
        /// <summary>
        /// конверсия плеер старта в долгий просмотр за последний день
        /// </summary>
        private static double?[] v_cr_click_long_view_1_days { get; set; }
        /// <summary>
        /// конверсия плеер старта в комментарий за последний день
        /// </summary>
        private static double?[] v_cr_click_comment_1_days { get; set; }
        /// <summary>
        ///  заголовок видео
        /// </summary>
        private static string[] title { get; set; }
        /// <summary>
        /// описание видео
        /// </summary>
        private static string[] description { get; set; }
        /// <summary>
        /// { get; set; }
        /// </summary>
        private static double?[] v_avg_watchtime_1_day { get; set; }
        /// <summary>
        /// средний watchtime на видео за 7 дней
        /// </summary>
        private static double?[] v_avg_watchtime_7_day { get; set; }
        /// <summary>
        /// средний watchtime на видео за 30 дней
        /// </summary>
        private static double?[] v_avg_watchtime_30_day { get; set; }
        /// <summary>
        /// средний watchtime на видео за 1 день в секундах делить на длительность видео в секундах
        /// </summary>
        private static double?[] v_frac_avg_watchtime_1_day_duration { get; set; }
        /// <summary>
        /// средний watchtime на видео за 7 дней делить на длительность видео в секундах
        /// </summary>
        private static double?[] v_frac_avg_watchtime_7_day_duration { get; set; }
        /// <summary>
        /// средний watchtime на видео за 30 дней делить на длительность видео в секундах
        /// </summary>
        private static double?[] v_frac_avg_watchtime_30_day_duration { get; set; }
        /// <summary>
        /// доля плеер стартов категории данного видео относительно всех плеер стартов за последние 7 дней
        /// </summary>
        private static double?[] v_category_popularity_percent_7_days { get; set; }
        /// <summary>
        /// доля плеер стартов категории данного видео относительно всех плеер стартов за последние 30 дней
        /// </summary>
        private static double?[] v_category_popularity_percent_30_days { get; set; }
        /// <summary>
        /// число плеер стартов с последующим "долгим" просмотром за 1 день
        /// </summary>
        private static ulong?[] v_long_views_1_days { get; set; }
        /// <summary>
        ///  число плеер стартов с последующим "долгим" просмотром за 7 дней
        /// </summary>
        private static ulong?[] v_long_views_7_days { get; set; }
        /// <summary>
        /// число плеер стартов с последующим "долгим" просмотром за 30 дней
        /// </summary>
        private static ulong?[] v_long_views_30_days { get; set; }
        /// <summary>
        /// ID автора видео
        /// </summary>
        private static string[] author_id { get; set; }
        /// <summary>
        /// ID категории видео
        /// </summary>
        private static string[] category_id { get; set; }
        #endregion

        internal static async Task Convert(string baseFolder)
        {
            var file = Path.Combine(baseFolder, "video_stat.parquet");
            using (var insertProcessor = new BatchProcessor<VideoStatEntry>(1000, records => _stat.Append(records)))
            {
                using (Stream fs = System.IO.File.OpenRead(file))
                {
                    using (ParquetReader reader = await ParquetReader.CreateAsync(fs))
                    {
                        for (int i = 0; i < reader.RowGroupCount; i++)
                        {
                            Log.Info($"Parse group {i} / {reader.RowGroupCount}");
                            CleanGroupData();
                            await ReadGroup(reader, i);
                            await ProceedGroupData(insertProcessor);
                        }
                    }
                }
            }
            _videos.Flush();
            _categories.Flush();
            _authors.Flush();
        }

        internal static void CleanGroupData()
        {
            video_id = null!;
            v_pub_datetime = null!;
            v_total_comments = null!;
            v_year_views = null!;
            v_month_views = null!;

            v_week_views = null!;
            v_day_views = null!;
            v_likes = null!;
            v_dislikes = null!;
            v_duration = null!;

            v_cr_click_like_7_days = null!;
            v_cr_click_dislike_7_days = null!;
            v_cr_click_vtop_7_days = null!;
            v_cr_click_long_view_7_days = null!;
            v_cr_click_comment_7_days = null!;

            v_cr_click_like_30_days = null!;
            v_cr_click_dislike_30_days = null!;
            v_cr_click_vtop_30_days = null!;
            v_cr_click_long_view_30_days = null!;
            v_cr_click_comment_30_days = null!;

            v_cr_click_like_1_days = null!;
            v_cr_click_dislike_1_days = null!;
            v_cr_click_vtop_1_days = null!;
            v_cr_click_long_view_1_days = null!;
            v_cr_click_comment_1_days = null!;

            title = null!;
            description = null!;
            v_avg_watchtime_1_day = null!;
            v_avg_watchtime_7_day = null!;
            v_avg_watchtime_30_day = null!;

            v_frac_avg_watchtime_1_day_duration = null!;
            v_frac_avg_watchtime_7_day_duration = null!;
            v_frac_avg_watchtime_30_day_duration = null!;
            v_category_popularity_percent_7_days = null!;
            v_category_popularity_percent_30_days = null!;

            v_long_views_1_days = null!;
            v_long_views_7_days = null!;
            v_long_views_30_days = null!;
            author_id = null!;
            category_id = null!;
        }

        internal static async Task ReadGroup(ParquetReader reader, int i)
        {
            using (ParquetRowGroupReader rowGroupReader = reader.OpenRowGroupReader(i))
            {
                foreach (DataField df in reader.Schema.GetDataFields())
                {
                    switch (df.Name)
                    {
                        case nameof(video_id): video_id = (string[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_pub_datetime): v_pub_datetime = (DateTime?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_total_comments): v_total_comments = (ulong?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_year_views): v_year_views = (ulong?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_month_views): v_month_views = (ulong?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;

                        case nameof(v_week_views): v_week_views = (ulong?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_day_views): v_day_views = (ulong?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_likes): v_likes = (ulong?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_dislikes): v_dislikes = (ulong?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_duration): v_duration = (double?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;

                        case nameof(v_cr_click_like_7_days): v_cr_click_like_7_days = (double?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_cr_click_dislike_7_days): v_cr_click_dislike_7_days = (double?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_cr_click_vtop_7_days): v_cr_click_vtop_7_days = (double?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_cr_click_long_view_7_days): v_cr_click_long_view_7_days = (double?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_cr_click_comment_7_days): v_cr_click_comment_7_days = (double?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;

                        case nameof(v_cr_click_like_30_days): v_cr_click_like_30_days = (double?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_cr_click_dislike_30_days): v_cr_click_dislike_30_days = (double?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_cr_click_vtop_30_days): v_cr_click_vtop_30_days = (double?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_cr_click_long_view_30_days): v_cr_click_long_view_30_days = (double?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_cr_click_comment_30_days): v_cr_click_comment_30_days = (double?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;

                        case nameof(v_cr_click_like_1_days): v_cr_click_like_1_days = (double?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_cr_click_dislike_1_days): v_cr_click_dislike_1_days = (double?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_cr_click_vtop_1_days): v_cr_click_vtop_1_days = (double?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_cr_click_long_view_1_days): v_cr_click_long_view_1_days = (double?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_cr_click_comment_1_days): v_cr_click_comment_1_days = (double?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;

                        case nameof(title): title = (string[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(description): description = (string[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_avg_watchtime_1_day): v_avg_watchtime_1_day = (double?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_avg_watchtime_7_day): v_avg_watchtime_7_day = (double?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_avg_watchtime_30_day): v_avg_watchtime_30_day = (double?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;

                        case nameof(v_frac_avg_watchtime_1_day_duration): v_frac_avg_watchtime_1_day_duration = (double?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_frac_avg_watchtime_7_day_duration): v_frac_avg_watchtime_7_day_duration = (double?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_frac_avg_watchtime_30_day_duration): v_frac_avg_watchtime_30_day_duration = (double?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_category_popularity_percent_7_days): v_category_popularity_percent_7_days = (double?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_category_popularity_percent_30_days): v_category_popularity_percent_30_days = (double?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;

                        case nameof(v_long_views_1_days): v_long_views_1_days = (ulong?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_long_views_7_days): v_long_views_7_days = (ulong?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(v_long_views_30_days): v_long_views_30_days = (ulong?[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(author_id): author_id = (string[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                        case nameof(category_id): category_id = (string[])((await rowGroupReader.ReadColumnAsync(df)).Data); break;
                    }
                }
            }
        }

        private static long GetValue(long?[] array, int index)
        {
            if (array == null) return default!;
            if (index >= array.Length || index < 0) return default!;
            return array[index] ?? 0;
        }

        private static ulong GetValue(ulong?[] array, int index)
        {
            if (array == null) return default!;
            if (index >= array.Length || index < 0) return default!;
            return array[index] ?? 0;
        }

        private static double GetValue(double?[] array, int index)
        {
            if (array == null) return default!;
            if (index >= array.Length || index < 0) return default!;
            return array[index] ?? 0;
        }
        private static DateTime GetValue(DateTime?[] array, int index)
        {
            if (array == null) return default!;
            if (index >= array.Length || index < 0) return default!;
            return array[index] ?? DateTime.MinValue;
        }

        private static string GetValue(string[] array, int index)
        {
            if (array == null) return default!;
            if (index >= array.Length || index < 0) return default!;
            return array[index] ?? string.Empty;
        }

        internal static async Task ProceedGroupData(BatchProcessor<VideoStatEntry> processor)
        {
            for (int i = 0; i < video_id.Length; i++)
            {
                var entry = new VideoStatEntry();

                entry.video_id = _videos.GetNormalId(video_id[i]);
                entry.v_pub_datetime = Timestamp.FromDateTimeOffset(GetValue(v_pub_datetime, i));
                entry.v_total_comments = GetValue(v_total_comments, i);
                entry.v_year_views = GetValue(v_year_views, i);
                entry.v_month_views = GetValue(v_month_views, i);

                entry.v_week_views = GetValue(v_week_views, i);
                entry.v_day_views = GetValue(v_day_views, i);
                entry.v_likes = GetValue(v_likes, i);
                entry.v_dislikes = GetValue(v_dislikes, i);
                entry.v_duration = GetValue(v_duration, i);

                entry.v_cr_click_like_7_days = GetValue(v_cr_click_like_7_days, i);
                entry.v_cr_click_dislike_7_days = GetValue(v_cr_click_dislike_7_days, i);
                entry.v_cr_click_vtop_7_days = GetValue(v_cr_click_vtop_7_days, i);
                entry.v_cr_click_long_view_7_days = GetValue(v_cr_click_long_view_7_days, i);
                entry.v_cr_click_comment_7_days = GetValue(v_cr_click_comment_7_days, i);

                entry.v_cr_click_like_30_days = GetValue(v_cr_click_like_30_days, i);
                entry.v_cr_click_dislike_30_days = GetValue(v_cr_click_dislike_30_days, i);
                entry.v_cr_click_vtop_30_days = GetValue(v_cr_click_vtop_30_days, i);
                entry.v_cr_click_long_view_30_days = GetValue(v_cr_click_long_view_30_days, i);
                entry.v_cr_click_comment_30_days = GetValue(v_cr_click_comment_30_days, i);

                entry.v_cr_click_like_1_days = GetValue(v_cr_click_like_1_days, i);
                entry.v_cr_click_dislike_1_days = GetValue(v_cr_click_dislike_1_days, i);
                entry.v_cr_click_vtop_1_days = GetValue(v_cr_click_vtop_1_days, i);
                entry.v_cr_click_long_view_1_days = GetValue(v_cr_click_long_view_1_days, i);
                entry.v_cr_click_comment_1_days = GetValue(v_cr_click_comment_1_days, i);

                entry.title = GetValue(title, i);
                entry.description = GetValue(description, i);
                entry.v_avg_watchtime_1_day = GetValue(v_avg_watchtime_1_day, i);
                entry.v_avg_watchtime_7_day = GetValue(v_avg_watchtime_7_day, i);
                entry.v_avg_watchtime_30_day = GetValue(v_avg_watchtime_30_day, i);

                entry.v_frac_avg_watchtime_1_day_duration = GetValue(v_frac_avg_watchtime_1_day_duration, i);
                entry.v_frac_avg_watchtime_7_day_duration = GetValue(v_frac_avg_watchtime_7_day_duration, i);
                entry.v_frac_avg_watchtime_30_day_duration = GetValue(v_frac_avg_watchtime_30_day_duration, i);
                entry.v_category_popularity_percent_7_days = GetValue(v_category_popularity_percent_7_days, i);
                entry.v_category_popularity_percent_30_days = GetValue(v_category_popularity_percent_30_days, i);

                entry.v_long_views_1_days = GetValue(v_long_views_1_days, i);
                entry.v_long_views_7_days = GetValue(v_long_views_7_days, i);
                entry.v_long_views_30_days = GetValue(v_long_views_30_days, i);
                entry.author_id = _authors.GetNormalId(GetValue(author_id, i));
                entry.category_id = _categories.GetNormalId(GetValue(category_id, i));

                processor.Add(entry);
            }
        }
    }
}
