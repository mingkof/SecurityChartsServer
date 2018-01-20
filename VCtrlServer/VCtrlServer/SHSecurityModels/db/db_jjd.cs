using System;
using System.Collections.Generic;
using System.Text;

namespace SHSecurityModels
{
    /********************************************************************************
    ** 类名称： db_jjd
    ** 描述：DB映射类-警情数据
    ** 作者： keven
    ** 创建时间：2017-09-20
    ** 最后修改人：（无）
    ** 最后修改时间：（无）
    *********************************************************************************/
    public class db_jjd
    {
        public int id { get; set; }                         //自增长ID
        public string jjdid { get; set; }                   //警情数据库ID
        public string datetime { get; set; }                //日期YEAR+MONTH+DAY+HH+MM+SS
        public string qy { get; set; }                      //分属
        public string cjdw { get; set; }                    //管辖地
        public string af_addr { get; set; }                 //案件地址
        public string bjay1 { get; set; }                   //类型
        public string bjay2 { get; set; }                   //子类型
        public string amap_gps_x { get; set; }              //经度
        public string amap_gps_y { get; set; }              //纬度

    }

}
