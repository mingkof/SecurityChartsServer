using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SHSecurityModels;
using System.Linq;
using PCServer.Server.GPS;
using System.Numerics;
using Microsoft.Extensions.Logging;
using SHSecurityContext.IRepositorys;
using SHSecurityServer.Controllers;

namespace SHSecurityServer.Controllers
{
    [Produces("application/json")]
    [Route("api/Test")]
    public class TestController : Controller
    {
        private readonly ILogger _logger;
        private readonly ITestReposity _testRepo;
        //private readonly ICamePeopleCountRepository _camPeopleCount;
        private readonly ISysConfigRepository _sysConfig;
        public TestController(ITestReposity testRepo,ISysConfigRepository sysConfig)
        {
            
            _testRepo = testRepo;
   
            _sysConfig = sysConfig;
        }




        [HttpGet("list")]
        public IActionResult GetList()
        {    //查询数据 返回的是对象
            var list = _testRepo.Find(p => p.id==1);
            //添加数据
            //_testRepo.Add(new db_test
            //{
            //    id=10,
            //    name="123"
            //});
            //查询出来修改更新数据
            //list.name = "adsad";
            //_testRepo.Update(list);


            //以json形式返回数据（对象类型）
            return Ok(new
            {
                res = list.name
            });
        }

    }
    
}