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
        public async Task InitializeAsync(SHSecuritySysContext context)
        {
            //var migrations = await context.Database.GetPendingMigrationsAsync();//获取未应用的Migrations，不必要，MigrateAsync方法会自动处理
            await context.Database.MigrateAsync();//根据Migrations修改/创建数据库
        }
    }

    public class SHSecuritySysContext : DbContext
    {
        public SHSecuritySysContext(DbContextOptions<SHSecuritySysContext> options) : base(options)
        {
        }

        public DbSet<db_jjd> db_jjds { get; set; }
        public DbSet<sys_wifitable> sys_wifitable { get; set; }
        public DbSet<sys_110warningdb> sys_110warningdb { get; set; }
        public DbSet<sys_ticketres> sys_ticketres { get; set; }
        public DbSet<sys_config> sys_config { get; set; }
        public DbSet<PoliceGPS> police_gps { get; set; }

        public DbSet<sys_GpsGridWarn> gps_grid { get; set; }

        public DbSet<sys_cameras> sys_cameras { get; set; }

        public DbSet<sys_camPeopleCount> sys_camPeopleCount { get; set; }

        public DbSet<PoliceGPSAreaStatic> PoliceGPSAreaStatic { get; set; }

        public DbSet<wifidata_peoples_history> WifiDataPeoplesHistory { get; set; }
        public DbSet<wifidata_peoples> WifiDataPeoples { get; set; }

        public DbSet<kakoudata_jin> KaKouDataJin { get; set; }
        public DbSet<kakoudata_jin_history> KaKouDataJinHistory { get; set; }
        public DbSet<traviodata> TravioData { get; set; }
        public DbSet<KakouTop> KakouTop { get; set; }
        public DbSet<RoadDataRecord> RoadDataRecord { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    => optionsBuilder
        // .UseMySql(@"Server=127.0.0.1;database=SHSecuritySys;uid=strike2014;pwd=strike@2014;");

    }
}
