using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CaseStudy_Netlog.Data.Context;
using CaseStudy_Netlog.Data.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace CaseStudy_Netlog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderItemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/orderitems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItemDto>>> GetOrderItems()
        {
            var items = await _context.OrderItems.ToListAsync();

            var dtos = items.Select(i => new OrderItemDto
            {
                ProductName = i.ProductName,
                Quantity = i.Quantity
            }).ToList();

            return Ok(dtos);
        }


        // GET: api/orderitems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItem>> GetOrderItem(int id)
        {
            var item = await _context.OrderItems
                .FirstOrDefaultAsync(oi => oi.Id == id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }
    }
}
