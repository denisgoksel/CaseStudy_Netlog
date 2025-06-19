using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudy_Netlog.API.Controllers
{
    public class OrderItemsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
