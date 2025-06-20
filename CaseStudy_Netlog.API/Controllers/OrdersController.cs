using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CaseStudy_Netlog.Core.Interfaces;

namespace CaseStudy_Netlog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPut("{id}/mark-delivered")]
        public async Task<IActionResult> MarkAsDelivered(int id)
        {
            var success = await _orderService.UpdateOrderStatusAsync(id, 1);
            if (!success)
                return BadRequest("Sipariş teslim edilemedi.Zaten teslim edilmiştir.");

            return Ok("Sipariş teslim edildi olarak güncellendi.");
        }
    }

}
