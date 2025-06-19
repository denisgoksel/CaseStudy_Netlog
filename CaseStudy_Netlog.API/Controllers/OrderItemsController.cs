using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CaseStudy_Netlog.Data.Context;
using CaseStudy_Netlog.Data.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

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
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems()
        {
            var items = await _context.OrderItems
                .Include(oi => oi.Order)
                .ToListAsync();

            return Ok(items);
        }

        // GET: api/orderitems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItem>> GetOrderItem(int id)
        {
            var item = await _context.OrderItems
                .Include(oi => oi.Order)
                .FirstOrDefaultAsync(oi => oi.Id == id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        // POST: api/orderitems
        [HttpPost]
        public async Task<ActionResult<OrderItem>> PostOrderItem([FromBody] OrderItem item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.OrderItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderItem), new { id = item.Id }, item);
        }

        // PUT: api/orderitems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderItem(int id, [FromBody] OrderItem updated)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var item = await _context.OrderItems.FindAsync(id);
            if (item == null)
                return NotFound();

            item.ProductName = updated.ProductName;
            item.Quantity = updated.Quantity;
            item.OrderId = updated.OrderId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/orderitems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var item = await _context.OrderItems.FindAsync(id);
            if (item == null)
                return NotFound();

            _context.OrderItems.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
