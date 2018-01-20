using Microsoft.Extensions.Configuration;
using PCServer.Server.Config;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCServer.Redis
{
    public class RedisClient : IDisposable
    {
        private RedisConfig _config;

        public string cf_ip { get; set; }
        public int cf_port { get; set; }
        public int cf_db { get; set; }
        public string cf_name { get; set; }
        public string cf_conn { get; set; }
        private ConcurrentDictionary<string, ConnectionMultiplexer> _connections;

        private ConnectionMultiplexer _connection;

        public RedisClient(RedisConfig config)
        {
            _config = config;

            cf_ip = _config.Redis_Default["ConnIP"];
            cf_port = int.Parse(_config.Redis_Default["ConnPort"]);
            cf_db = int.Parse(_config.Redis_Default["DB"]);
            cf_name = _config.Redis_Default["InstanceName"];
            cf_conn = cf_ip + ":" + cf_port;
            
            _connections = new ConcurrentDictionary<string, ConnectionMultiplexer>();
        }

        public IDatabase GetDatabase(int? db = null)
        {
            int defaultDB = cf_db;
            if (db.HasValue)
            {
                defaultDB = db.Value;
            }
            return GetConnect().GetDatabase(defaultDB);
        }

        public IServer GetServer(string configName = null, int endPointsIndex = 0)
        {
            var confOption = ConfigurationOptions.Parse((string)cf_conn);
            return GetConnect().GetServer(confOption.EndPoints[endPointsIndex]);
        }

        public ISubscriber GetSubscriber(string configName = null)
        {
            return GetConnect().GetSubscriber();
        }



        /// <summary>
        /// 获取ConnectionMultiplexer
        /// </summary>
        /// <param name="redisConfig">RedisConfig配置文件</param>
        /// <returns></returns>
        private ConnectionMultiplexer GetConnect()
        {
            if (string.IsNullOrEmpty(cf_conn))
                return null;

            return _connections.GetOrAdd(cf_name, p => ConnectionMultiplexer.Connect(cf_conn));
        }

     
        public void Dispose()
        {
            if (_connections != null && _connections.Count > 0)
            {
                foreach (var item in _connections.Values)
                {
                    item.Close();
                }
            }
        }
    }
}


/*
 *         private readonly IDatabase RedisDb;
 *         
 *   public RedisController(IConfiguration config, ILogger<RedisController> logger)
 *   
 *        RedisDb = RedisClientSingleton.GetInstance(_config).GetDatabase("Redis_Default");
 * */
