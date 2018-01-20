using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PCServer.TaskNodeServer;
using PCServer.TaskNodeServer.Task;

namespace PCServer.Areas.NodeServer
{
    [Produces("application/json")]
    [Route("api/mknode")]
    public class NodeServerController : Controller
    {
        //[HttpPost("AddTask")]
        //public IActionResult AddTask([FromBody]NodeTask task) {
        //    try
        //    {
        //        TaskManager.inst.AddTask(task);
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest(new { message = "出错" });
        //    }

        //    return Ok();
        //}

    }
}