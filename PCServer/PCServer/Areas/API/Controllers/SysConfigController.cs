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
    [Route("api/sysconfig")]
    public class SysConfigController : Controller
    {
        private readonly ILogger _logger;
        private readonly ISysConfigRepository _sysConfig;
        public SysConfigController(ISysConfigRepository sysConfig, ILogger<Sys110WarnController> logger)
        {
            _logger = logger;
            _sysConfig = sysConfig;
        }
        /// <summary>
        /// 修改SiP
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet("ChangeSipStatus/{value}")]
        public IActionResult ChangeSipStatus(int value)
        {
            int key = (int)EConfigKey.kSipSCStatus;
            var query = _sysConfig.Find(p => p.key == key);
            if (query == null)
            {
                _sysConfig.Add(new sys_config() {
                    key = key,
                    value = "",
                    valueInt = value
                });
            }
            else
            {
                query.valueInt = value;
                _sysConfig.Update(query);
            }
            return Ok("ok");
        }
        /// <summary>
        /// 获取sip
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSipStatus/")]
        public IActionResult GetSipStatus()
        {
            int key = (int)EConfigKey.kSipSCStatus;
            var query = _sysConfig.Find(p => p.key == key);
            if (query == null)
            {
                _sysConfig.Add(new sys_config() {
                    key = key,
                    value = "",
                    valueInt = 0
                });
                return Ok(new {
                    res = 0
                });
            }
            else
            {
                return Ok(new {
                    res = query.valueInt
                });
            }
        }
        /// <summary>
        /// 设置图标镜头的 camearId
        /// </summary>
        /// <param name="tableIndex"></param>
        /// <param name="camIndex"></param>
        /// <param name="camId"></param>
        /// <returns></returns>
        [HttpPost("SetTableCamId/{tableIndex}/{camIndex}/{camId}")]
        public IActionResult SetTableCamId(string tableIndex, string camIndex, string camId)
        {
            string keystr = "tb" + tableIndex + "_" + "cam" + camIndex + "_id";
            int key;
            try
            {
                key = (int)Enum.Parse(typeof(EConfigKey), keystr);
            }
            catch
            {
                return BadRequest("无法找到该配置" + keystr);
            }
            var query = _sysConfig.Find(p => p.key == key);
            if (query != null)
            {
                query.value = camId;
                _sysConfig.Update(query);
                return Ok("修改" + keystr + "成功");
            }
            return BadRequest("无法找到该配置");
        }
        /// <summary>
        /// 设置图标镜头的 viddeoUrl
        /// </summary>
        /// <param name="tableIndex"></param>
        /// <param name="camIndex"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpPost("SetTableCamUrl/{tableIndex}/{camIndex}")]
        public IActionResult SetTableCamUrl(string tableIndex, string camIndex, [FromBody] string url)
        {
            string keystr = "tb" + tableIndex + "_" + "cam" + camIndex + "_url";
            int key;
            try
            {
                key = (int)Enum.Parse(typeof(EConfigKey), keystr);
            }
            catch
            {
                return BadRequest("无法找到该配置" + keystr);
            }
            var query = _sysConfig.Find(p => p.key == key);
            if (query != null)
            {
                query.value = url;
                _sysConfig.Update(query);
                return Ok("修改" + keystr + url + "成功");
            }
            return BadRequest("无法找到该配置");
        }
        /// <summary>
        /// 添加配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueInt"></param>
        /// <param name="valueStr"></param>
        /// <returns></returns>
        [HttpPost("SetConfig/{key}/{valueInt}/{valueStr}")]
        public IActionResult SetConfig(int key, int valueInt=0, string valueStr="")
        {
            if (key<1000)
            {
                return BadRequest("key值无效" + key);
            }
            var query = _sysConfig.Find(p => p.key == key);
            if (query==null)
            {
                _sysConfig.Add(new sys_config() {
                    key = key,
                    value = valueStr,
                    valueInt = valueInt
                });
                return Ok("配置已添加");
            }
            else
            {
                query.value = valueStr;
                query.valueInt = valueInt;
                _sysConfig.Update(query);
                return BadRequest("配置已更新" + key);
            }
        }

        /// <summary>
        /// 根据key值获取配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("GetConfig/{key}")]
        public IActionResult GetConfig(int key)
        {
            var query = _sysConfig.Find(p => p.key == key);
            if (query!=null)
            {
                return Ok(new {
                    res= new {
                        key=key,
                        valueInt=query.valueInt,
                        valueStr=query.value
                    }
                });
            }
            return BadRequest("无配置");
        }


    }
 }