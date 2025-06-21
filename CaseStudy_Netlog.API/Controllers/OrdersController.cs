using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Include için gerekli
using System.Threading.Tasks;
using CaseStudy_Netlog.Core.Interfaces;
using CaseStudy_Netlog.Data.Entities;
using CaseStudy_Netlog.Data.Context;
using System;
using System.Linq;

namespace CaseStudy_Netlog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly AppDbContext _dbContext;

        // Constructor'a AppDbContext de eklendi
        public OrdersController(IOrderService orderService, AppDbContext dbContext)
        {
            _orderService = orderService;
            _dbContext = dbContext;
        }
        // GET: api/orders
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _dbContext.Orders
                .Include(o => o.OrderItems)
                .ToListAsync();

            var orderDtos = orders.Select(order => new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                DeliveryPoint = order.DeliveryPoint,
                ReceiverName = order.ReceiverName,
                ContactPhone = order.ContactPhone,
                Status = order.Status,
                Items = order.OrderItems.Select(item => new OrderItemDto
                {
                    ProductName = item.ProductName,
                    Quantity = item.Quantity
                }).ToList()
            }).ToList();

            return Ok(orderDtos);
        }


        // PUT: api/orders/5/mark-delivered
        [HttpPut("{id}/mark-delivered")]
        public async Task<IActionResult> MarkAsDelivered(int id)
        {
            var order = await _dbContext.Orders
                .Include(o => o.Delivery)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound("Sipariş bulunamadı.");

            if (order.Status >= 1)
                return BadRequest("Sipariş zaten teslim edilmiş.");

            // Teslimat kaydı ekle (test için sabit bilgiler giriliyor)
            order.Status = 1;
            order.Delivery = new Delivery
            {
                DeliveryDate = DateTime.Now,
                PlateNumber = "TEST-34XYZ123",
                DeliveredBy = "Test Kullanıcısı"
            };

            await _dbContext.SaveChangesAsync();

            return Ok("Sipariş teslim edildi olarak güncellendi ve teslimat bilgisi eklendi.");
        }
    }
}
