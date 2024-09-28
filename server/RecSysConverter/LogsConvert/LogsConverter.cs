using Hencoder.Services;
using Parquet;
using Parquet.Data;
using Parquet.Schema;
using RecSysConverter.Storages;
using ZeroLevel;
using ZeroLevel.Services.Collections;

namespace RecSysConverter.LogsConvert
{
    internal static class LogsConverter
    {
        static LogRepository _logs = new LogRepository();
        static UUID2LongCachee _localities = new UUID2LongCachee("localities", true);
        static UUID2LongCachee _users = new UUID2LongCachee("users", true);
        static UUID2LongCachee _videos = new UUID2LongCachee("videos", true);

        internal static async Task Convert(string baseFolder)
        {
            DateTime?[] event_timestamp = null;
            string[] video_id = null;
            long?[] watchtime = null;
            string[] region = null;
            string[] city = null;
            string[] user_id = null;

            using (var insertProcessor = new BatchProcessor<LogEntry>(200000, records => _logs.Append(records)))
            {
                foreach (var file in Directory.EnumerateFiles(baseFolder).Where(f => Path.GetFileName(f).StartsWith("logs")))
                {
                    Log.Info($"Parse log file '{file}'");
                    using (Stream fs = System.IO.File.OpenRead(file))
                    {
                        using (ParquetReader reader = await ParquetReader.CreateAsync(fs))
                        {
                            for (int i = 0; i < reader.RowGroupCount; i++)
                            {
                                Log.Info($"Parse group {i} / {reader.RowGroupCount}");
                                using (ParquetRowGroupReader rowGroupReader = reader.OpenRowGroupReader(i))
                                {
                                    foreach (DataField df in reader.Schema.GetDataFields())
                                    {
                                        if (df.Name == "event_timestamp")
                                        {
                                            DataColumn columnData = await rowGroupReader.ReadColumnAsync(df);
                                            event_timestamp = (DateTime?[])columnData.Data;
                                        }
                                        if (df.Name == "video_id")
                                        {
                                            DataColumn columnData = await rowGroupReader.ReadColumnAsync(df);
                                            video_id = (string[])columnData.Data;
                                        }
                                        if (df.Name == "watchtime")
                                        {
                                            DataColumn columnData = await rowGroupReader.ReadColumnAsync(df);
                                            watchtime = (long?[])columnData.Data;
                                        }
                                        if (df.Name == "region")
                                        {
                                            DataColumn columnData = await rowGroupReader.ReadColumnAsync(df);
                                            region = (string[])columnData.Data;
                                        }
                                        if (df.Name == "city")
                                        {
                                            DataColumn columnData = await rowGroupReader.ReadColumnAsync(df);
                                            city = (string[])columnData.Data;
                                        }
                                        if (df.Name == "user_id")
                                        {
                                            DataColumn columnData = await rowGroupReader.ReadColumnAsync(df);
                                            user_id = (string[])columnData.Data;
                                        }
                                    }
                                }

                                
                                for (int index = 0; index < video_id.Length; index++)
                                {
                                    var logEntry = new LogEntry();
                                    logEntry.region = region[index] ?? string.Empty;
                                    logEntry.city = city[index] ?? string.Empty;
                                    if (string.IsNullOrWhiteSpace(logEntry.city) == false || string.IsNullOrWhiteSpace(logEntry.region) == false)
                                    {
                                        logEntry.locality_id = _localities.GetNormalId($"{logEntry.region}.{logEntry.city}");
                                    }
                                    else
                                    {
                                        logEntry.locality_id = -1;
                                    }
                                    logEntry.event_timestamp = Timestamp.FromDateTimeOffset(event_timestamp[index] ?? DateTime.MinValue);
                                    logEntry.video_id = _videos.GetNormalId(video_id[index]);
                                    logEntry.watchtime = watchtime[index] ?? 0;
                                    logEntry.user_id = _users.GetNormalId(user_id[index]);
                                    if (logEntry.video_id != -1 && logEntry.user_id != -1)
                                    {
                                        insertProcessor.Add(logEntry);
                                    }
                                    if (index % 10000 == 0)
                                    {
                                        Log.Info($"Inserted {index} / {video_id.Length}");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            _localities.Flush();
            _users.Flush();
            _videos.Flush();
        }
    }
}
