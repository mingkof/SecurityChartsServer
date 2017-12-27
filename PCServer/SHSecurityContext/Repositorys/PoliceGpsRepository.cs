using SHSecurityContext.IRepositorys;
using System;
using System.Collections.Generic;
using System.Text;
using SHSecurityContext.DBContext;
using System.Linq;
using SHSecurityContext.Base;
using SHSecurityModels;
using Microsoft.EntityFrameworkCore;

namespace SHSecurityContext.Repositorys
{
    public class PoliceGpsRepository : BaseRepository<PoliceGPS>, IPoliceGpsRepository
    {
        public PoliceGpsRepository(SHSecuritySysContext context)
        {
            nContext = context;
        }
    }

    public class GpsGridRepository : BaseRepository<sys_GpsGridWarn>, IGpsGridRepository
    {
        public GpsGridRepository(SHSecuritySysContext context)
        {
            nContext = context;
        }
    }


    public class CamerasRepository : BaseRepository<sys_cameras>, ICamerasRepository
    {
        public CamerasRepository(SHSecuritySysContext context)
        {
            nContext = context;
        }
    }

    
   public class CamePeopleCountRepository : BaseRepository<sys_camPeopleCount>, ICamePeopleCountRepository
    {
        public CamePeopleCountRepository(SHSecuritySysContext context)
        {
            nContext = context;
        }
    }
}
