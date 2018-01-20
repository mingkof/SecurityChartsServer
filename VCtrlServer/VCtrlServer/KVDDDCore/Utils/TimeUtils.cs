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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeStamp">时间戳单位秒</param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(long timeStamp)
        {
            var start = Get1970UtcTime();
            return start.AddMilliseconds(timeStamp).AddHours(8);
        }

    }
}
