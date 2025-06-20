using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Include için gerekli
using System.Threading.Tasks;
using CaseStudy_Netlog.Core.Interfaces;
using CaseStudy_Netlog.Data.Entities;
using CaseStudy_Netlog.Data.Context;
using System;

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
