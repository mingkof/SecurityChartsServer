//mysql模型
//卡口排名top5
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SHSecurityModels
{
    public class KakouTop
    {
        public int Id { get; set; }
        public string SBBHID { get; set; }
        public int Value { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
        public int Timestamp { get; set; }

    }

}
