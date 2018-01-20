using System;
using System.Collections.Generic;
using System.Text;

namespace SHSecurityModels
{
    public enum EConfigKey
    {
        kNone = 0,
        kSdpBaseUrl = 1
    }


    public class sys_config
    {
        public int id { get; set; }
        public int key { get; set; }
        public string value { get; set; }
        public int valueInt { get; set; }
    }
}
