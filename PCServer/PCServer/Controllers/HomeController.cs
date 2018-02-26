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
            //if (_logger == null)
            //    _logger = factory.CreateLogger("mylogger");

            _logger = logger;
        }

        public IActionResult Index()
        {
            return Redirect("/swagger/");
            //return View();
        }

        public IActionResult swagger()
        {
            return Redirect("/swagger/");
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