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
        //static string Path = @"E:\MKProjects\MKSecurityCharts\SecurityChartsServer\SyncSQLServer\Data\";
        static string Path = Environment.CurrentDirectory + "/Data/";
        static List<string> snType = new List<string>() { "3016CF5D","4B41B36B","4633AABC","811C2CF0","FCC07153","2D89EA67","373FA7D5","356CE9A0","538DC2FB","C48FCCEE","889E179E","0C986F57","B5B2FE89",
        "9701EB1C","09D8F256","74119134","04E1DE42","33FEBA62","9E83D072","20D90515","0D0CD120","1B5D043C","31974FEB","FB3B3246","2DB9EA67"};


        static List<string> dataList = new List<string>();
        public SqlDataServer()
        {
            if (!System.IO.Directory.Exists(Path))
                System.IO.Directory.CreateDirectory(Path);

 
                StartServer();
   
            Console.WriteLine("已启动服务-定时读取人数计数SqlServer");
            Console.ReadLine();
        }

        static DatabaseSql MainDBSql = null;

        static void ConnectSql(string DBip, string DBName, string UserID, string Password)
        {
            string strConn = "Data Source=" + DBip + ";Initial Catalog=" + DBName + ";User ID=" + UserID + ";Password =" + Password + ";";
            MainDBSql  = new DatabaseSql(strConn);
            MainDBSql.DoEnsureOpen();
        }



        static void QuerySQL()
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
            string dateNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string fileName = "hongwai_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            string filePath = Path +  fileName;
            dataList = new List<string>(); 

            Console.WriteLine(dateNow);
            Console.WriteLine("正在读取人数计数-SqlServer");

            for (int i = 0; i < snType.Count; i++)
            {
                var upCount = MainDBSql.QueryValue(
                    "SELECT SUM(up) FROM ut_datalist_2018 WHERE sn=" + "'" + snType[i] + "'" + " AND " + "dt_data>=" + "'" + today + "'" + " And " + "dt_data<=" + "'" + dateNow + "'",
                    null,
                    false
                    );

                if (upCount != null)
                    ProcessData(snType[i], "1", upCount.ToString());


                var downCount = MainDBSql.QueryValue(
                    "SELECT SUM(down) FROM ut_datalist_2018 WHERE sn=" + "'" + snType[i] + "'" + " AND " + "dt_data>=" + "'" + today + "'" + " And " + "dt_data<=" + "'" + dateNow + "'",
                    null,
                    false
                    );
                if(downCount != null)
                    ProcessData(snType[i], "0", downCount.ToString());
            }


            FileUtils.WriteFile(filePath, dataList, true, Encoding.UTF8);
            UpLoadToFtp(filePath, fileName);
        }

        static void UpLoadToFtp(string localPath,string filename)
        {
            FtpClient ftpClient = new FtpClient("ftp://180.168.211.5:32121/", "zbfjrcwj", "zbfjrcwj");
            ftpClient.Upload(localPath, "send /rcwj/"+filename);
        }

        public static void ProcessData(string sn,string type,string count)
        {
            int timeStampNow = TimeUtils.ConvertToTimeStampNow();
            var jsonStruct = new JosnStruct() { sn = sn, type = type, count = count, timeStamp = timeStampNow };
            var dataStr = Newtonsoft.Json.JsonConvert.SerializeObject(jsonStruct);
            dataList.Add(dataStr);
        }

        static void StartServer()
        {
            ThreadPool.QueueUserWorkItem((a) =>
            {
                ConnectSql(@"localhost", "iDTKdata", "sa", "JingAn110");

                while (true)
                {
                    //UpLoadToFtp(@"D:\2018-security\SecurityCharts\SecurityChartsServer\SyncSQLServer\ExportInfo\ExportInfo\bin\Debug\Data\2018-01-25.txt", "2018-01-25.txt");
                    QuerySQL();
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
