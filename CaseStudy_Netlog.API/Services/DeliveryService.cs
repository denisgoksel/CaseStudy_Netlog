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
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ProcessDeliveredOrdersAsync()
        {
            // Status == 1 olan, teslim edildi olarak işaretlenen siparişleri getiriyoruz
            var orders = await _context.Orders
                .Include(o => o.Delivery)
                .Where(o => o.Status == 1)
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
                    // B firması API'sine teslimat bilgisi gönderiliyor (localhost örnek)
                    var response = await httpClient.PutAsync("https://localhost:5001/api/delivery/update-deliveries", content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Başarılıysa durum 2 = Tamamlandı olarak güncelleniyor
                        order.Status = 2;
                        _context.Entry(order).State = EntityState.Modified;
                        _logger.LogInformation($"Order {order.Id} başarıyla güncellendi ve B firmasına gönderildi.");
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
