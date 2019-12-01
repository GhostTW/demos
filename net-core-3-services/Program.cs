using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace NetCore3Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (Directory.Exists("/var/log") == false)
                Directory.CreateDirectory("/var/log");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSystemd()
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                });
    }
}
