using System;
using System.Collections.Generic;
using System.Text;

namespace SHSecurityModels
{
   public class sys_GpsGridWarn
    {
        public int Id { get; set; }

        public string GridName { get; set; }

        public int GridX { get; set; }
        public int GridY { get; set; }

        //110警情ID
        public string JJD_ID { get; set; }
        public int JJD_TIMESIGN { get; set; }
    }

    //public enum GpsGridStaticsType
    //{
    //    Hour = 0,
    //    Day = 1,
    //    Month = 2,
    //    Year = 3
    //}


    //public class sys_GpsGridStatics
    //{
    //    public int Id { get; set; }

    //    public string GridName { get; set; }

    //    public GpsGridStaticsType Type { get; set; }

    //    //统计数
    //    public int Count { get; set; }
    //}
}
