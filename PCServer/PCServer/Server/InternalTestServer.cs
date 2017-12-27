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

    public class ticketres
    {
        public string PassageName { get; set; }
        public string PassageID { get; set; }
        public string PassageType { get; set; }
        public string PassageState { get; set; }
        public string GoDate { get; set; }
        public string GoTime { get; set; }
        public string GoLocation { get; set; }
        public string ToLocation { get; set; }
        public string SeatNo { get; set; }
        public string TicketDate { get; set; }
        public string TicketTime { get; set; }
        public string CheckTime { get; set; }
    }

    public class InternalTestServer
    {
        static int length = 1;
        static List<SHSecurityModels.sys_ticketres> ticketList = new List<SHSecurityModels.sys_ticketres>();
        static List<SHSecurityModels.sys_110warningdb> warnnigList = new List<SHSecurityModels.sys_110warningdb>();


        static List<SHSecurityModels.sys_ticketres> ticketList1 = new List<SHSecurityModels.sys_ticketres>();
        static List<SHSecurityModels.sys_110warningdb> warnnigList1 = new List<SHSecurityModels.sys_110warningdb>();


        static Random random;
        public static void Add110WarnningData(ISys110WarningRepository sys110WarningRepo)
        {
            random = new Random();

            int RandomIndex = 0;
            int RandomLength = 2; // 1 - 4

            var queryAll = sys110WarningRepo.FindList(p => p.AMAP_GPS_X != null && p.AMAP_GPS_X != "","",false);
            var count = queryAll.Count();


            RandomIndex = random.Next(1, count > 3 ? count - 3 : count);
            RandomLength = random.Next(1, count > 3 ? 3 : count);


            var query = queryAll.Skip(count - RandomIndex + 1).Take(RandomLength);

            //var allWarnning = sys110WarningRepo.FindList(p => true, "", false);
            //int count = allWarnning.Count();

            warnnigList.Clear();
            warnnigList1.Clear();
            var list = query.ToList();
            for (int i = 0; i < list.Count(); i++)
            {
                string date = DateTime.Now.ToString("yyyyMMdd");
                string hour = DateTime.Now.ToString("hhmmssfff");
                string end = random.Next(0, 10).ToString() + random.Next(0, 10).ToString() + random.Next(0, 10).ToString();

                var warnning = list[i];

                SHSecurityModels.sys_110warningdb warn = new SHSecurityModels.sys_110warningdb()
                {
                    JJD_ID = date + hour + end,
                    AF_ADDR = warnning.AF_ADDR,
                    YEAR = DateTime.Now.ToString("yyyy"),
                    MONTH = DateTime.Now.ToString("MM"),
                    DAY = DateTime.Now.ToString("dd"),
                    HH = DateTime.Now.ToString("hh"),
                    MM = DateTime.Now.ToString("mm"),
                    SS = DateTime.Now.ToString("ss"),
                    BJAY1 = warnning.BJAY1,
                    BJAY2 = warnning.BJAY2,
                    BJAY3 = warnning.BJAY3,
                    BJAY4 = warnning.BJAY4,
                    BJAY5 = warnning.BJAY5,
                    FKAY1 = warnning.FKAY1,
                    FKAY2 = warnning.FKAY2,
                    FKAY3 = warnning.FKAY3,
                    FKAY4 = warnning.FKAY4,
                    FKAY5 = warnning.FKAY5,
                    QY = warnning.QY,
                    KYE_AREAS = warnning.KYE_AREAS,
                    ROAD = warnning.ROAD,
                    JJY_NAME = warnning.JJY_NAME,
                    JJY_ID = warnning.JJY_ID,
                    CJDW = warnning.CJDW,
                    CJY_NAME = warnning.CJY_NAME,
                    CJY_ID = warnning.CJY_ID,
                    COMMET = warnning.COMMET,
                    AMAP_GPS_X = warnning.AMAP_GPS_X,
                    AMAP_GPS_Y = warnning.AMAP_GPS_Y,
                    BJ_PHONE = warnning.BJ_PHONE,
                    TIMESIGN = warnning.TIMESIGN,
                };


                warnning.JJD_ID = date + hour + end;

                warnnigList.Add(warn);

                warnnigList1.Add(warn);

            }

            int time = 1 * 3 * 1000 * 10;

            ThreadPool.QueueUserWorkItem(async (a) =>
            {
                using (var serviceScope = ServiceLocator.Instance.CreateScope())
                {
                    ISys110WarningRepository wi = serviceScope.ServiceProvider.GetService<ISys110WarningRepository>();
                    while (true)
                    {
                        await UpdateWannningResult(wi, warnnigList);
                        Thread.Sleep(time);

                        warnnigList.Clear();

                        for (int i = 0; i < warnnigList1.Count; i++)
                        {
                            var warnning = warnnigList1[i];

                            string date = DateTime.Now.ToString("yyyyMMdd");

                            string hour = DateTime.Now.ToString("hhmmssfff");

                            string end = random.Next(0, 10).ToString() + random.Next(0, 10).ToString() + random.Next(0, 10).ToString();

                            SHSecurityModels.sys_110warningdb warn = new SHSecurityModels.sys_110warningdb()
                            {
                                JJD_ID = date + hour + end,
                                AF_ADDR = warnning.AF_ADDR,
                                YEAR = DateTime.Now.ToString("yyyy"),
                                MONTH = DateTime.Now.ToString("MM"),
                                DAY = DateTime.Now.ToString("dd"),
                                HH = DateTime.Now.ToString("hh"),
                                MM = DateTime.Now.ToString("mm"),
                                SS = DateTime.Now.ToString("ss"),
                                BJAY1 = warnning.BJAY1,
                                BJAY2 = warnning.BJAY2,
                                BJAY3 = warnning.BJAY3,
                                BJAY4 = warnning.BJAY4,
                                BJAY5 = warnning.BJAY5,
                                FKAY1 = warnning.FKAY1,
                                FKAY2 = warnning.FKAY2,
                                FKAY3 = warnning.FKAY3,
                                FKAY4 = warnning.FKAY4,
                                FKAY5 = warnning.FKAY5,
                                QY = warnning.QY,
                                KYE_AREAS = warnning.KYE_AREAS,
                                ROAD = warnning.ROAD,
                                JJY_NAME = warnning.JJY_NAME,
                                JJY_ID = warnning.JJY_ID,
                                CJDW = warnning.CJDW,
                                CJY_NAME = warnning.CJY_NAME,
                                CJY_ID = warnning.CJY_ID,
                                COMMET = warnning.COMMET,
                                AMAP_GPS_X = warnning.AMAP_GPS_X,
                                AMAP_GPS_Y = warnning.AMAP_GPS_Y,
                                BJ_PHONE = warnning.BJ_PHONE,
                                TIMESIGN = warnning.TIMESIGN,
                            };
                            warnnigList.Add(warn);
                        }
                    }
                }
            });
        }

        static async Task UpdateWannningResult(ISys110WarningRepository sys110WarningRepo, List<SHSecurityModels.sys_110warningdb> List)
        {
            sys110WarningRepo.AddRange(List);
        }

        public static void AddTicketresData(ISysTicketresRepository systicketRepo)
        {
            var allticket = systicketRepo.FindList(p => true, "", false);
            int count = allticket.Count();

            ticketList.Clear();
            for (int i = 1; i <= length; i++)
            {
                var ticket = systicketRepo.Find(p => p.ID == i);

                ticket.GoDate = DateTime.Now.ToString("yyyy-MM-dd");

                ticket.TicketDate = ticket.GoDate + " " + ticket.TicketDate.Split(' ')[1];

                ticketList.Add(new SHSecurityModels.sys_ticketres()
                {
                    PassageName = ticket.PassageName,
                    PassageID = ticket.PassageID,
                    PassageType = ticket.PassageType,
                    PassageState = ticket.PassageState,
                    GoDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    GoTime = ticket.GoTime,
                    GoLocation = ticket.GoLocation,
                    ToLocation = ticket.ToLocation,
                    SeatNo = ticket.SeatNo,
                    TicketDate = ticket.TicketDate,
                    TicketTime = DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("hhmmssfff"),

                    CheckTime = DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("hhmmssfff"),
                });
                ticketList1.Add(ticket);
            }

            int time = 1 * 3 * 1000 * 10;
            ThreadPool.QueueUserWorkItem(async (a) =>
            {
                using (var serviceScope = ServiceLocator.Instance.CreateScope())
                {
                    ISysTicketresRepository ti = serviceScope.ServiceProvider.GetService<ISysTicketresRepository>();
                    while (true)
                    {
                        await UpdateTicketResult(ti, ticketList);
                        Thread.Sleep(time);
                        ticketList.Clear();
                        for (int i = 0; i < ticketList1.Count; i++)
                        {
                            ticketList.Add(new SHSecurityModels.sys_ticketres()
                            {
                                PassageName = ticketList1[i].PassageName,
                                PassageID = ticketList1[i].PassageID,
                                PassageType = ticketList1[i].PassageType,
                                PassageState = ticketList1[i].PassageState,
                                GoDate = DateTime.Now.ToString("yyyy-MM-dd"),

                                GoTime = ticketList1[i].GoTime,
                                GoLocation = ticketList1[i].GoLocation,
                                ToLocation = ticketList1[i].ToLocation,
                                SeatNo = ticketList1[i].SeatNo,
                                TicketDate = ticketList1[i].TicketDate,
                                TicketTime = DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("hhmmssfff"),

                                CheckTime = DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("hhmmssfff"),
                            });
                        }
                        //var allticket = ti.FindList(p => true, "", false);
                        //int count = allticket.Count();
                        //ticketList.Clear();
                        //for (int i = 1; i <= length; i++)
                        //{
                        //    var ticket = ti.Find(p => p.ID == i);
                        //    ticket.ID = count + i;
                        //    ticket.GoDate = DateTime.Now.ToString("yyyy-MM-dd");
                        //    ticket.TicketDate = ticket.GoDate + " " + ticket.TicketDate.Split(' ')[1];
                        //    ticketList.Add(ticket);
                        //}
                        //ticketres t = new ticketres()
                        //{
                        //    PassageName = ticket.PassageName,
                        //    PassageID = ticket.PassageID,
                        //    PassageType = ticket.PassageType,
                        //    PassageState = ticket.PassageState,
                        //    GoDate = ticket.GoDate,
                        //    GoTime = ticket.GoTime,
                        //    GoLocation = ticket.GoLocation,
                        //    ToLocation = ticket.ToLocation,
                        //    SeatNo = ticket.SeatNo,
                        //    TicketDate = ticket.TicketDate,
                        //    TicketTime = ticket.TicketTime,
                        //    CheckTime = ticket.CheckTime
                        //};
                    }
                }
            });
        }

        static async Task UpdateTicketResult(ISysTicketresRepository systicketRepo, List<SHSecurityModels.sys_ticketres> List)
        {
            systicketRepo.AddRange(List);
            systicketRepo.SaveChanges();
        }



        public static void ClearData()
        {
            int time = 1 * 180 * 1000 * 60;
            ThreadPool.QueueUserWorkItem(async (a) =>
            {
                using (var serviceScope = ServiceLocator.Instance.CreateScope())
                {
                    ISys110WarningRepository wi = serviceScope.ServiceProvider.GetService<ISys110WarningRepository>();
                    ISysTicketresRepository ti = serviceScope.ServiceProvider.GetService<ISysTicketresRepository>();
                    while (true)
                    {
                        await ClearDBData(wi, ti);
                        Thread.Sleep(time);
                    }
                }
            });
        }

        static async Task ClearDBData(ISys110WarningRepository sys110WarningRepo, ISysTicketresRepository systicketRepo)
        {
            var ticketList = systicketRepo.FindList(p => p.ID > 400, "", false);
            systicketRepo.RemoveRange(ticketList);


            
            var warnningList = sys110WarningRepo.FindList(p => int.Parse(p.MM) % 2 == 0, "", false);
            sys110WarningRepo.RemoveRange(warnningList);
        }
    }
}
