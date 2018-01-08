using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MKServerWeb.Model.RealData;
using MKServerWeb.Server;
using PCServer.Redis;
using PCServer.Server.GPS;
using PCServer.Server.GPSSocket;
using SHSecurityContext.DBContext;
using SHSecurityContext.IRepositorys;
using SHSecurityModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace PCServer.Server
{
    public class PCServerEntry
    {
       public PoliceGpsStaticAreaManager PoliceGpsStaticAreaManager = new PoliceGpsStaticAreaManager();


        public async void Init(bool isPublishGongAn = false)
        {
            test();

            using (var serviceScope = ServiceLocator.Instance.CreateScope())
            {
                //获取Context
                var dbContext = serviceScope.ServiceProvider.GetService<SHSecuritySysContext>();

                //自动迁移
                //await new DbInitializer().InitializeAsync(dbContext);
                //迁移创建命令: Add-Migration init_data


                //确保已创建
                await dbContext.Database.EnsureCreatedAsync();


                //初始化数据库数据
                await InitDatabase(dbContext);

                ReadCameraData();

                var IPoliceGpsRepo = serviceScope.ServiceProvider.GetService<IPoliceGpsRepository>();
            }

            if(isPublishGongAn)
            {
                ThreadPool.QueueUserWorkItem((a) =>
                {
                    using (var serviceScope = ServiceLocator.Instance.CreateScope())
                    {
                        var IPoliceGpsRepo = serviceScope.ServiceProvider.GetService<IPoliceGpsRepository>();
                        var RealDataConfig = serviceScope.ServiceProvider.GetService<IOptions<RealDataUrl>>();

                        //启动GPSSocket服务
                        GPSSocketClient GPSSocketClient = new GPSSocketClient();
                        GPSSocketClient.Run(IPoliceGpsRepo, RealDataConfig);


                        while (true)
                        {
                            if (GPSSocketClient.clientSocket == null || GPSSocketClient.clientSocket.Connected == false)
                            {
                                Logmng.Logger.Trace("GPSSocketClient 断了 重新连接");

                                GPSSocketClient.ConnectServer();
                            }

                            Thread.Sleep(60000);
                        }
                    }
                });

                GPSGridServer.Run();
            }


            //读取配置
            ReadConfig_PoliceGpsStaticAreas();




            return;
        }

        private void test()
        {
           // var point1 = new Point(20, 20);   //1
           // var point2 = new Point(110, 20);   //0
           // var point3 = new Point(20, 70);  //0
           // var point4 = new Point(1, 20);  //1
           // var point5 = new Point(50, 60);  //1
           // var point6 = new Point(50, 71);  //0
           // var point7 = new Point(60, 90);  //0


           // var arr = new Point[] {
           //     new Point(0,0),
           //     new Point(0,30),
           //     new Point(50,70),
           //     new Point(100,70),
           //     new Point(100,0)
           // };

           //bool t1 = KVDDDCore.Utils.PointInPolygon.CheckPointInPolygon(point1, arr);
           // bool t2 = KVDDDCore.Utils.PointInPolygon.CheckPointInPolygon(point2, arr);
           // bool t3 = KVDDDCore.Utils.PointInPolygon.CheckPointInPolygon(point3, arr);
           // bool t4 = KVDDDCore.Utils.PointInPolygon.CheckPointInPolygon(point4, arr);
           // bool t5 = KVDDDCore.Utils.PointInPolygon.CheckPointInPolygon(point5, arr);
           // bool t6 = KVDDDCore.Utils.PointInPolygon.CheckPointInPolygon(point6, arr);
           // bool t7 = KVDDDCore.Utils.PointInPolygon.CheckPointInPolygon(point7, arr);

        }

        private async Task InitDatabase(SHSecuritySysContext context)
        {
            AddConf(context, SHSecurityModels.EConfigKey.kLastResultUpdateTime, "0", 0);
            AddConf(context, SHSecurityModels.EConfigKey.kTimer1, "", 60000);
            AddConf(context, SHSecurityModels.EConfigKey.kTimer2, "0", 30000);

            await context.SaveChangesAsync();

            return;
        }

        private void ReadCameraData()
        {
            using (var serviceScope = ServiceLocator.Instance.CreateScope())
            {
                var ICamerasRepository = serviceScope.ServiceProvider.GetService<ICamerasRepository>();

                string file = "static/cameras.csv";

                var list = KVDDDCore.Utils.FileUtils.ReadFileToList(file);

                if (list != null)
                {
                    //List<sys_cameras> addrangeList = new List<sys_cameras>();

                    for (int i = 0; i < list.Count; i++)
                    {
                        var item = list[i];

                        if (string.IsNullOrEmpty(item))
                            continue;

                        try
                        {
                            //31010811001180006016,华康路秣陵路朝南HG,,,,31010811,,,0,0,31010601002000000002,ON,0,00,,

                            var arr = item.Split(',');

                            if (arr.Length < 13)
                                continue;

                            var c_id = arr[0] ?? "";
                            var c_name = arr[1] ?? "";
                            var c_domain = arr[5] ?? "";
                            var c_back1 = arr[8] ?? "";
                            var c_back2 = arr[9] ?? "";
                            var c_parent = arr[10] ?? "";
                            var c_state = arr[11] ?? "";
                            var c_lang = arr[12] ?? "";
                            var c_lat = arr[13] ?? "";

                            c_lat = c_lat.Substring(c_lang.Length);


                            var query = ICamerasRepository.Find(p => p.id == c_id);
                            if (query != null)
                            {
                                continue;
                            }
                            else
                            {
                                ICamerasRepository.Add(new SHSecurityModels.sys_cameras()
                                {
                                    id = c_id,
                                    name = c_name,
                                    domain = c_domain,
                                    back1 = c_back1,
                                    back2 = c_back2,
                                    parent = c_parent,
                                    state = c_state,
                                    lang = c_lang,
                                    lat = c_lat
                                });
                            }
                        }
                        catch (Exception)
                        {
                            Logmng.Logger.Error("导入Camera出错： Index:" + i);
                        }
                    }

                }
            }
        }

        void AddConf(SHSecuritySysContext context,SHSecurityModels.EConfigKey key, string defaultStr = "", int defaultInt = 0)
        {
            var conf1 = context.sys_config.Where(p => p.key == (int)key);
            if (conf1 == null || conf1.Count() <= 0)
            {
                context.sys_config.Add(new SHSecurityModels.sys_config()
                {
                    key = (int)key,
                    value = defaultStr,
                    valueInt = defaultInt
                });
            }
        }



        /// <summary>
        /// 读取static下的 PoliceGpsStaticAreas.json文件
        /// 目的是为了图表4-警力分布，需要统计按小时，某区域的警员分布人数
        /// 这个配置是区域的 场景世界坐标区域
        /// 需要将gps位置转换，并判断在哪个区域，并记录在数据库，供api使用
        /// </summary>
        void ReadConfig_PoliceGpsStaticAreas ()
        {
            using (var serviceScope = ServiceLocator.Instance.CreateScope())
            {
                var police_area_static_repo = serviceScope.ServiceProvider.GetService<IPoliceGPSAreaStaticRepository>();


                string file = "static/PoliceGpsStaticAreas.json";

                var content = KVDDDCore.Utils.FileUtils.ReadFile(file);


                PoliceGpsStaticAreaManager.AreaConfig = 
                Newtonsoft.Json.JsonConvert.DeserializeObject<PoliceGpsStaticAreaConfig>(content);
                PoliceGpsStaticAreaManager.InitAreaConfig(police_area_static_repo);

                //test
                //var p1 = PoliceGpsStaticAreaManager.CheckInArea(1580, 120);
                //var p12 = PoliceGpsStaticAreaManager.CheckInArea(850, 100);
                //var p2 = PoliceGpsStaticAreaManager.CheckInArea(15750, -200);
                //var p3 = PoliceGpsStaticAreaManager.CheckInArea(15000, -5000);
                //var p4 = PoliceGpsStaticAreaManager.CheckInArea(10666, -100);

            }
        }




        }
}
