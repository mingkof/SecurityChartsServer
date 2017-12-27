using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SHSecurityContext.IRepositorys;
using SHSecurityModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.NodeServices;
namespace SHSecurityServer.Controllers
{
    [Produces("application/json")]
    [Route("api/cameras")]
    public class CameraController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICamerasRepository _cameraRepo;
        public CameraController(ICamerasRepository cameraRepo, ILogger<PoliceGpsController> logger)
        {
            _logger = logger;
            _cameraRepo = cameraRepo;
        }

        [HttpGet("list")]
        public IActionResult GetList()
        {
            var list = _cameraRepo.FindList(p => true,"",false);

            return Ok(new
            {
                res = list
            });
        }

        [HttpGet("getcamera/{id}")]
        public IActionResult GetCamera(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Ok(new
                {
                    res = "{}"
                });
            }
            
            var query = _cameraRepo.Find(p => p.id == id);

            return Ok(new
            {
                res = query
            });
        }
    }
}
