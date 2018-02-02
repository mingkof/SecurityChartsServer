//卡口模型数据以及ftp中的KaKouData.json的反解析结构体

//mysql模型
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SHSecurityModels
{
    public class sys_policearea
    {
        [Key]
        public string AreaName { get; set; }    //地名
        public int TimeStamp { get; set; }
        public string Count { get; set; }       //数量
    }
    public class sys_policeareahistory
    {
        public int Id { get; set; }
        public string AreaName { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
        public string Hour { get; set; }
        public string Minute { get; set; }
        public string Second { get; set; }
        public int TimeStamp { get; set; }
        public string Count { get; set; }
    }


}
