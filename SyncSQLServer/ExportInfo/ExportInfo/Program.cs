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
namespace ExportInfo
{
    class Program
    {
       static List<string> li = new List<string>();
        public static string TxtPath = @"D:\MyText.txt";
        static DataTable  ConnectSQL(string DBip,string DBName,string UserID,string Password)
        {
            string strConn = "Data Source=" + DBip + ";Initial Catalog=" + DBName + ";User ID=" + UserID + ";Password =" + Password+";";
            SqlConnection conn = new SqlConnection(strConn);
           
            string sql = "select * from ut_datalist_2018";
            conn.Open();           
            SqlCommand cmd = new SqlCommand(sql, conn);         
            SqlDataAdapter myda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            myda.Fill(dt);
            return dt;
        }

        public static void DataTableToJson(DataTable table)
        {
            var JsonString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                int nowtimeStamp = TimeUtils.ConvertToTimeStamps(DateTime.Now.ToString());
                for (int i= 0;i <table.Rows.Count;i++)
                {
                    
                    if (TimeUtils.ConvertToTimeStamps(table.Rows[i]["dt_data"].ToString()) < nowtimeStamp)
                    {
                        var s = new JosnStruct()
                        {
                            sn = table.Rows[i]["sn"].ToString(),
                            dt_data = table.Rows[i]["dt_data"].ToString(),
                            dt_upload = table.Rows[i]["dt_upload"].ToString(),
                            up = Convert.ToInt32(table.Rows[i]["up"]),
                            down = Convert.ToInt32(table.Rows[i]["down"]),
                            timeStamp = TimeUtils.ConvertToTimeStampNow()
                        };
                        string str = Newtonsoft.Json.JsonConvert.SerializeObject(s);                     
                        li.Add(str);
                    }
                }
                FileUtils.WriteFile(TxtPath, li, true, Encoding.UTF8);                         
            }
            Console.ReadLine();
        }
        static void Main(string[] args)
        {
            thread();
        }


       static void thread()
        {
            ThreadPool.QueueUserWorkItem((a) =>
            {
                while (true)
                {

                    DataTableToJson(ConnectSQL(@"10.1.30.246\SQLEXPRESS", "iDTKdata", "sa", "P@ssw0rd"));


                    Thread.Sleep(1000 * 60);
                }
            });
        }
    }
}
public class JosnStruct
{
    public string sn;
    public string dt_data;
    public string dt_upload;
    public int up;
    public int down;
    public int timeStamp;


}
//DataSet ds = new DataSet();
// SqlDataReader reader = cmd.ExecuteReader();
// StreamWriter sr;
// string report;
// if (File.Exists(@"D:\MyText.txt"))
// {
//     sr = File.AppendText(@"D:\MyText.txt");
//     report = "appended";

// }
// else   //如果文件不存在,则创建File.CreateText对象   
// {
//     sr = File.CreateText(@"D:\MyText.txt");
//     report = "created";
// }
// StringBuilder sb = new StringBuilder();
//// sr.WriteLine("sn"+"\t\t" +"dt_data"+"\t\t"+" dt_upload"+"\r\n");
// while (reader.Read())
// {

//     string column1 = reader.GetString(reader.GetOrdinal("sn"));
//     string column2 = reader.GetString(reader.GetOrdinal("dt_data"));
//     string column3 = reader.GetString(reader.GetOrdinal("dt_upload"));
//     //sr.WriteLine(column1 + "\t" + column2 + "\t" + column3 + "\r\n");
//     string sts = "column1\tcolumn2\tcolumn3\n";
//     FileUtils.WriteFile(@"D:\MyText.txt", sts);
//    // Console.Write("{0}\t{1}\t{2}\n", column1, column2, column3);
// }
// sr.Close();
// //DataTable dt = new DataTable();
// //myda.Fill(dt);
// //myda.Fill(ds);
