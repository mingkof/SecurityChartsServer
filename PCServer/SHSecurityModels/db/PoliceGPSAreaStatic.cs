using System;
using System.Collections.Generic;
using System.Text;

namespace SHSecurityModels
{
    //统计某区域，在小时内，出现警员的历史记录

   public class PoliceGPSAreaStatic
    {
        public int Id { get; set; }

        //不同
        public string AreaName { get; set; }
        //不同
        public string PoliceId { get; set; }


        public string Year { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
        public string HH { get; set; }
    }
}
