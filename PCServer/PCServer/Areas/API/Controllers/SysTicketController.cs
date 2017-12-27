using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SHSecurityContext.IRepositorys;
using SHSecurityModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MKServerWeb.Controllers
{
    [Produces("application/json")]
    [Route("api/ticket")]
    public class SysTicketController : Controller
    {
        private readonly ILogger _logger;
        private readonly ISysTicketresRepository _sysTicketresRepository;
        public SysTicketController(ISysTicketresRepository sysTicketresRepository, ILogger<SysTicketController> logger)
        {
            _logger = logger;

            _sysTicketresRepository = sysTicketresRepository;
        }



        [HttpGet("list", Name = "TicketList")]
        public IActionResult Get()
        {
            var list = _sysTicketresRepository.FindList(p => true, "", false);

            if (list == null)
                return BadRequest("无数据");

            return Ok(new
            {
                array = list
            });
        }

    }
}