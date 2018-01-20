using SHSecurityContext.Base;
using SHSecurityContext.DBContext;
using SHSecurityContext.IRepositorys;
using SHSecurityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SHSecurityContext.Repositorys
{
    public class SipPortRepository : BaseRepository<sys_sipport>, ISipPortRepository
    {
        public SipPortRepository(PPCServerContext context)
        {
            nContext = context;
        }
    }


}
