using NLog;
using NLog.Config;
using NLog.Targets;
using SHSecurityContext.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHSecurityServer.Models
{
    public class SampleData
    {
        public static readonly List<string> WifiIdList = new List<string>()
        {
            "3101063D000754",
            "3101083D000311",
            "3101083D000389",
            "3101083D000542",
            "3101083D000605",
            "3101083D000635",
            "3101083D000674",
            "3101083D000701",
            "3101083D000710",
            "3101083D000755",
            "3101083D000764",
            "3101083D000788",
            "3101083D001367",
            "3101083D001370",
            "3101083D001379",
            "3101083D001385",
            "3101083D001388",
            "3101083D001391",
            "3101083D001397",
            "3101083D001400",
            "3101083D001403",
            "3101083D001409",
            "3101083D001412",
            "3101083D001436",
            "3101083D001442",
            "3101083D001445",
            "3101083D001448",
            "3101083D001460",
            "3101083D001469",
            "3101083D001484",
            "3101083D001550",
            "3101083D001787",
            "3101083D004064",
            "3101083D004403",
            "3101083D004631",
            "3101083D004634",
            "3101083D005741",
            "3101083D005924"
        };


        public static void InitDB(ref SHSecuritySysContext db)
        {

            //// Step 1. Create configuration object 
            //var config = new LoggingConfiguration();

            //// Step 2. Create targets and add them to the configuration 
            //var consoleTarget = new ColoredConsoleTarget();
            //config.AddTarget("console", consoleTarget);

            //var fileTarget = new FileTarget();
            //config.AddTarget("file", fileTarget);

            //// Step 3. Set target properties 
            //consoleTarget.Layout = @"${date:format=HH\:mm\:ss} ${logger} ${message}";
            //fileTarget.FileName = "${basedir}/file.txt";
            //fileTarget.Layout = "${message}";

            //// Step 4. Define rules
            //var rule1 = new LoggingRule("*", LogLevel.Debug, consoleTarget);
            //config.LoggingRules.Add(rule1);

            //var rule2 = new LoggingRule("*", LogLevel.Debug, fileTarget);
            //config.LoggingRules.Add(rule2);

            //// Step 5. Activate the configuration
            //LogManager.Configuration = config;

            //// Example usage
            //Logger logger = LogManager.GetLogger("Example");
            //logger.Trace("trace log message");
            //logger.Debug("debug log message");
            //logger.Info("info log message");
            //logger.Warn("warn log message");
            //logger.Error("error log message");
            //logger.Fatal("fatal log message");



            //db.db_jjds.Add(new SHSecurityModels.db_jjd()
            //{
            //    jjdid = "444",
            //    af_addr = "123",
            //    datetime = "12312312"
            //});

            //db.SaveChanges();

            //db.sys_110warningdb.Add(new SHSecurityModels.sys_110warningdb()
            //{
            //    JJD_ID = (db.sys_110warningdb.Count() + 1).ToString(),
            //    AF_ADDR = "1",
            //    AMAP_GPS_X = "1",
            //    AMAP_GPS_Y = "1",
            //    BJAY1 = "1",
            //    BJAY2 = "1",
            //    YEAR = System.DateTime.Now.Year.ToString(),
            //    MONTH = System.DateTime.Now.Month.ToString("00"),
            //    DAY = System.DateTime.Now.Day.ToString("00"),
            //    HH = System.DateTime.Now.Hour.ToString("00"),
            //    MM = System.DateTime.Now.Minute.ToString("00"),
            //    SS = System.DateTime.Now.Second.ToString("00"),
            //    QY = "22",
            //     COMMET = System.DateTime.Now.ToLongTimeString()
            //});

            //db.sys_wifitable.Add(new SHSecurityModels.sys_wifitable()
            //{
            //    ACCESS_AP_CHANNEL = 1,
            //    ACCESS_AP_ENCRYPTION_TYPE = 2,
            //    MAC = "mac"
            //});

            //db.SaveChanges();
        }

    }
}
