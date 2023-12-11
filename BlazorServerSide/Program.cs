using Manager;
using BlazorServerSide;
using Common;
using Manager;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace YourNamespace
{
    public class Program
    {
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        
        public static async Task Main(string[] args)
        {
            {
                // DB Setting
                MyProjectInfoConfig.Refresh();
                await RankManager.SetOverallRankInfoListAsync();
                await RankManager.SetOtherLaneRanks();
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