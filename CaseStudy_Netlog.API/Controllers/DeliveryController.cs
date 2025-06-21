using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CaseStudy_Netlog.Data.Context;
using CaseStudy_Netlog.Data.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using CaseStudy_Netlog.Core.Interfaces;
using System;

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
        public IActionResult UpdateDeliveries([FromBody] DeliveryDto delivery)
        {
            Console.WriteLine($"Teslimat Alındı - OrderId: {delivery.OrderId}, Plaka: {delivery.PlateNumber}");
            return Ok("Teslimat alındı");
        }

    }

}
