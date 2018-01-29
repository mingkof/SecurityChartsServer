using SHSecurityContext.Base;
using SHSecurityContext.Base;
using SHSecurityContext.DBContext;
using SHSecurityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SHSecurityContext.IRepositorys
{
    public interface IFaceAlarmDataRepositoy : IBaseRepository<FaceAlarmData>
    {

    }
    public interface IPeopleCountConfigRepositoy :IBaseRepository<sys_PeopleCountConfig>
    {

    }
}
