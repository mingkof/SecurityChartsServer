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
    public class Sys110WarningRepository : BaseRepository<sys_110warningdb>, ISys110WarningRepository
    {
        public Sys110WarningRepository(SHSecuritySysContext context)
        {
            nContext = context;
        }

        public sys_110warningdb GetWarn(string jjdId)
        {
            if (string.IsNullOrEmpty(jjdId.Trim()))
                return null;

            return Find(p => p.JJD_ID == jjdId);
        }
    }
}
