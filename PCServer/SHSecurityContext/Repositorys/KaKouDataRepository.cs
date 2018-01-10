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
    public class KaKouDataJinRepository : BaseRepository<kakoudata_jin>, IKaKouDataJinRepository
    {
        public KaKouDataJinRepository(SHSecuritySysContext context)
        {
            nContext = context;
        }
    }
    public class KaKouDataJinHistoryRepository : BaseRepository<kakoudata_jin_history>, IKaKouDataJinHistoryRepository
    {
        public KaKouDataJinHistoryRepository(SHSecuritySysContext context)
        {
            nContext = context;
        }
    }
}
