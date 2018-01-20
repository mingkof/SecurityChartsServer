using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SHSecurityModels.tbsdb
{
    public enum CustomerTypeEnum
    {
        company = 0,
        person = 1,
    }


    public class Customer
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CustomerID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerNumber { get; set; }

        [Key, Column(Order = 1)]
        public string CustomerName { get; set; }

        //地区区域
        [MaxLength (10)]
        public string Area { get; set; }

        //客户类型 公司/个人
        public CustomerTypeEnum CustomerType { get; set; }

        //办公电话
        [MaxLength (20)]
        public string Phone { get; set; }
        //移动电话
        [MaxLength(20)]
        public string Telephone { get; set; }
        //传真
        [MaxLength(20)]
        public string Fax { get; set; }
        //地址
        [MaxLength(50)]
        public string Address { get; set; }
        //邮编
        [MaxLength(10)]
        public string YB { get; set; }
        //邮箱
        [MaxLength(20)]
        public string Mail { get; set; }
    }





}
