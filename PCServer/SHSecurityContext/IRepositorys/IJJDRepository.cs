using SHSecurityContext.Base;
using SHSecurityContext.DBContext;
using SHSecurityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SHSecurityContext.IRepositorys
{
    /********************************************************************************
    ** 接口名称： IJJDRepository
    ** 描述：警情数据提供Repository 
    ** 作者： keven
    ** 创建时间：2017-09-20
    ** 最后修改人：（无）
    ** 最后修改时间：（无）
*********************************************************************************/
    public interface IJJDRepository : IBaseRepository<db_jjd>
    {
        /// <summary>
        /// 根据警情id, 获取警情原始数据库数据
        /// </summary>
        /// <param name="jjdid">警情id</param>
        /// <returns>单个警情信息</returns>
        db_jjd Get(string jjdid);

        /// <summary>
        /// 根据警情id, 获取警情json传输格式
        /// </summary>
        /// <param name="jjdid">警情id</param>
        /// <returns>单个警情信息</returns>
        JPoliceJJD GetCm(string jjdid);

    }
}
