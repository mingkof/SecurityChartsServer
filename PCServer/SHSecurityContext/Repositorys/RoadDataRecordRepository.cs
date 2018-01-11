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
    public class RoadDataRecordRepository : BaseRepository<RoadDataRecord>, IRoadDataRecordRepository
    {
        public RoadDataRecordRepository(SHSecuritySysContext context)
        {
            nContext = context;
        }
    }
}
