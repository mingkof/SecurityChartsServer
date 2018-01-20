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
    public class SysWifiTableRepository : BaseRepository<sys_wifitable>, ISysWifiTableRepository
    {
        public SysWifiTableRepository(PPCServerContext context)
        {
            nContext = context;
        }
    }
}
