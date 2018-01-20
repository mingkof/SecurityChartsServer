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
    public class JJDRepository : BaseRepository<db_jjd>, IJJDRepository
    {
        public JJDRepository(PPCServerContext context)
        {
            nContext = context;
        }


        public db_jjd Get(string jjdid) {
            return Find(p => p.jjdid == jjdid);
        } 

        public JPoliceJJD GetCm(string jjdid)
        {
            db_jjd query = Get(jjdid);

            return query == null ? null : new JPoliceJJD()
            {
                DateTime = query.datetime,
                分属 = query.qy,
                类型 = query.bjay1,
                子类型 = query.bjay2,
                案件地址 = query.af_addr,
                管辖地 = query.cjdw,
                纬度 = query.amap_gps_y,
                经度 = query.amap_gps_x
            };
        }
    }
}
