using System;
using System.Collections.Generic;
using System.Text;

namespace SHSecurityModels
{
    public enum EConfigKey
    {
        kNone = 0,
        kLastResultUpdateTime = 100,
        kTimer1 = 101,
        kTimer2 = 102,

        kGpsGridServerLast110Timestamp = 103,

        //静安今日警力总数
        kPoliceTotalCountTaday = 104
    }


    public class sys_config
    {
        public int id { get; set; }
        public int key { get; set; }
        public string value { get; set; }
        public int valueInt { get; set; }
    }
}
