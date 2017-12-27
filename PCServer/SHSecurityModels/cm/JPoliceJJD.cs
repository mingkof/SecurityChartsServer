using System;
using System.ComponentModel.DataAnnotations;

namespace SHSecurityModels
{
    /********************************************************************************
    ** 类名称： JPoliceJJD
    ** 描述：传输格式-单条警情数据
    ** 作者： keven
    ** 创建时间：2017-09-20
    ** 最后修改人：（无）
    ** 最后修改时间：（无）
    *********************************************************************************/
    public class JPoliceJJD
    {
        public string JJDId { get; set; }
        public string DateTime { get; set; }
        public string 分属 { get; set; }
        public string 管辖地 { get; set; }
        public string 案件地址 { get; set; }
        public string 类型 { get; set; }
        public string 子类型 { get; set; }
        public string 经度 { get; set; }
        public string 纬度 { get; set; }
    }
}
