using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SHSecurityModels
{
    public enum ArgKeyEnum
    {
        kRole = 1, //角色分类
        kAccountType = 2, //账户类型
    }

    //TBS用户账户类型
    public enum TbsUserAccountStateEnum
    {
        kNormal = 0,  //正常
        kForbit = 1, //禁止
        kUnNormal = 2, //异常
        kHasNoPwd = 3, //没有登录密码
    }

    //系统用户
    public class TBSUser
    {
        //[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }          //根据账户类型数字自定义开头

        [Required]  //必须 不可更改
        [RegularExpression(@"^[A-Za-z0-9]+$")] //数字和26个英文字母组成
        [StringLength(maximumLength:12,MinimumLength = 3)]  //长度3-12
        public string UserName { get; set; }    

        public string RealName { get; set; }
        public string Sex { get; set; }

        [RegularExpression(@"^[a-zA-Z]\w{5,17}$")] // 正确格式为：以字母开头，长度在6 ~18之间，只能包含字符、数字和下划线
        public string Password { get; set; }


        //角色-对应角色拥有的权限
        [Required]
        public string Role { get; set; }

        //账户类型
        public string UserAccountType { get; set; }

        //账户状态  正常/禁用/异常
        [Required]
        public TbsUserAccountStateEnum AccountState { get; set; }

        [DefaultValue(false)]
        public bool delete { get; set; }
    }

    //系统参数配置 [可删除]
    //1.角色 删除之前需确认没有地方用到
    //2.账户类型 删除前需确认没有地方用到
    public class AccountArgs
    {
        public int Id { get; set; }
        public ArgKeyEnum ArgKey { get; set; }
        public string ArgValue { get; set; }
        public int ArgValueInt { get; set; }
    } 

    //权限组
    public class RolePermitGroup
    {
        [Key]
        [Required]
        public string GroupName { get; set; }      //不可更改
        public string Desc { get; set; }
        public string PermitPoints { get; set; }
    }


}
