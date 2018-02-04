using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using NLog;
using SHSecurityContext.DBContext;
using SHSecurityContext.IRepositorys;
using SHSecurityModels;
using SHSecurityServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PCServer;

namespace MKServerWeb.Server
{
    public class PL_JADB_RESULT {
        public string RESULTID { get; set; }
        public string TICKETUUID { get; set; }
        public string RYLX { get; set; }
        public string CREATETIME { get; set; }
        public string CREATEUSER { get; set; }
        public string UPDATETIME { get; set; }
        public string STATE { get; set; }
        public string FEEDINFO { get; set; }
    }

     public class PL_JADB_TICKET {
        public string UUID { get; set; }
        public string TICKETID { get; set; }
        public string SALEDWAY { get; set; }
        public string FLIGHTDATE { get; set; }
        public string FLIGHTTIME { get; set; }
        public string FROMSTATIONNAME { get; set; }
        public string TOREGIONNAME { get; set; }
        public string PASSENGERNAME { get; set; }
        public string PASSENGERCERTTYPE { get; set; }
        public string PASSENGERCERTNO { get; set; }
        public string PASSENGERPHONE { get; set; }
        public string SEATNO { get; set; }
        public string CREATEDATE { get; set; }
        public string CREATETIME { get; set; }
        public string CREATEUSER { get; set; }
        public string UPDATETIME { get; set; }
        public string UPDATEUSER { get; set; }
      
    }


    public class NodeServer
    {

        public static string LastResultUpdateTime = "0";
        public static bool HadInitTicketResult = false;


        public static void SyncPoliceData(INodeServices nodeServices, ISysConfigRepository configRepo)
        {
            //Logmng.Logger.LogInformation("测试1");

            Logmng.Logger.Trace("SyncPoliceData Start .... (Node/gongan/SyncPoliceData.js)");

            int time = 1 * 60 * 1000;

            var conf = configRepo.Find(p => p.key == (int)SHSecurityModels.EConfigKey.kTimer1);
            if (conf != null)
                time = conf.valueInt;

            ThreadPool.QueueUserWorkItem(async (a) =>
            {
                while (true)
                {
                    Logmng.Logger.Trace("Run At SyncPoliceData Whileing ...");

                    await nodeServices.InvokeAsync<string>(
                          "Node/gongan/SyncPoliceData.js"
                    );

                    Thread.Sleep(time);
                }
            });
        }


        public static void InitTicketResultData(INodeServices nodeServices,
            ISysTicketresRepository systicketRepo,
            ISysConfigRepository configRepo
            ) {


            if (HadInitTicketResult)
            {
                Logmng.Logger.Warn("Warn: InitTicketResultData Had Done !!!)");
                return;
            }


            var conf = configRepo.Find(p => p.key == (int)SHSecurityModels.EConfigKey.kLastResultUpdateTime);
            if (conf == null) {
                configRepo.Add(new SHSecurityModels.sys_config()
                {
                    key = (int)SHSecurityModels.EConfigKey.kLastResultUpdateTime,
                    value = "0"
                });
                LastResultUpdateTime = "0";
            } else
            {
                LastResultUpdateTime = conf.value;
            }


            int time = 1 * 30 * 1000;

            //RunTimer2(time, Task.Run(() =>
            //{
            //    return Test(nodeServices, systicketRepo, configRepo);
            //}),
            //(List<sys_ticketres> list) => {
            //    //Logmng.Logger.Trace("Run At InitTicketResultData Whileing ...");

            //    ISysTicketresRepository so = (ISysTicketresRepository)ServiceLocator.Instance.GetService(typeof(ISysTicketresRepository));

            //    so.AddRange(list);
            //}
            //);

    
            ThreadPool.QueueUserWorkItem(async (a) =>
            {
                using (var serviceScope = ServiceLocator.Instance.CreateScope())
                {
                    //var context = serviceScope.ServiceProvider.GetService<SGDTPContext>();
                    // Seed the database.
                    ISysTicketresRepository ti = serviceScope.ServiceProvider.GetService<ISysTicketresRepository>();
                    ISysConfigRepository ci = serviceScope.ServiceProvider.GetService<ISysConfigRepository>();

                    while (true)
                    {
                        await UpdateTicketResult(nodeServices, ti,ci);

                        Logmng.Logger.Trace("Run At InitTicketResultData Whileing ...");

                        var conf2 = ci.Find(p => p.key == (int)SHSecurityModels.EConfigKey.kTimer2);
                        if (conf2 != null)
                            time = conf2.valueInt;

                        Thread.Sleep(time);
                    }

                }
                 
            });

        }



        //static void Test(ISysTicketresRepository ti)
        //{
        //    //var db = ServiceLocator.Instance.GetService<SHSecuritySysContext>();

        //    //tick.Add(new sys_ticketres()
        //    //{
        //    //    PassageName = "汪子文哈哈哈"
        //    //});

        //    //List<sys_ticketres> list = new List<sys_ticketres>();

        //    //var Mo = new SHSecurityModels.sys_ticketres()
        //    //{
        //    //    PassageName = "汪子文哈哈哈"
        //    //};
        //    //ti.Add(Mo);
        //    //list.Add(Mo);

        //    //return list;

        //}






        static async Task UpdateTicketResult(INodeServices nodeServices,
             ISysTicketresRepository systicketRepo,
               ISysConfigRepository configRepo
             ) {

            try
            {
                string queryMaxUpdateTime = await nodeServices.InvokeAsync<string>(
               "Node/gongan/QueryQCZResultMaxUpdateTime.js", LastResultUpdateTime
           );

                if (LastResultUpdateTime == queryMaxUpdateTime)
                {
                    Logmng.Logger.Trace("InitTicketResultData MaxTime has no new date! LastResultUpdateTime: " + LastResultUpdateTime);
                    return;
                }

                Logmng.Logger.Trace("UpdateTicketResult Start .... UpdateTicketResult - " + LastResultUpdateTime + "   : " + queryMaxUpdateTime);

                //List<PL_JADB_RESULT>
                string queryResultStr = await nodeServices.InvokeAsync<string>(
                   "Node/gongan/QueryQCZResult.js", LastResultUpdateTime, queryMaxUpdateTime
                );

                //Logmng.Logger.Trace("A1 : " + queryResultStr);

                List<string[]> queryResult = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string[]>>(queryResultStr);

                //List<PL_JADB_RESULT> queryResult = new List<PL_JADB_RESULT>();

                Logmng.Logger.Trace("A22222 : " + (queryResult == null ? "null" : queryResult.Count.ToString()));


                if (queryResult == null || queryResult.Count <= 0)
                {
                    Logmng.Logger.Error("ERROR: InitTicketResultData  QueryQCZResult.js Result is NULL OR EMPTY!");
                    return;
                }

                //Logmng.Logger.Trace("InitTicketResultData  at  111");

                List<SHSecurityModels.sys_ticketres> ticketList = new List<SHSecurityModels.sys_ticketres>();

                for (int i = 0; i < queryResult.Count; i++)
                {
                    var res = queryResult[i];

                    //PL_JADB_TICKET
                    string ticketstr = await nodeServices.InvokeAsync<string>(
                       "Node/gongan/QueryQCZTicket.js", res[1] //res.TICKETUUID
                    );

                    List<string[]> tlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string[]>>(ticketstr);

                    if (tlist == null || tlist.Count <= 0)
                    {
                        Logmng.Logger.Trace("InitTicketResultData  at  222： find no ticketuuid: " + res[1]); // queryResult[i].TICKETUUID
                        continue;
                    }

                    var ticket = tlist[0];

                    //Logmng.Logger.Trace("InitTicketResultData  at 2222");

                    //Logmng.Logger.Trace("100: " + ticket[7]);
                    //Logmng.Logger.Trace("101 " + ticket[9]);
                    //Logmng.Logger.Trace("102: " + res[2]);
                    //Logmng.Logger.Trace("103: " + res[7]);
                    //Logmng.Logger.Trace("104: " + ticket[3]);
                    //Logmng.Logger.Trace("105: " + ticket[4]);
                    //Logmng.Logger.Trace("106: " + ticket[5]);
                    //Logmng.Logger.Trace("107: " + ticket[6]);
                    //Logmng.Logger.Trace("108: " + ticket[11]);
                    //Logmng.Logger.Trace("109: " + ticket[12]);
                    //Logmng.Logger.Trace("110: " + ticket[13]);
                    //Logmng.Logger.Trace("111: " + res[5]);


                    //处理Ticket时间和Go时间,记录对应的年月日 方便以后进行统计
                    DateTime.TryParse(ticket[12], out DateTime TicketDT);
                    DateTime.TryParse(ticket[3] + " " + ticket[4], out DateTime GoDT);



                    ticketList.Add(new SHSecurityModels.sys_ticketres()
                    {
                        PassageName = ticket[7] ?? "",//ticket.PASSENGERNAME,
                        PassageID = ticket[9] ?? "", // ticket.PASSENGERCERTNO,
                        PassageType = res[2] ?? "",//res.RYLX,
                        PassageState = res[7] ?? "", //res.STATE,
                        GoDate = ticket[3] ?? "",//ticket.FLIGHTDATE,
                        GoTime = ticket[4] ?? "",//ticket.FLIGHTTIME,
                        GoLocation = ticket[5] ?? "",//ticket.FROMSTATIONNAME,
                        ToLocation = ticket[6] ?? "",//ticket.TOREGIONNAME,
                        SeatNo = ticket[11] ?? "",//ticket.SEATNO,
                        TicketDate = ticket[12] ?? "",//ticket.CREATEDATE,
                        TicketTime = ticket[13] ?? "",//ticket.CREATETIME,
                        CheckTime = res[5] ?? "",//res.UPDATETIME
                        CheckInName = ticket[17] ?? "",
                        TicketYear = TicketDT.Year.ToString(),
                        TicketMonth = TicketDT.Month.ToString("00"),
                        TicketDay = TicketDT.Day.ToString("00"),
                        GoDateYear = GoDT.Year.ToString(),
                        GoDateMonth = GoDT.Month.ToString("00"),
                        GoDateDay = GoDT.Day.ToString("00")
                    });


                    //Logmng.Logger.Trace("InitTicketResultData at 222:  IN FOR AT : " + i);
                }


                //Logmng.Logger.Trace("InitTicketResultData A555555");

                systicketRepo.AddRange(ticketList);

                //Logmng.Logger.Trace("InitTicketResultData A666666");

                //保存
                LastResultUpdateTime = queryMaxUpdateTime;
                Logmng.Logger.Trace("InitTicketResultData A777777" + queryMaxUpdateTime);

                var conf = configRepo.Find(p => p.key == (int)SHSecurityModels.EConfigKey.kLastResultUpdateTime);
                if (conf != null)
                {
                    conf.value = queryMaxUpdateTime;
                    configRepo.Update(conf);
                }


                Logmng.Logger.Trace("InitTicketResultData A88888");
                return;
            }
            catch (Exception e)
            {
                Logmng.Logger.Error("InitTicketResultData ERROR: " + e.Message.ToString());
            }
           
        }



        //static void RunTimer (int waitTime, Task act, Action log = null)
        //{
        //    ThreadPool.QueueUserWorkItem(async (a) =>
        //    {
        //        while (true)
        //        {
        //            await act;

        //            if (log != null)
        //            {
        //                log();
        //            }
        //            Thread.Sleep(waitTime);
        //        }
        //    });
        //}

        //static void RunTimer2(int waitTime, Task<List<sys_ticketres>> act, Action<List<sys_ticketres>> cb = null)
        //{
        //    ThreadPool.QueueUserWorkItem(async (a) =>
        //    {
        //        while (true)
        //        {
        //            List<sys_ticketres>  list = await act;

        //            if (cb != null)
        //            {
        //                cb(list);
        //            }
        //            Thread.Sleep(waitTime);
        //        }
        //    });
        //}


    }
}
