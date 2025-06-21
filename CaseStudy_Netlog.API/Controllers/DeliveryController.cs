using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CaseStudy_Netlog.Data.Context;
using CaseStudy_Netlog.Data.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using CaseStudy_Netlog.Core.Interfaces;
using System;
using Microsoft.Extensions.Logging;

namespace CaseStudy_Netlog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;
        private readonly ILogger<DeliveryController> _logger;
        public DeliveryController(IDeliveryService deliveryService, ILogger<DeliveryController> logger)
        {
            _deliveryService = deliveryService;
            _logger = logger;
        }

        [HttpPut("update-deliveries")]
        public IActionResult UpdateDeliveries([FromBody] DeliveryDto delivery)
        {
            _logger.LogInformation($"Teslimat Alındı - OrderId: {delivery.OrderId}, Plaka: {delivery.PlateNumber}");
            return Ok("Teslimat alındı");
        }

    }

}
