using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace NetCore3Service
{
    public class Worker : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                File.AppendAllText("/var/log/netcore3service.log", $"Worker running at: {DateTime.Now}{Environment.NewLine}");
                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}
