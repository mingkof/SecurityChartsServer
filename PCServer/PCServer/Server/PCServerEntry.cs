using KVDDDCore.Utils;
using Microsoft.AspNetCore.NodeServices;
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
using Microsoft.AspNetCore.Hosting;

namespace PCServer.Server
{
    public class PCServerEntry
    {
       public PoliceGpsStaticAreaManager PoliceGpsStaticAreaManager = new PoliceGpsStaticAreaManager();

        //WifiDataAreas 信息
        public WifiDataAreaStruct WifiDataAreas = new WifiDataAreaStruct();

        public SHSecuritySysContext dbContext = null;

        public Dictionary<string, List<string>> wifiConfigDic = new Dictionary<string, List<string>>();

        //卡口排名
        public int topCount = 5;
        List<KakouTop> topList = new List<KakouTop>();
        public async void Init(bool isPublishGongAn = false)
        {
            ReadConfig_WifiDataAreas();
            ReadConfig_WifiLocationConfig();
            test();

            using (var serviceScope = ServiceLocator.Instance.CreateScope())
            {
                //获取Context
                 dbContext = serviceScope.ServiceProvider.GetService<SHSecuritySysContext>();

                //自动迁移
                await new DbInitializer().InitializeAsync(dbContext);
                //迁移创建命令: Add-Migration init_data


                //确保已创建
                await dbContext.Database.EnsureCreatedAsync();


                //初始化数据库数据
                await InitDatabase(dbContext);

                ReadCameraData();

                var IPoliceGpsRepo = serviceScope.ServiceProvider.GetService<IPoliceGpsRepository>();

                if (isPublishGongAn)
                {
                    var nodeServices = serviceScope.ServiceProvider.GetService<INodeServices>();
                    var configRepo = serviceScope.ServiceProvider.GetService<ISysConfigRepository>();
                    var systicketRepo = serviceScope.ServiceProvider.GetService<ISysTicketresRepository>();

                    NodeServer.SyncPoliceData(nodeServices, configRepo);
                    NodeServer.InitTicketResultData(nodeServices, systicketRepo, configRepo);
                }
            }

            if (isPublishGongAn)
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

                //GPSGridServer.Run();



                //每隔1分钟，读取ftp文件resultAll.json，更新wifidatapeoples和wifidatapeopleshistory表.
                ThreadPool.QueueUserWorkItem((a) =>
                {

                    using (var serviceScope = ServiceLocator.Instance.CreateScope())
                    {
                        var IWifiDataHistory = serviceScope.ServiceProvider.GetService<IWifiDataPeoplesHistoryRepository>();
                        var IWifiData = serviceScope.ServiceProvider.GetService<IWifiDataPeoplesRepository>();

                        var RealDataConfig = serviceScope.ServiceProvider.GetService<IOptions<RealDataUrl>>();

                        string path = "resultAll.json";

                        
                        while (true)
                        {
                            Logmng.Logger.Trace("ThreadPool=1-----------正在读取：resultAll.json");
                            FtpClient ftpClient = new FtpClient(RealDataConfig.Value.ip, RealDataConfig.Value.username, RealDataConfig.Value.userpassword);

                            string str = ftpClient.DownloadToStr(path);
                            if (!string.IsNullOrEmpty(str))
                            {
                                    JsonWifiDataStruct res = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonWifiDataStruct>(str);

                                    // Logmng.Logger.Trace(str);

                                    if(res != null)
                                    {
                                        // Logmng.Logger.Trace("A5555555555  read ftp resultAll.json");
                                        //存储到自己的数据库

                                        int timeNow = TimeUtils.ConvertToTimeStampNow();

                                        for (int i = 0; i < res.peopleList.Count; i++)
                                        {
                                            var item = res.peopleList[i];

                                            var query = IWifiData.Find(p => p.WifiID == item.wifiID);
                                            if(query == null)
                                            {
                                                int areaId = 0;
                                                for (int m = 0; m < WifiDataAreas.data.Count; m++)
                                                {
                                                    if(WifiDataAreas.data[m].ids.Contains(item.wifiID))
                                                    {
                                                        areaId = WifiDataAreas.data[m].id;
                                                        break;
                                                    }
                                                }


                                                IWifiData.Add(new wifidata_peoples()
                                                {
                                                    WifiID = item.wifiID,
                                                    Count = item.count,
                                                    Timestamp = timeNow,
                                                    AreaId = areaId
                                                });
                                            } else
                                            {
                                                query.Count = item.count;
                                                query.Timestamp = timeNow;
                                                IWifiData.Update(query);
                                            }

                                            //从57分到03分 记录且记录一次
                                            var YEAR = System.DateTime.Now.Year.ToString();
                                            var MONTH = System.DateTime.Now.Month.ToString("00");
                                            var DAY = System.DateTime.Now.Day.ToString("00");
                                            var HH = System.DateTime.Now.Hour.ToString("00");
                                            var MM = System.DateTime.Now.Minute.ToString("00");
                                            var SS = System.DateTime.Now.Second.ToString("00");

                                            var curMinute = System.DateTime.Now.Minute;

                                            // Logmng.Logger.Trace("A5555555555");
                                            
                                            if (curMinute >= 1 && curMinute <= 59)
                                            {
                                                var queryHis = IWifiDataHistory.Find(p => p.WifiID == item.wifiID && p.Year == YEAR && p.Month == MONTH && p.Day == DAY && p.HH == HH);
                                                if(queryHis == null)
                                                {
                                                    IWifiDataHistory.Add(new wifidata_peoples_history()
                                                    {
                                                        Timestamp = timeNow,
                                                        WifiID = item.wifiID,
                                                        Count = item.count,
                                                         Year = YEAR,
                                                         Month = MONTH,
                                                         Day = DAY,
                                                          HH = HH,
                                                          MM = MM,
                                                          SS = SS
                                                    });
                                                }
                                            }

                                        }
                                }
                            }

                            Thread.Sleep(60 * 1000);
                        }
                    }
                });

                 //每隔1分钟，读取ftp文件KaKouData.json，更新kakoudatajin和kakoudatajinhistory表.
                ThreadPool.QueueUserWorkItem((a) =>
                {

                    using (var serviceScope = ServiceLocator.Instance.CreateScope())
                    {
                        var IKaKouDataJinHistory = serviceScope.ServiceProvider.GetService<IKaKouDataJinHistoryRepository>();
                        var IKaKouDataJin = serviceScope.ServiceProvider.GetService<IKaKouDataJinRepository>();
                        var IKaKouTop = serviceScope.ServiceProvider.GetService<IKaKouTopRepository>();
                        var RealDataConfig = serviceScope.ServiceProvider.GetService<IOptions<RealDataUrl>>();

                        string path = "KaKouData.json";
                        while(true)
                        {
                            Logmng.Logger.Trace("ThreadPool=2-----------正在读取：KaKouData.json");
                            FtpClient ftpClient = new FtpClient(RealDataConfig.Value.ip, RealDataConfig.Value.username, RealDataConfig.Value.userpassword);
                            // Logmng.Logger.Trace("q11111111111111111111");
                            string str = ftpClient.DownloadToStr(path);
                            if(!string.IsNullOrEmpty(str))
                            {
                                // Logmng.Logger.Trace(str);
                                JsonKaKouDataStruct res = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonKaKouDataStruct>(str);
                                if(res!=null)
                                {
                                    int timeNow=TimeUtils.ConvertToTimeStampNow();
                                    for(int m = 0;m<res.JinArray.Count;m++)
                                    {
                                        var item=res.JinArray[m];
                                        var query=IKaKouDataJin.Find(p=>p.SBBHID==item.SBBH);
                                        if(query==null)
                                        {
                                            IKaKouDataJin.Add(new kakoudata_jin(){
                                                SBBHID=item.SBBH,
                                                SBMC=item.SBMC,
                                                XSFX=item.XSFX,
                                                Count=item.Count,
                                                pass_or_out=item.pass_or_out,
                                                Timestamp=timeNow
                                            });
                                        }
                                        else
                                        {
                                            query.XSFX=item.XSFX;
                                            query.Count=item.Count;
                                            query.pass_or_out=item.pass_or_out;
                                            query.Timestamp=timeNow;
                                            IKaKouDataJin.Update(query);
                                        }

                                        var curMinute = System.DateTime.Now.Minute;
                                        var YEAR = System.DateTime.Now.Year.ToString();
                                        var MONTH = System.DateTime.Now.Month.ToString("00");
                                        var DAY = System.DateTime.Now.Day.ToString("00");
                                        var HH = System.DateTime.Now.Hour.ToString("00");
                                        if (curMinute>=52||curMinute<=1)
                                        {
                                            
                                            var queryHis=IKaKouDataJinHistory.Find(p=>p.SBBHID==item.SBBH&&p.Year==YEAR&&p.Month==MONTH&&p.Day==DAY&&p.HH==HH);

                                            if (queryHis==null)
                                            {
                                                IKaKouDataJinHistory.Add(new kakoudata_jin_history(){
                                                    Year=YEAR,
                                                    Month=MONTH,
                                                    Day=DAY,
                                                    HH=HH,
                                                    SBBHID=item.SBBH,
                                                    SBMC=item.SBMC,
                                                    XSFX=item.XSFX,
                                                    Count=item.Count,
                                                    pass_or_out=item.pass_or_out,
                                                });
                                            }
                                        }

                                        //记录卡口top5
                                        KakouTop kakou = new KakouTop()
                                        {
                                            SBBHID = item.SBBH,
                                            Value=int.Parse(item.Count),
                                            Year= YEAR,
                                            Month=MONTH,
                                            Day=DAY,
                                            Timestamp=timeNow
                                        };
                                        //判断是否超过5
                                        if (topList.Count < topCount)
                                        {
                                        
                                            topList.Add(kakou);
                                            if (topList.Count == topCount)
                                            {
                                                var queryList = IKaKouTop.FindList(p => true, "",false);
                                                if(queryList != null){
                                                    IKaKouTop.RemoveRange(queryList);
                                                }
                                                for(int i=0; i<topList.Count;i++)
                                                {
                                                    IKaKouTop.Add(topList[i]);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //降序排列
                                            topList = topList.OrderByDescending(p => p.Value).ToList();
                                            var qu = IKaKouTop.Find(p => p.SBBHID == kakou.SBBHID);
                                            if (topList[topList.Count-1].Value<kakou.Value&&qu==null)
                                            {
                                                topList.RemoveAt(topList.Count - 1);
                                                topList.Add(kakou);
                                                var queryList = IKaKouTop.FindList(p => true, "",false);
                                                if(queryList != null){
                                                    IKaKouTop.RemoveRange(queryList);
                                                }
                                                for(int i=0; i<topList.Count;i++)
                                                {
                                                    IKaKouTop.Add(topList[i]);
                                                }
                                            }
                                           
                                        }
                                    }
                                }
                            }
                            Thread.Sleep(1000*60);
                        }
                    }
                });

                 //每隔1分钟，读取ftp文件抓拍 Travio.json，更新traviodata
                ThreadPool.QueueUserWorkItem((a) =>
                {

                    using (var serviceScope = ServiceLocator.Instance.CreateScope())
                    {
                        var ITravioData = serviceScope.ServiceProvider.GetService<ITravioDataRepositoy>();

                        var RealDataConfig = serviceScope.ServiceProvider.GetService<IOptions<RealDataUrl>>();

                        string path = "AllTravioInfo.json";
                        while(true)
                        {
                            Logmng.Logger.Trace("ThreadPool=3-----------正在读取：AllTravioInfo.json");
                            FtpClient ftpClient = new FtpClient(RealDataConfig.Value.ip, RealDataConfig.Value.username, RealDataConfig.Value.userpassword);
                            string str = ftpClient.DownloadToStr(path);
                            if(!string.IsNullOrEmpty(str))
                            {
                                int timeNow=TimeUtils.ConvertToTimeStampNow();
                                // Logmng.Logger.Trace(str);
                                TraVioInfoStruct res = Newtonsoft.Json.JsonConvert.DeserializeObject<TraVioInfoStruct>(str);
                                var count = res.NearWeekCount.nearWeekCount;

                                var YEAR = System.DateTime.Now.Year.ToString();
                                var MONTH = System.DateTime.Now.Month.ToString("00");
                                var DAY = System.DateTime.Now.Day.ToString("00");

                                var query =ITravioData.Find(p=>p.Year==YEAR&&p.Month==MONTH&&p.Day==DAY);
                                if(query!=null)
                                {
                                    query.TodayCount=count;
                                    query.TimeStamp=timeNow;
                                    ITravioData.Update(query);
                                }
                                else
                                {
                                    ITravioData.Add(new traviodata(){
                                        Year=YEAR,
                                        Month=MONTH,
                                        Day=DAY,
                                        TodayCount=count,
                                        TimeStamp=timeNow
                                    });
                                }
                            }
                            Thread.Sleep(1000*60);
                        }
                    }
                });

                //每隔五分钟记录roaddata   更新roaddatarecord
                ThreadPool.QueueUserWorkItem((a) =>
                {

                    using (var serviceScope = ServiceLocator.Instance.CreateScope())
                    {
                        var IRoadDataRecord = serviceScope.ServiceProvider.GetService<IRoadDataRecordRepository>();
                        RealDataUrl RealDataConfig = serviceScope.ServiceProvider.GetService<IOptions<RealDataUrl>>().Value;
                        while(true)
                        {
                            Logmng.Logger.Trace("ThreadPool=4-----------正在读取：AllTravioInfo.json");
                            int timeNow=TimeUtils.ConvertToTimeStampNow();
                            var model = WebClientUls.GetString(RealDataConfig.TrafficUrl);
                            var modelRoadTop = WebClientUls.GetString(RealDataConfig.RoadUrl);

                            TrafficData tampData = null;
                            if (model != null)
                            {
                                // _logger.LogInformation(model["data"]["overview"]["traIndex"].ToString());
                                tampData = new TrafficData
                                {
                                    TrafficDataForAll = model["data"]["overview"]["traIndex"].ToString(),
                                    TrafficAvgSpeed = model["data"]["overview"]["avgSpeed"].ToString(),
                                    TopsRoads = new TrafficRoadState[5]
                                };
                            }
                            if (modelRoadTop != null)
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    tampData.TopsRoads[i] = new TrafficRoadState(
                                            modelRoadTop["data"]["rows"][i]["roadName"].ToString(),
                                            modelRoadTop["data"]["rows"][i]["speed"].ToString(),
                                            modelRoadTop["data"]["rows"][i]["traIndex"].ToString());
                                }
                            }
                            if(tampData!=null)
                            {
                                // JsonRoadDataStruct res = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonRoadDataStruct>(tampData);
                                var YEAR = System.DateTime.Now.Year.ToString();
                                var MONTH = System.DateTime.Now.Month.ToString("00");
                                var DAY = System.DateTime.Now.Day.ToString("00");
                                var HH = System.DateTime.Now.Hour.ToString("00");
                                var MM = System.DateTime.Now.Minute.ToString("00");
                                for (int i=0;i<tampData.TopsRoads.Count();i++)
                                {
                                    var item = tampData.TopsRoads[i];
                                    IRoadDataRecord.Add(new RoadDataRecord{
                                        Timestamp =timeNow,
                                        Year =YEAR,
                                        Month =MONTH,
                                        Day =DAY,
                                        HH =HH,
                                        MM = MM,
                                        Roadname=item.RoadName, 
                                        TrafficAvgSpeed=item.TrafficAvgSpeed,
                                        TrafficData =item.TrafficData
                                    });
                                }
                            }

                            Thread.Sleep(1000*60*3);
                        }
                    }
                });

                //每隔1分钟读取mqServerData  更新mqServerData
                ThreadPool.QueueUserWorkItem((a) => 
                {
                    using (var serviceScope = ServiceLocator.Instance.CreateScope())
                    {
                        var IMQServerData = serviceScope.ServiceProvider.GetService<IMQServerDataRepository>();

                        var RealDataConfig = serviceScope.ServiceProvider.GetService<IOptions<RealDataUrl>>();

                        while (true)
                        {
                            Logmng.Logger.Trace("ThreadPool=5-----------正在读取：ftp MQ物联报警数据");
                            var YEAR = System.DateTime.Now.Year.ToString();
                            var MONTH = System.DateTime.Now.Month.ToString("00");
                            var DAY = System.DateTime.Now.Day.ToString("00");
                            var HH = System.DateTime.Now.Hour.ToString("00");
                            string path = "receive/rcwj/" + "wulian_" + YEAR +MONTH+DAY+HH+".txt";

                            FtpClient ftpClient = new FtpClient("ftp://10.15.55.15:32121/", "zbfjrcwj", "zbfjrcwj");
                            List<string> strList = ftpClient.DownloadToListStr(path);
                            if (strList.Count!=0)
                            {
                                for (int i = 0; i < strList.Count; i++)
                                {
                                    JsonMQServerDataContruct data = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonMQServerDataContruct>(strList[i]);
                                    var query = IMQServerData.Find(p => p.dsnum == data.dsnum && p.mtype == data.mtype && p.time == data.time);
                                    if (query==null)
                                    {
                                        IMQServerData.Add(new MQServerData {
                                            commRmk = data.commRmk,
                                            dsnum = data.dsnum,
                                            foreignld = data.foreignld,
                                            mtype = data.mtype,
                                            projectId = data.projectId,
                                            time = data.time,
                                            userId = data.userId,
                                            topicType = data.topicType,
                                            timeStamp = data.timeStamp
                                        });
                                    }
                                }
                            }
                            Thread.Sleep(1000 * 60);
                        }
                    }
                });

                //每隔1分钟读取 ftp  sqlserver hongwaiPeopleData 更新hongwaiPeopleData
                ThreadPool.QueueUserWorkItem((a) =>
                {
                    using (var serviceScope = ServiceLocator.Instance.CreateScope())
                    {
                        var Ihongwaipeople = serviceScope.ServiceProvider.GetService<IHongWaiPeopleDataRepositoy>();

                        var RealDataConfig = serviceScope.ServiceProvider.GetService<IOptions<RealDataUrl>>();

                        while (true)
                        {
                            Logmng.Logger.Trace("ThreadPool=6-----------正在读取：ftp 红外报警数据");
                            var YEAR = System.DateTime.Now.Year.ToString();
                            var MONTH = System.DateTime.Now.Month.ToString("00");
                            var DAY = System.DateTime.Now.Day.ToString("00");
                            var HH = System.DateTime.Now.Hour.ToString("00");

                            string path = "receive/rcwj/" + "hongwai_" + YEAR+"-" + MONTH + "-" + DAY + ".txt";

                            FtpClient ftpClient = new FtpClient("ftp://10.15.55.15:32121/", "zbfjrcwj", "zbfjrcwj");
                            List<string> strList = ftpClient.DownloadToListStr(path);
                            if (strList.Count!=0)
                            {
                                int timeNow = TimeUtils.ConvertToTimeStampNow();
                                for (int i = 0; i < strList.Count; i++)
                                {
                                    JosnHongWaiStruct data = Newtonsoft.Json.JsonConvert.DeserializeObject<JosnHongWaiStruct>(strList[i]);
                                    var query = Ihongwaipeople.Find(p => p.sn == data.sn && p.type == data.type&&p.Year==YEAR&&p.Month==MONTH&&p.Day==DAY);
                                    if (query != null)
                                    {
                                        query.count = data.count;

                                        Ihongwaipeople.Update(query);
                                    }
                                    else
                                    {
                                        Ihongwaipeople.Add(new HongWaiPeopleData {
                                            sn=data.sn,
                                            count=data.count,
                                            type=data.type,
                                            timeStamp=timeNow,
                                            Year=YEAR,
                                            Month=MONTH,
                                            Day=DAY
                                        });
                                    }
                                }
                            }
                            Thread.Sleep(1000 * 60 *1);
                        }
                    }
                });

                //每隔1分钟读取 ftp人脸识别信息 更新FaceAlarmData
                ThreadPool.QueueUserWorkItem((a) =>
                {
                    using (var serviceScope = ServiceLocator.Instance.CreateScope())
                    {
                        var IFaceAlarmData = serviceScope.ServiceProvider.GetService<IFaceAlarmDataRepositoy>();

                        var RealDataConfig = serviceScope.ServiceProvider.GetService<IOptions<RealDataUrl>>();
                        var hostingEnv = serviceScope.ServiceProvider.GetService<IHostingEnvironment>();

                        while (true)
                        {
                            Logmng.Logger.Trace("ThreadPool=7-----------正在读取：ftp人脸识别信息");
                            var YEAR = System.DateTime.Now.Year.ToString();
                            var MONTH = System.DateTime.Now.Month.ToString("00");
                            var DAY = System.DateTime.Now.Day.ToString("00");
                            var HH = System.DateTime.Now.Hour.ToString("00");

                            string path = hostingEnv.WebRootPath+ @"\FaceAlarmData\AlarmData";
                            
                            FtpClient ftpClient = new FtpClient(RealDataConfig.Value.ip, RealDataConfig.Value.username, RealDataConfig.Value.userpassword);
                            //List<string> strList = FileUtils.ReadDirChild(@"\\38.104.104.4\FaceAlarmData\AlarmData", "data.json");
                            List<string> strList = FileUtils.ReadDirChild(path, "data.json");
                            if (strList.Count != 0)
                            {
                                for (int i = 0; i < strList.Count; i++)
                                {
                                    try
                                    {
                                        if (string.IsNullOrEmpty(strList[i]))
                                            continue;
                                        JsonFaceStruct data = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonFaceStruct>(strList[i]);
                                        var query = IFaceAlarmData.Find(p => p.alarmId == data.alarmId);
                                        if (query == null)
                                        {
                                            List<string> list = new List<string>();
                                            for (int m = 0; m < data.humans.Count; m++)
                                            {
                                                list.Add(data.humans[m].humanId);
                                            }
                                            string matchString = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                                            IFaceAlarmData.Add(new FaceAlarmData
                                            {
                                                alarmTime = data.alarmTime,
                                                timeStamp = TimeUtils.ConvertToTimeStamps(data.alarmTime),
                                                cameraName = data.cameraName,
                                                position = "上海火车站",
                                                alarmId = data.alarmId,
                                                humanId = data.humanId,
                                                humanName = data.humanName,
                                                matchHumanList = matchString,
                                                Year=YEAR,
                                                Month=MONTH,
                                                Day=DAY
                                            });
                                        }
                                    }
                                    catch 
                                    {
                                        continue;
                                    }
                                }
                            }
                            Thread.Sleep(1000 * 60 * 5);
                        }
                    }
                });

            }
            //弃用  服务器不用分析区域 改为api设置  读取配置
            //ReadConfig_PoliceGpsStaticAreas();


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
            //用于视频监控图表
            AddConf(context, SHSecurityModels.EConfigKey.tb1_cam1_id, "0", 0);
            AddConf(context, SHSecurityModels.EConfigKey.tb1_cam2_id, "0", 0);
            AddConf(context, SHSecurityModels.EConfigKey.tb1_cam3_id, "0", 0);
            AddConf(context, SHSecurityModels.EConfigKey.tb1_cam4_id, "0", 0);
            AddConf(context, SHSecurityModels.EConfigKey.tb2_cam1_id, "0", 0);
            AddConf(context, SHSecurityModels.EConfigKey.tb2_cam2_id, "0", 0);
            AddConf(context, SHSecurityModels.EConfigKey.tb2_cam3_id, "0", 0);
            AddConf(context, SHSecurityModels.EConfigKey.tb2_cam4_id, "0", 0);
            AddConf(context, SHSecurityModels.EConfigKey.tb1_cam1_url, "0", 0);
            AddConf(context, SHSecurityModels.EConfigKey.tb1_cam2_url, "0", 0);
            AddConf(context, SHSecurityModels.EConfigKey.tb1_cam3_url, "0", 0);
            AddConf(context, SHSecurityModels.EConfigKey.tb1_cam4_url, "0", 0);
            AddConf(context, SHSecurityModels.EConfigKey.tb2_cam1_url, "0", 0);
            AddConf(context, SHSecurityModels.EConfigKey.tb2_cam2_url, "0", 0);
            AddConf(context, SHSecurityModels.EConfigKey.tb2_cam3_url, "0", 0);
            AddConf(context, SHSecurityModels.EConfigKey.tb2_cam4_url, "0", 0);

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


                            //转换成世界坐标
                            var c_worldX = "";
                            var c_worldY = "";


                            var query = ICamerasRepository.Find(p => p.id == c_id);
                            if (query != null)
                            {
                                continue;
                            }
                            else
                            {
                                
                                if(!string.IsNullOrEmpty(c_lang) && !string.IsNullOrEmpty(c_lat))
                                {
                                    if(c_lang =="0" && c_lat == "0") {


                                    } else {
                                        GPS.Vector3 vec = GPSUtils.ComputeLocalPositionGCJ(c_lang, c_lat);
                                        c_worldX = vec.x.ToString();
                                        c_worldY = vec.y.ToString();
                                    }
                                }

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
                                    lat = c_lat,
                                    worldX = c_worldX,
                                    worldY = c_worldY
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


        /// <summary>
        /// 读取static下的WifiDataAreas.json文件
        /// </summary>
        void ReadConfig_WifiDataAreas()
        {
            string file = "static/WifiDataAreas.json";
            var content = KVDDDCore.Utils.FileUtils.ReadFile(file);
            WifiDataAreas = Newtonsoft.Json.JsonConvert.DeserializeObject<WifiDataAreaStruct>(content);
        }

        void ReadConfig_WifiLocationConfig()
        {
            try
            {
                List<WifiConfig> configList = new List<WifiConfig>();
                wifiConfigDic["南广场"] = new List<string>();
                wifiConfigDic["北广场"] = new List<string>();
                string file = "static/WifiConfig.json";
                var content = KVDDDCore.Utils.FileUtils.ReadFile(file);
                configList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WifiConfig>>(content);
                for (int i = 0; i < configList.Count; i++)
                {
                    if (configList[i].Locate == "南广场")
                    {
                        wifiConfigDic["南广场"].Add(configList[i].id);
                    }
                    else if (configList[i].Locate == "北广场")
                    {
                        wifiConfigDic["北广场"].Add(configList[i].id);
                    }
                }
            }
            catch
            {
            }
        }
    }





    //wifidata peoples
    public class WifiDataAreaItemStruct
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<string> ids = new List<string>();
        public Dictionary<int, int> history = new Dictionary<int, int>();
    }


    public class WifiDataAreaStruct
    {
        public List<WifiDataAreaItemStruct> data = new List<WifiDataAreaItemStruct>();

    }

    public class WifiConfig
    {
        public string id;
        public string Locate;
    }

}
