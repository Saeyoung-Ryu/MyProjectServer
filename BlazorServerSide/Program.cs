using Manager;
using Common;

namespace YourNamespace
{
    public class Program
    {
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        
        public static async Task Main(string[] args)
        {
            {
                // Rank Setting
                MyProjectInfoConfig.Refresh();
                await RankManager.InitRankAsync();
            }

            log.Info("Server Has Started");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls($"{MyProjectInfoConfig.Instance.ServerAddress}"); // this server url 내가세팅가능
                });
    }
}