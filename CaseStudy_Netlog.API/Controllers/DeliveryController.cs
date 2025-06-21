using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CaseStudy_Netlog.Data.Context;
using CaseStudy_Netlog.Data.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using CaseStudy_Netlog.Core.Interfaces;

namespace CaseStudy_Netlog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveryController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpPut("update-deliveries")]
        //public async Task<IActionResult> UpdateDeliveries()
        public  IActionResult UpdateDeliveries()
        {
            //await _deliveryService.ProcessDeliveredOrdersAsync();deneme olduğu için kapattım.
            //sadece örnek Ok gönderdim normalde burda B firmasının modeline yönelik işlemler var.
            return Ok("Teslimat durumu güncellendi ve REST API’ye gönderildi.");
        }
    }

}
