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
    public class DeliveriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DeliveriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/deliveries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Delivery>>> GetDeliveries()
        {
            var deliveries = await _context.Deliveries
                .Include(d => d.Order)
                .ToListAsync();

            return Ok(deliveries);
        }

        // GET: api/deliveries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Delivery>> GetDelivery(int id)
        {
            var delivery = await _context.Deliveries
                .Include(d => d.Order)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (delivery == null)
                return NotFound();

            return Ok(delivery);
        }

        // POST: api/deliveries
        [HttpPost]
        public async Task<ActionResult<Delivery>> PostDelivery([FromBody] Delivery delivery)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Deliveries.Add(delivery);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDelivery), new { id = delivery.Id }, delivery);
        }

        // PUT: api/deliveries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDelivery(int id, [FromBody] Delivery updated)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var delivery = await _context.Deliveries.FindAsync(id);
            if (delivery == null)
                return NotFound();

            delivery.DeliveryDate = updated.DeliveryDate;
            delivery.PlateNumber = updated.PlateNumber;
            delivery.DeliveredBy = updated.DeliveredBy;
            delivery.OrderId = updated.OrderId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/deliveries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDelivery(int id)
        {
            var delivery = await _context.Deliveries.FindAsync(id);
            if (delivery == null)
                return NotFound();

            _context.Deliveries.Remove(delivery);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
