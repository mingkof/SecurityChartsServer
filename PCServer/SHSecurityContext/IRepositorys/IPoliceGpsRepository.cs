using SHSecurityContext.Base;
using SHSecurityContext.DBContext;
using SHSecurityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SHSecurityContext.IRepositorys
{
    public interface IPoliceGpsRepository : IBaseRepository<PoliceGPS>
    {
    }

    public interface IGpsGridRepository : IBaseRepository<sys_GpsGridWarn>
    {
    }

    public interface ICamerasRepository : IBaseRepository<sys_cameras>
    {
    }

    public interface ICamePeopleCountRepository : IBaseRepository<sys_camPeopleCount>
    {
    }


}
