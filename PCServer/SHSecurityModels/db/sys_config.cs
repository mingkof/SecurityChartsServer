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
        kPoliceTotalCountTaday = 104,

        //SIP用于客户端和sip处理服务器的状态更改
        kSipSCStatus = 110,


        //用于视频监控图表的配置
        tb1_cam1_id = 120,
        tb1_cam2_id = 121,
        tb1_cam3_id = 122,
        tb1_cam4_id = 123,
        tb2_cam1_id = 124,
        tb2_cam2_id = 125,
        tb2_cam3_id = 126,
        tb2_cam4_id = 127,
        tb1_cam1_url = 130,
        tb1_cam2_url = 131,
        tb1_cam3_url = 132,
        tb1_cam4_url = 133,
        tb2_cam1_url = 134,
        tb2_cam2_url = 135,
        tb2_cam3_url = 136,
        tb2_cam4_url = 137,
    }


    public class sys_config
    {
        public int id { get; set; }
        public int key { get; set; }
        public string value { get; set; }
        public int valueInt { get; set; }
    }
}
