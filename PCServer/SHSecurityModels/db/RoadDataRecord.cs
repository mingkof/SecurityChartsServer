//mysql模型
//卡口排名top5
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SHSecurityModels
{
    public class RoadDataRecord
    {
        public int Id { get; set; }
        public int Timestamp { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
        public string HH { get; set; }
        public string MM { get; set; }
        public string Roadname { get; set; }
        public string TrafficAvgSpeed { get; set; }
        public string TrafficData { get; set; }
    }

}
