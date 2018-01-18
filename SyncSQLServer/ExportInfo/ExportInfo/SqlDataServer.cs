using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using KVDDDCore.Utils;
using System.Threading;
using ServerDBExt.Database;


namespace ExportInfo
{
    public class SqlDataServer
    {

        static List<string> li = new List<string>();
        static string Path = @"E:\MKProjects\MKSecurityCharts\SecurityChartsServer\SyncSQLServer\Data\";
        static List<string> snType = new List<string>() { "4B41B36B" };

        static List<string> dataList = new List<string>();
        public SqlDataServer()
        {
            StartServer();
            Console.ReadLine();
        }

        static void QuerySQL(string DBip, string DBName, string UserID, string Password)
        {

            string strConn = "Data Source=" + DBip + ";Initial Catalog=" + DBName + ";User ID=" + UserID + ";Password =" + Password + ";";
            DatabaseSql sql = new DatabaseSql(strConn);
            string today = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
            string dateNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string filePath = Path + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

            Console.WriteLine(dateNow);
            Console.WriteLine("update data");

            for (int i = 0; i < snType.Count; i++)
            {
                var upCount = sql.QueryValue("SELECT COUNT(*) FROM ut_datalist_2018 WHERE sn=" + "'" + snType[i] + "'" + " AND " + "up=1" + " AND " + "dt_data>=" + "'" + today + "'" + " And " + "dt_data<=" + "'" + dateNow + "'", null);
                ProcessData(snType[i], "1", upCount.ToString());

            }
            sql.DoEnsureClose();
            sql = new DatabaseSql(strConn);
            for (int i = 0; i < snType.Count; i++)
            {
                var downCount = sql.QueryValue("SELECT COUNT(*) FROM ut_datalist_2018 WHERE sn=" + "'" + snType[i] + "'" + " AND " + "down=1" + " AND " + "dt_data>=" + "'" + today + "'" + " And " + "dt_data<=" + "'" + dateNow + "'", null);
                ProcessData(snType[i], "0", downCount.ToString());
            }
            sql.DoEnsureClose();

            FileUtils.WriteFile(filePath, dataList, true, Encoding.UTF8);

        }

        public static void ProcessData(string sn,string type,string count)
        {
            int timeStampNow = TimeUtils.ConvertToTimeStampNow();
            var jsonStruct = new JosnStruct() { sn = sn, type = type, count = count, timeStamp = timeStampNow };
            var dataStr = Newtonsoft.Json.JsonConvert.SerializeObject(jsonStruct);
            dataList.Add(dataStr);
        }


        //public static void DataTableToJson(DataTable table)
        //{
        //    var JsonString = new StringBuilder();
        //    if (table.Rows.Count > 0)
        //    {
        //        int nowtimeStamp = TimeUtils.ConvertToTimeStamps(DateTime.Now.ToString());
        //        for (int i = 0; i < table.Rows.Count; i++)
        //        {
        //            if (TimeUtils.ConvertToTimeStamps(table.Rows[i]["dt_data"].ToString()) < nowtimeStamp)
        //            {
        //                var s = new JosnStruct()
        //                {
        //                    sn = table.Rows[i]["sn"].ToString(),
        //                    dt_data = table.Rows[i]["dt_data"].ToString(),
        //                    dt_upload = table.Rows[i]["dt_upload"].ToString(),
        //                    up = Convert.ToInt32(table.Rows[i]["up"]),
        //                    down = Convert.ToInt32(table.Rows[i]["down"]),
        //                    timeStamp = TimeUtils.ConvertToTimeStampNow()
        //                };
        //                string str = Newtonsoft.Json.JsonConvert.SerializeObject(s);
        //                li.Add(str);
        //            }
        //        }
        //        FileUtils.WriteFile(TxtPath, li, true, Encoding.UTF8);
        //    }
        //    Console.WriteLine("read SqlServerData");
        //}

        static void StartServer()
        {
            ThreadPool.QueueUserWorkItem((a) =>
            {
                while (true)
                {
                    QuerySQL(@"10.1.30.246\SQLEXPRESS", "iDTKdata", "sa", "P@ssw0rd");
                    Thread.Sleep(1000 * 60);
                }
            });
        }
    }
    public class JosnStruct
    {
        public string sn;
        public string type;
        public string count;
        public int timeStamp;
    }
}
