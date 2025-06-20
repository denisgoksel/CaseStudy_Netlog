using CaseStudy_Netlog.Core.Dtos;
using CaseStudy_Netlog.Core.Interfaces;
using CaseStudy_Netlog.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CaseStudy_Netlog.API.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<DeliveryService> _logger;

        public DeliveryService(AppDbContext context, IHttpClientFactory httpClientFactory, ILogger<DeliveryService> logger)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task ProcessDeliveredOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.Delivery)               // Teslimat bilgisi için
                .Where(o => o.Status == 1)              // Sadece Teslim Edildi statüsü
                .ToListAsync();

            var httpClient = _httpClientFactory.CreateClient();

            foreach (var order in orders)
            {
                if (order.Delivery == null)
                {
                    _logger.LogWarning($"Order {order.Id} için teslimat bilgisi bulunamadı.");
                    continue;
                }

                var deliveryDto = new DeliveryDto
                {
                    OrderId = order.Id,
                    DeliveryDate = order.Delivery.DeliveryDate,
                    PlateNumber = order.Delivery.PlateNumber,
                    DeliveredBy = order.Delivery.DeliveredBy
                };

                var jsonContent = JsonSerializer.Serialize(deliveryDto);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                try
                {
                    var response = await httpClient.PutAsync("https://bcompany.com/api/deliveries", content);

                    if (response.IsSuccessStatusCode)
                    {
                        order.Status = 2;  // 2 = Tamamlandı
                        _logger.LogInformation($"Order {order.Id} başarıyla B firmasına gönderildi.");
                    }
                    else
                    {
                        _logger.LogWarning($"Order {order.Id} gönderilemedi. StatusCode: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Order {order.Id} gönderilirken hata oluştu.");
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
