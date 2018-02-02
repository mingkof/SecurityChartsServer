//卡口模型数据以及ftp中的KaKouData.json的反解析结构体

//mysql模型
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SHSecurityModels
{
    public class CarAlarmData
    {
        public int Id { get; set; }
        public string Positon { get; set; }         //位置
        public string alarmTime { get; set; }       //报警时间
        public string plateId { get; set; }         //车牌号
        public string Year { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
        public int timeStamp { get; set; }
    }


}
