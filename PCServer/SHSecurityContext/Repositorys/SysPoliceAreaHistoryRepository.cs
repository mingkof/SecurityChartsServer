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
    public class SysPoliceAreaHistoryRepository : BaseRepository<sys_policeareahistory>,ISysPoliceAreaHistoryRepository
    {
        public SysPoliceAreaHistoryRepository(SHSecuritySysContext context)
        {
            nContext = context;
        }
    }
    
}
