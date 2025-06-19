using CaseStudy_Netlog.Core.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection; // Bu using eklenecek
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseStudy_Netlog.API.BackgroundServices
{
    public class SoapOrderPollingService : BackgroundService
    {
        private readonly ILogger<SoapOrderPollingService> _logger;
        private readonly IServiceProvider _serviceProvider; // IServiceProvider eklendi

        public SoapOrderPollingService(ILogger<SoapOrderPollingService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var date = DateTime.Today.AddDays(-1); // Önceki günün siparişlerini çek
                    _logger.LogInformation($"[SOAP CHECK] Başlatıldı: {date}");

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var orderImportService = scope.ServiceProvider.GetRequiredService<IOrderImportService>();

                        var orders = await orderImportService.GetOrdersFromSoapAsync(date);
                        await orderImportService.SaveOrdersAsync(orders);

                        _logger.LogInformation($"[SOAP CHECK] {orders.Count} sipariş kaydedildi.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "SOAP sipariş çekme hatası.");
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
