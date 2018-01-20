 using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;

namespace MKServerWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult Index()
        {
            return Redirect("/swagger/");
        }

        public IActionResult Client()
        {
            return View();
        }
        public IActionResult Client2()
        {
            return View();
        }

        public IActionResult Error()
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var error = feature?.Error;
            _logger.LogError("Oops!", error);
            return View("~/Views/Shared/Error.cshtml", error);

            //ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            //return View();
        }
    }
}