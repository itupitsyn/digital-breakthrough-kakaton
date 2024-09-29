using Hencoder.Services;
using Hencoder.Services.QdrantServices;
using Hencoder.Services.RecomendationSystem;
using System.Text;
using ZeroLevel;

namespace Hencoder
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var config = Configuration.ReadSetFromIniFile("config.ini");

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Log.AddConsoleLogger(ZeroLevel.Logging.LogLevel.FullDebug);

            var qdrantRepository = new QdrantRepository("video", config.Default.First("vector_db"));

            await qdrantRepository.Fill();            

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<RecSys>(new RecSys(qdrantRepository));

            builder.Services.AddControllers();

            var app = builder.Build();

            app.UseAuthMiddleware();

            app.MapControllers();

            app.Run();
        }
    }
}
