﻿using CaseStudy_Netlog.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

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
        _logger.LogInformation("HourlyIntegrationService başladı.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var deliveryService = scope.ServiceProvider.GetRequiredService<IDeliveryService>();
                    await deliveryService.ProcessDeliveredOrdersAsync();
                }

                _logger.LogInformation("HourlyIntegrationService tamamlandı.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hata: HourlyIntegrationService.");
            }
            //Test:olarak FromMinutes(1) yapıldı.
            //FromHours(1) Olması gerekiyor.
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
