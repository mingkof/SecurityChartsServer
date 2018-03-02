using System;
using SHSecurityContext.Base;
using SHSecurityModels;
using System.Collections.Generic;
using System.Text;

namespace SHSecurityContext.IRepositorys
{
   public interface ITestReposity :IBaseRepository<db_test>
    {
        ///<summary>
        ///根据id获取原始数据
        ///</summary>
        db_test Get(int id);

    }
}
