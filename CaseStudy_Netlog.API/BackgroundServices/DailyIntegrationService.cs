using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using CaseStudy_Netlog.Core.Interfaces;

namespace CaseStudy_Netlog.API.BackgroundServices
{
    public class DailyIntegrationService : BackgroundService
    {
        private readonly ILogger<DailyIntegrationService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public DailyIntegrationService(ILogger<DailyIntegrationService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("DailyIntegrationService started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("DailyIntegrationService running at: {time}", DateTimeOffset.Now);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var orderImportService = scope.ServiceProvider.GetRequiredService<IOrderImportService>();

                        var previousDay = DateTime.Today.AddDays(-1);

                        var orders = await orderImportService.GetOrdersFromSoapAsync(previousDay);
                        await orderImportService.SaveOrdersAsync(orders);

                        // Teslim edilen siparişleri REST API'ye gönder
                        await orderImportService.SendDeliveredOrdersToRestApiAsync();
                    }

                    _logger.LogInformation("DailyIntegrationService completed cycle at: {time}", DateTimeOffset.Now);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred in DailyIntegrationService.");
                }

                // Günde 1 kere çalışacak şekilde 24 saat bekle
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
    }
}
