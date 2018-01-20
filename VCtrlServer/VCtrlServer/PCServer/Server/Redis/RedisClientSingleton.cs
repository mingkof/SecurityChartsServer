using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCServer.Redis
{

    public class RedisClientSingleton
    {
        private static RedisClient _redisClinet;
        private RedisClientSingleton() { }

        private static object _lockObj = new object();
        public static RedisClient Inst
        {
            get {
                if (_redisClinet == null)
                {
                    lock (_lockObj)
                    {
                        if (_redisClinet == null)
                        {
                            using (var serviceScope = ServiceLocator.Instance.CreateScope())
                            {
                                #region
                                //var _config = serviceScope.ServiceProvider.GetService<IConfiguration>();
                                //IConfigurationSection redisConfig = _config.GetSection("RedisConfig").GetSection("Redis_Default");
                                //var ip = redisConfig["ConnIP"];
                                //var port = redisConfig["ConnPort"];
                                //var db = redisConfig["DB"];
                                //var name = redisConfig["Redis01"];
                                #endregion

                                //读取Redisg配置
                                _redisClinet = new RedisClient(PCServerMain.Instance.RedisConfig);
                            }
                        }
                    }
                }
                return _redisClinet;
            }
        }
    }
}
