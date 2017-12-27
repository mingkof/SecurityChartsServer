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
    public class SysTicketresRepository : BaseRepository<sys_ticketres>, ISysTicketresRepository
    {
        public SysTicketresRepository(SHSecuritySysContext context)
        {
            nContext = context;
        }

    }
}
