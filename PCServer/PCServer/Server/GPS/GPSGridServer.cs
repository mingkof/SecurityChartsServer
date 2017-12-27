using Microsoft.Extensions.DependencyInjection;
using SHSecurityContext.IRepositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PCServer.Server.GPS
{
    public class GPSGridServer
    {
        static int LastResultStaticsTime = 0;


        //ISysConfigRepository configRepo, ISys110WarningRepository _repo_110_warn, IGpsGridRepository _repo_gps_warn

        public static void Run()
        {
            int time = 30000;

            ThreadPool.QueueUserWorkItem(async (a) =>
            {
                using (var serviceScope = ServiceLocator.Instance.CreateScope())
                {
                    var configRepo = serviceScope.ServiceProvider.GetService<ISysConfigRepository>();
                    var _repo_110_warn = serviceScope.ServiceProvider.GetService<ISys110WarningRepository>();
                    var _repo_gps_warn = serviceScope.ServiceProvider.GetService<IGpsGridRepository>();

                    while (true)
                    {
                        await RunStatics(configRepo, _repo_110_warn, _repo_gps_warn);
                        Thread.Sleep(time);
                    }

                }
            });
        }


        async static Task RunStatics(ISysConfigRepository configRepo, ISys110WarningRepository _repo_110_warn, IGpsGridRepository _repo_gps_warn)
        {
            if(LastResultStaticsTime == 0)
            {
                LastResultStaticsTime = Get110WarnLastRunStaticsMaxTimesign(configRepo);
            }

            int Now110MaxTimestamp = _repo_110_warn.Max(p => p.TIMESIGN);

            if (Now110MaxTimestamp == LastResultStaticsTime)
                return;

            var queryList = _repo_110_warn.FindList(p => p.TIMESIGN > LastResultStaticsTime && p.TIMESIGN <= Now110MaxTimestamp,"",false);

            var list = queryList.ToList();

            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                AddGrid(_repo_gps_warn, item.AMAP_GPS_X, item.AMAP_GPS_Y, item.JJD_ID, item.TIMESIGN);
            }

            UpdateLastTimesign(configRepo, Now110MaxTimestamp);
            return; 
        }


        static void AddGrid(IGpsGridRepository _repo_gps_warn, string gps_x, string gps_y, string jjdid, int timestamp)
        {
                Vector3 vec = GPSUtils.ComputeLocalPositionGCJ(gps_x, gps_y);

                int GridX = (int)(vec.x / 100);
                int GridY = (int)(vec.y / 100);

                _repo_gps_warn.Add(new SHSecurityModels.sys_GpsGridWarn()
                {
                    GridName = GridX + "_" + GridY,
                    GridX = GridX,
                    GridY = GridY,
                    JJD_ID = jjdid,
                    JJD_TIMESIGN = timestamp
                });
        }

        static int Get110WarnLastRunStaticsMaxTimesign(ISysConfigRepository configRepo)
        {
            var conf = configRepo.Find(p => p.key == (int)SHSecurityModels.EConfigKey.kGpsGridServerLast110Timestamp);
            if (conf == null)
            {
                configRepo.Add(new SHSecurityModels.sys_config()
                {
                    key = (int)SHSecurityModels.EConfigKey.kGpsGridServerLast110Timestamp,
                    valueInt = 0
                });
                LastResultStaticsTime = 0;
            }
            else
            {
                LastResultStaticsTime = conf.valueInt;
            }

            return LastResultStaticsTime;
        }

        static void UpdateLastTimesign(ISysConfigRepository configRepo, int timestamp)
        {
            var conf = configRepo.Find(p => p.key == (int)SHSecurityModels.EConfigKey.kGpsGridServerLast110Timestamp);
            if (conf == null)
            {
                configRepo.Add(new SHSecurityModels.sys_config()
                {
                    key = (int)SHSecurityModels.EConfigKey.kGpsGridServerLast110Timestamp,
                    valueInt = timestamp
                });
            }

            conf.valueInt = timestamp;
            configRepo.Update(conf);

            LastResultStaticsTime = timestamp;
            return;
        }



    }
}
