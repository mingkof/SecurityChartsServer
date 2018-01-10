using SHSecurityContext.Base;
using SHSecurityContext.DBContext;
using SHSecurityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SHSecurityContext.IRepositorys
{
    public interface IKaKouDataJinRepository : IBaseRepository<kakoudata_jin>
    {

    }
     public interface IKaKouDataJinHistoryRepository : IBaseRepository<kakoudata_jin_history>
    {

    }
}
