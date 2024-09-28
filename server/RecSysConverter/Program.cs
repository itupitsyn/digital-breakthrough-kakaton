using RecSysConverter.LogsConvert;
using ZeroLevel;

namespace RecSysConverter
{
    internal class Program
    {
        const string BasePath = @"D:\cold_start_train_2";
        
        static async Task Main(string[] args)
        {
            Log.AddConsoleLogger();
            await LogsConverter.Convert(BasePath);
            // await VideoStatConverter.Convert(BasePath);
            // await VideoInfoEncoder.Encode();
            // TrainSetBuilder.Build();
        }
    }
}
