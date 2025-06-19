using CaseStudy_Netlog.Core.Interfaces;
using CaseStudy_Netlog.Data.Context;
using CaseStudy_Netlog.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Net.Http.Headers;

namespace CaseStudy_Netlog.API.Services
{
    public class OrderImportService : IOrderImportService
    {
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _dbContext;

        public OrderImportService(HttpClient httpClient, AppDbContext dbContext)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<OrderDto>> GetOrdersFromSoapAsync(DateTime date)
        {
            string xmlDate = date.ToString("yyyy-MM-ddTHH:mm:ss");

            string soapRequest = $@"
<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
   <soapenv:Header/>
   <soapenv:Body>
      <tem:GetOrders>
         <tem:date>{xmlDate}</tem:date>
      </tem:GetOrders>
   </soapenv:Body>
</soapenv:Envelope>";

            var soapBytes = Encoding.UTF8.GetBytes(soapRequest);
            var content = new ByteArrayContent(soapBytes);
            content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
            // content.Headers.ContentLength = soapBytes.Length; // Bu satır gerek yok, HttpClient otomatik ayarlar

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5005/OrderService.svc")
            {
                Content = content
            };

            httpRequest.Headers.Add("SOAPAction", "\"http://tempuri.org/IOrderService/GetOrders\"");
            // httpRequest.Headers.Host = "localhost:5005"; // Genellikle bunu set etmene gerek yok

            var response = await _httpClient.SendAsync(httpRequest);

            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStreamAsync();

            // Deserialize için model isimleri ve namespace'ler tam uyumlu olmalı
            var serializer = new XmlSerializer(typeof(SoapEnvelope<GetOrdersResponseWrapper>));
            var envelope = (SoapEnvelope<GetOrdersResponseWrapper>)serializer.Deserialize(responseStream);

            // Null check yap
            if (envelope?.Body?.Response?.GetOrdersResult?.Orders == null)
                return new List<OrderDto>();

            return envelope.Body.Response.GetOrdersResult.Orders;
        }


        public async Task SaveOrdersAsync(List<OrderDto> orders)
        {
            if (orders == null || !orders.Any())
                return;

            foreach (var orderDto in orders)
            {
                var existingOrder = await _dbContext.Orders
                    .Include(o => o.OrderItems) // İlişkili itemları da çek
                    .FirstOrDefaultAsync(o => o.Id == orderDto.Id);

                if (existingOrder == null)
                {
                    var newOrder = new Order
                    {
                        //Id = orderDto.Id,
                        OrderDate = orderDto.OrderDate,
                        DeliveryPoint = orderDto.DeliveryPoint,
                        ReceiverName = orderDto.ReceiverName,
                        ContactPhone = orderDto.ContactPhone,
                        Status = orderDto.Status, // Soap'tan gelen status kullanılırsa bu şekilde
                        OrderItems = orderDto.Items?.Select(item => new OrderItem
                        {
                            ProductName = item.ProductName,
                            Quantity = item.Quantity
                        }).ToList() ?? new List<OrderItem>()
                    };

                    _dbContext.Orders.Add(newOrder);
                }
                else
                {
                    // İstersen güncelleme mantığı eklenir, şu an sadece yeni kayıt ekleniyor
                }
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task SendDeliveredOrdersToRestApiAsync()
        {
            var deliveredOrders = await _dbContext.Orders
                .Where(o => o.Status == 1)
                .Include(o => o.OrderItems)
                .ToListAsync();

            foreach (var order in deliveredOrders)
            {
                var deliveryDto = new DeliveryDto
                {
                    OrderId = order.Id,
                    DeliveryDate = DateTime.Now,
                    PlateNumber = "34ABC123",
                    DeliveredBy = "Sürücü Adı"
                };

                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(deliveryDto),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PutAsync(
                    $"https://BCompanyRestApiUrl/api/delivery/{order.Id}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    order.Status = 2; // Durum güncelle
                }
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
