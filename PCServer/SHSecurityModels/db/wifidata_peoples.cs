using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SHSecurityModels
{
    /// <summary>
    /// 根据ftp中的resultAll.json文件
    /// 每分钟获取一次此文件，读取内容
    /// 更新数据到此模型表中。 时间以获取时间记录。
    /// </summary>
   public class wifidata_peoples_history
    {
        public int Id { get; set; }

        public string Year { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
        public string HH { get; set; }
        public string MM { get; set; }
        public string SS { get; set; }

        public int Timestamp { get; set; }
        public string WifiID { get; set; }
        public int Count { get; set; }
    }


    public class wifidata_peoples
    {
        [Key]
        public string WifiID { get; set; }
        public int Count { get; set; }
        public int Timestamp { get; set; }

        public int AreaId { get; set; }
        
    }


    //ftp中的resultAll.json的类格式-不是新建的表
    public class JsonWifiDataStruct
    {
        public List<JsonWifiDataItemStruct> peopleList = new List<JsonWifiDataItemStruct>();
    }
    public class JsonWifiDataItemStruct
    {
        public string wifiID { get; set; }
        public int count { get; set; }
    }

}
