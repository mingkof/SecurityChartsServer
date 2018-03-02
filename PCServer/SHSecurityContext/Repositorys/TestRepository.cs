using System;
using System.Collections.Generic;
using SHSecurityContext.Base;
using SHSecurityContext.IRepositorys;
using SHSecurityModels;
using SHSecurityContext.DBContext;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace SHSecurityContext.Repositorys
{
    public class TestRepository : BaseRepository<db_test>, ITestReposity
    {
        public TestRepository(SHSecuritySysContext context)
        {
            nContext = context;

        }
        public db_test Get(int id)
        {
            return Find(p =>p.id == id);
        }
    }
}
