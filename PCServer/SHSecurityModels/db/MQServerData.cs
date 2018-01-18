using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SHSecurityModels
{
    public class MQServerData
    {
        [Key]
        public int key { get; set; }
        public string commRmk { get; set; } //时间描述
        public string dsnum { get; set; }//事件相关设备编码
        public string foreignld { get; set; }//
        public string mtype { get; set; }//报警主机类型:8:电气漏电报警系统； 9： 可燃气体探测报警系统；否则为火警（可能为空）
        public string projectId { get; set; }//项目id
        public string time { get; set; }//事件发生的时间
        public string userId { get; set; }//
        public string topicType { get; set; }//时间类型  1：报警通知  2：报警消除通知  3：故障通知  4：故障消除通知  5：异常通知
        public int timeStamp { get; set; }//通知的时间
    }

    public class JsonMQServerDataContruct
    {
        public string commRmk;
        public string dsnum;
        public string foreignld;
        public string mtype;
        public string projectId;
        public string time;
        public string userId;
        public string topicType;
        public int timeStamp;
    }

}
