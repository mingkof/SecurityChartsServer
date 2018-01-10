using SHSecurityContext.Base;
using SHSecurityContext.DBContext;
using SHSecurityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SHSecurityContext.IRepositorys
{
    public interface ISys110WarningRepository : IBaseRepository<sys_110warningdb>
    {
        sys_110warningdb GetWarn(string jjdId);

    }


    public interface IPoliceGPSAreaStaticRepository : IBaseRepository<PoliceGPSAreaStatic>
    {
    }

    public interface IWifiDataPeoplesRepository : IBaseRepository<wifidata_peoples>
    {
    }

    public interface IWifiDataPeoplesHistoryRepository : IBaseRepository<wifidata_peoples_history>
    {
    }
}
