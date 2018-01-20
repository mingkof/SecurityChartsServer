using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PCServer.TaskNodeServer;
using PCServer.Server.Proto_Gongan;
using SHSecurityContext.IRepositorys;

namespace PCServer.Areas.CtrlServer
{
    [Produces("application/json")]
    [Route("api/vmain")]
    public class VMainController : Controller
    {
        ISipPortRepository _sipRepo;
        public VMainController(ISipPortRepository sipRepo)
        {
            _sipRepo = sipRepo;
        }


        [HttpGet("test")]
        public string test()
        {
            return "test hello";
        }


        [HttpGet("testsv")]
        public IActionResult testsv(string ip, string port, string sdp , string type, string value)
        {
            SipProtoSC data = new SipProtoSC()
            {
                ip = ip,
                port = port,
                sdp = sdp,
                type = type,
                value = value
            };
            AgentsManager.inst.SendMessageToSipAgent(Newtonsoft.Json.JsonConvert.SerializeObject(data));
            return Ok();
        }


        [HttpGet("UpdateConfigAndSipList")]
        public IActionResult updateSipRepoList()
        {
            AgentsManager.inst._SipManager.portsMngList = _sipRepo.FindList(p => true, "", false).ToList();
            AgentsManager.inst._SipManager.InitConfig();
            return Ok();
        }

        [HttpGet("getSipList")]
        public IActionResult getSipList()
        {
            return Ok(new {
                res = AgentsManager.inst._SipManager.portsMngList
            });
        }
    }


    
}