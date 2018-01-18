using System;
using System.Collections.Generic;
using System.Text;

namespace KVDDDCore.Utils
{
    public class TimeUtils
    {
        static DateTime Get1970UtcTime()
        {
            DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return time;
        }

        public static int ConvertToTimeStampNow()
        {
            DateTime time = DateTime.Now.AddHours(-8);
            return (int)(time - Get1970UtcTime()).TotalSeconds;
        }


        public static int ConvertToTimeStampNowByZero()
        {
            DateTime time = DateTime.Now.AddHours(-8);

            DateTime nt = new DateTime(time.Year, time.Month, time.Day);

            return (int)(nt - Get1970UtcTime()).TotalSeconds;
        }

        public static int ConvertToTimeStamps(string DateStr)
        {
            DateTime.TryParse(DateStr, out DateTime time);

            return (int)(time - Get1970UtcTime()).TotalSeconds;
        }


        public static int ConvertToTimeStampNowByZero(int delayDay)
        {
            DateTime time = DateTime.Now.AddHours(-8);
            DateTime nt = new DateTime(time.Year, time.Month, time.Day);
            nt = nt.AddDays(delayDay);

            return (int)(nt - Get1970UtcTime()).TotalSeconds;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeStamp">时间戳单位秒</param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(long timeStamp)
        {
            var start = Get1970UtcTime();
            return start.AddSeconds(timeStamp).AddHours(8);
        }


    }
}
