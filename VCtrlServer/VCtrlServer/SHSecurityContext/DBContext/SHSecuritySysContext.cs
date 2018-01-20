using Microsoft.EntityFrameworkCore;
using SHSecurityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHSecurityContext.DBContext
{
    public class DbInitializer
    {
        public async Task InitializeAsync(PPCServerContext context)
        {
            //var migrations = await context.Database.GetPendingMigrationsAsync();//获取未应用的Migrations，不必要，MigrateAsync方法会自动处理
            await context.Database.MigrateAsync();//根据Migrations修改/创建数据库
        }
    }

    public class PPCServerContext : DbContext
    {
        public PPCServerContext(DbContextOptions<PPCServerContext> options) : base(options)
        {
        }

        //public DbSet<db_jjd> db_jjds { get; set; }
        //public DbSet<sys_wifitable> sys_wifitable { get; set; }
        //public DbSet<sys_110warningdb> sys_110warningdb { get; set; }
        //public DbSet<sys_ticketres> sys_ticketres { get; set; }
        public DbSet<sys_config> sys_config { get; set; }

        //public DbSet<TBSUser> tbs_users { get; set; }
        //public DbSet<AccountArgs> tbs_account_args { get; set; }
        //public DbSet<RolePermitGroup> tbs_role_permit_groups { get; set; }

        public DbSet<sys_sipport> sys_sipports { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    => optionsBuilder
        // .UseMySql(@"Server=127.0.0.1;database=SHSecuritySys;uid=strike2014;pwd=strike@2014;");

    }
}
