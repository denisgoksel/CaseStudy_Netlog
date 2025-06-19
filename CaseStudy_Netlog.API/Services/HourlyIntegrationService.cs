using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using CaseStudy_Netlog.Core.Interfaces;

namespace CaseStudy_Netlog.API.Services
{
    public class HourlyIntegrationService : BackgroundService
    {
        private readonly ILogger<HourlyIntegrationService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public HourlyIntegrationService(ILogger<HourlyIntegrationService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("HourlyIntegrationService started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("HourlyIntegrationService running at: {time}", DateTimeOffset.Now);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var orderImportService = scope.ServiceProvider.GetRequiredService<IOrderImportService>();

                        var previousDay = DateTime.Today.AddDays(-1);

                        var orders = await orderImportService.GetOrdersFromSoapAsync(previousDay);
                        await orderImportService.SaveOrdersAsync(orders);
                        await orderImportService.SendDeliveredOrdersToRestApiAsync();
                    }

                    _logger.LogInformation("HourlyIntegrationService completed cycle at: {time}", DateTimeOffset.Now);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred in HourlyIntegrationService.");
                }

                // 1 saat bekleme (test amaçlı süreyi 1 dakika yapabilirsiniz)
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
