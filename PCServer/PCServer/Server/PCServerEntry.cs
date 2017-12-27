using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MKServerWeb.Server;
using PCServer.Redis;
using PCServer.Server.GPS;
using PCServer.Server.GPSSocket;
using SHSecurityContext.DBContext;
using SHSecurityContext.IRepositorys;
using SHSecurityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PCServer.Server
{
    public class PCServerEntry
    {
        public async void Init(bool isPublishGongAn = false)
        {
            using (var serviceScope = ServiceLocator.Instance.CreateScope())
            {
                //获取Context
                var dbContext = serviceScope.ServiceProvider.GetService<SHSecuritySysContext>();

                //自动迁移
                await new DbInitializer().InitializeAsync(dbContext);
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

                        //启动GPSSocket服务
                        GPSSocketClient GPSSocketClient = new GPSSocketClient();
                        GPSSocketClient.Run(IPoliceGpsRepo);


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


            return;
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




    }
}
