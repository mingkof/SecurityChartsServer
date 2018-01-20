using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCServer.Server.Config
{
    public class PCServerConfig
    {
        public string ServerName { get; set; }
        public string ServerDesc { get; set; }
        public string Version { get; set; }
        public string ReadmeUrl { get; set; }
        public bool UseProtobuf { get; set; }

        public Dictionary<string, string> ext { get; set; }
    }

    public class RedisConfig
    {
        public Dictionary<string, string> Redis_Default { get; set; }
    }



}
