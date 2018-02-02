using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PCServer;
using SHSecurityContext.IRepositorys;
using SHSecurityModels;
using SHSecurityServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SHSecurityServer.Controllers
{
    [Produces("application/json")]
    [Route("api/kakoudatajin")]
    public class KaKouDataJinController : Controller
    {
        private readonly ILogger _logger;
        // private readonly IKaKouDataHistoryJinRepository _kakoudatajin_history;
        private readonly IKaKouDataJinRepository _kakoudatajin;
        private readonly IKaKouTopRepository _kakouTop;

        public KaKouDataJinController(IKaKouDataJinRepository kakoudata, IKaKouTopRepository kakouTop, ILogger<Sys110WarnController> logger)
        {
            _logger = logger;
            _kakoudatajin = kakoudata;
            _kakouTop = kakouTop;
        }
        /// <summary>
        /// 获取历史所有卡口记录
        /// </summary>
        /// <returns></returns>
        [HttpGet("getkakoudata/")]
        public IActionResult GetKaKouData()
        {
            var query=_kakoudatajin.FindList(p => true,"",false);
            if (query!=null)
            {
                return Ok(new {
                    res=query
                });
            }else
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// 获取卡口流量top5 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetKakouTop5Data/")]
        public IActionResult GetKakouTop5Data()
        {
            var query = _kakouTop.FindList(p => true, "", false);
            if (query!=null)
            {

                return Ok(new {
                    res = query
                });
            }
            return BadRequest();
        }
        /// <summary>
        /// 获取当前车流进出统计
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCarCount/{sn}")]
        public IActionResult GetCarCount(int sn)
        {
            var inQueryCount = 0;
            var outQueryCount = 0;
            var kakouConfig = PCServerMain.Instance.kakouConfigDic;
            if (sn==0)
            {
                var inQuery = _kakoudatajin.FindList(p => kakouConfig["南广场"].Contains(p.SBBHID) && p.pass_or_out == "0", "", false);

                var outQuery = _kakoudatajin.FindList(p => kakouConfig["北广场"].Contains(p.SBBHID) && p.pass_or_out == "1", "", false);

                try
                {
                    if (inQuery != null)
                    {
                        inQueryCount = inQuery.Sum(p => int.Parse(p.Count));
                    }
                    if (outQuery != null)
                    {
                        outQueryCount = outQuery.Sum(p => int.Parse(p.Count));
                    }
                }
                catch
                {

                    throw;
                }
            }
            else if(sn==1)
            {
                var inQuery = _kakoudatajin.FindList(p => kakouConfig["北广场"].Contains(p.SBBHID) && p.pass_or_out == "0", "", false);

                var outQuery = _kakoudatajin.FindList(p => kakouConfig["北广场"].Contains(p.SBBHID) && p.pass_or_out == "1", "", false);

                try
                {
                    if (inQuery != null)
                    {
                        inQueryCount = inQuery.Sum(p => int.Parse(p.Count));
                    }
                    if (outQuery != null)
                    {
                        outQueryCount = outQuery.Sum(p => int.Parse(p.Count));
                    }
                }
                catch
                {

                    throw;
                }
            }
          
           
            
            return Ok(new {
                res=new {
                    inCount=inQueryCount,
                    outCount=outQueryCount
                }
            });
        }
    }
 }