//卡口模型数据以及ftp中的KaKouData.json的反解析结构体

//mysql模型
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SHSecurityModels
{
    public class traviodata {
        public int Id { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
        public int TimeStamp { get; set; }
        public string TodayCount { get; set; }
    }

    public class TraVioInfoStruct
    {
        public FirstTravioByLocationNew travioByLocationNew =new FirstTravioByLocationNew(); //最新抓拍的同地方的数量
        public StataTravioByTodayEachHour TravioByTodayEachHour =new StataTravioByTodayEachHour(); //当天每个小时抓拍的数量
        public StataTraVioByNearOneMonthNewAll TravioByNearOneMonth =new StataTraVioByNearOneMonthNewAll();  //最近一周的抓拍总数量（用于折线图数据）
        public TodayNearWeekCount NearWeekCount =new TodayNearWeekCount(); // 当天数量与这周的数量
        public int MaxNearWeekCount { set; get; }  //一周最大抓拍数量
        public int MaxTodayCount { set; get; }  //当天最大抓拍数量
    }

    public class FirstTravioByLocationNew
    {
        public List<LocationNameInfo> statList = new List<LocationNameInfo>();
        public int totalCount {get;set;}
    }
    public class LocationNameInfo
    {
        public string name { set; get; }
        public int totalCount { set; get; }
        public string color { set; get; }
        public bool overLimit { set; get; }
        public int noDiscardTotalCount { set; get; }
        public int unauditCount { set; get; }
        public int auditCount { set; get; }
        public int lightVioCount { set; get; }
        public int discardCount { set; get; }
    }

    public class StataTravioByTodayEachHour
    {
        public List<LocationTimeInfo> list = new List<LocationTimeInfo>();
    }

    public class LocationTimeInfo
    {
        public string time { set; get; }
        public string timeText { set; get; }
        public string noDiscardTotalCount { set; get; }
        public string unauditCount { set; get; }
        public string auditCount { set; get; }
        public string lightVioCount { set; get; }
        public string discardCount { set; get; }
        public string totalCount { set; get; }
    }

    public class StataTraVioByNearOneMonthNewAll
    {
        public List<LocationTimeInfo> list = new List<LocationTimeInfo>();
    }

    public class TodayNearWeekCount
    {
        public string todayCount { set; get; }
        public string nearWeekCount { set; get; }
    }


}
