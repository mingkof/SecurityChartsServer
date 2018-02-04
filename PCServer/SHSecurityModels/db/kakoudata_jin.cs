//卡口模型数据以及ftp中的KaKouData.json的反解析结构体

//mysql模型
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SHSecurityModels
{
    /// <summary>
    /// 根据ftp中的kakoudata.json文件
    /// 每分钟获取一次此文件，读取内容
    /// 更新数据到此模型表中。 时间以获取时间记录。
    /// </summary>
   public class kakoudata_jin_history
    {
        [Key]
        public int Id { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
        public string HH { get; set; }
        public string SBBHID { get; set; }
        public string SBMC { get; set; }
        public string XSFX { get; set; }
        public int Count { get; set; }
        public int pass_or_out { get; set; }
    }

    public class kakoudata_jin
    {
        [Key]
        public int Id { get; set; }
        public string SBBHID { get; set; }
        public string SBMC { get; set; }
        public string XSFX { get; set; }
        public int Count { get; set; }
        public int pass_or_out { get; set; }
        public int Timestamp { get; set; }
    }


    //ftp中的kakoudata.json的类格式-不是新建的表
    public class JsonKaKouDataStruct
    {
        public List<JsonKaKouDataItemStruct> JinArray = new List<JsonKaKouDataItemStruct>();
        public List<JsonKaKouDataItemStruct> ChuArray = new List<JsonKaKouDataItemStruct>();
        public List<JsonKaKouDataItemStruct> AllArray = new List<JsonKaKouDataItemStruct>();
        public List<JsonKaKouDataItemStruct> TopArray = new List<JsonKaKouDataItemStruct>();
    }
    public class JsonKaKouDataItemStruct
    {
        public string SBBH { get; set; }
        public string SBMC { get; set; }
        public string XSFX { get; set; }
        public int Count { get; set; }
        public int pass_or_out { get; set; }
    }

}
