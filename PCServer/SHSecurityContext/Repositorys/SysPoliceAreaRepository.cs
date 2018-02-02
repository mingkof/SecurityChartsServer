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
    public class SysPoliceAreaRepository : BaseRepository<sys_policearea>,ISysPoliceAreaRepository
    {
        public SysPoliceAreaRepository(SHSecuritySysContext context)
        {
            nContext = context;
        }
    }
    
}
