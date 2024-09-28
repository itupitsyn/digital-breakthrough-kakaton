using Hencoder.Services;
using Hencoder.Services.QdrantServices;
using Hencoder.Services.RecomendationSystem;
using System.Diagnostics;
using System.Net;
using System.Text;
using ZeroLevel;
using static System.Net.WebRequestMethods;

namespace Hencoder
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Log.AddConsoleLogger(ZeroLevel.Logging.LogLevel.FullDebug);

            // Start qdrant
            /*Process qdrantProcess = new Process();
            qdrantProcess.StartInfo.FileName = "binary/qdrant.exe";
            qdrantProcess.Start();*/

            var qdrantRepository = new QdrantRepository("http://localhost:6334", "video");
            await qdrantRepository.Fill();            

            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.ConfigureKestrel((context, serverOptions) =>
            {
                var kestrelSection = context.Configuration.GetSection("Kestrel");
                serverOptions.Configure(kestrelSection)
                    .Endpoint("HTTP", listenOptions =>
                    {
                        serverOptions.Listen(IPAddress.Any, 8077);
                    });
            });

            builder.Services.AddSingleton<RecSys>(new RecSys(qdrantRepository));

            builder.Services.AddControllers();

            var app = builder.Build();

            app.UseAuthMiddleware();

            app.MapControllers();

            app.Run();
        }
    }
}
