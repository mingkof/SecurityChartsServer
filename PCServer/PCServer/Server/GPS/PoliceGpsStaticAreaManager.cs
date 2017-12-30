using KVDDDCore.Utils;
using SHSecurityContext.IRepositorys;
using SHSecurityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCServer.Server.GPS
{
    public class PoliceGpsStaticAreaConfig_xy
    {
        public long x { get; set; }
        public long y { get; set; }
    }

    public class PoliceGpsStaticAreaConfig_item
    {
        public string name { get; set; }
        public PoliceGpsStaticAreaConfig_xy a1 { get; set; }
        public PoliceGpsStaticAreaConfig_xy a2 { get; set; }
        public PoliceGpsStaticAreaConfig_xy a3 { get; set; }
        public PoliceGpsStaticAreaConfig_xy a4 { get; set; }
    }

  public  class PoliceGpsStaticAreaConfig
    {
        public List<PoliceGpsStaticAreaConfig_item> data { get; set; }
    }


    public class PoliceGpsStaticAreaManager
    {
        IPoliceGPSAreaStaticRepository _police_area_static_repo;

        public PoliceGpsStaticAreaConfig AreaConfig = new PoliceGpsStaticAreaConfig();

        public Dictionary<string, KPoint[]> Areas = new Dictionary<string, KPoint[]>();


        public void InitAreaConfig(IPoliceGPSAreaStaticRepository police_area_static_repo)
        {
            _police_area_static_repo = police_area_static_repo;

            Areas.Clear();

            for (int i = 0; i < AreaConfig.data.Count; i++)
            {
                var name = AreaConfig.data[i].name;
                
                var arr = new KPoint[4];

                arr[0] = new KPoint()
                {
                    X = AreaConfig.data[i].a1.x,
                    Y = AreaConfig.data[i].a1.y
                };
                arr[1] = new KPoint()
                {
                    X = AreaConfig.data[i].a2.x,
                    Y = AreaConfig.data[i].a2.y
                };
                arr[2] = new KPoint()
                {
                    X = AreaConfig.data[i].a3.x,
                    Y = AreaConfig.data[i].a3.y
                };
                arr[3] = new KPoint()
                {
                    X = AreaConfig.data[i].a4.x,
                    Y = AreaConfig.data[i].a4.y
                };

                if (!Areas.ContainsKey(name)) {
                    Areas.Add(name, arr);
                } else
                {
                    Areas[name] = arr;
                }
            }
        }


        public List<string> CheckInAreaByGps(string gps_x, string gps_y)
        {
            Vector3 vec = GPSUtils.ComputeLocalPositionGCJ(gps_x, gps_y);

            //int GridX = (int)(vec.x / 100);
            //int GridY = (int)(vec.y / 100);

            return CheckInArea((long)vec.x, (long)vec.y);
        }


        public List<string> CheckInArea(long x, long y)
        {
            List<string> list = new List<string>();
            if (Areas == null || Areas.Count <= 0)
                return list;

            KPoint p = new KPoint(x, y);

            foreach (var item in Areas)
            {
                bool inArea = KVDDDCore.Utils.PointInPolygon.CheckPointInPolygon(p, item.Value);

                if (inArea)
                {
                    list.Add(item.Key);
                }
            }

            return list;
        }



        public Task UpdatePoliceAreaStatic(PoliceGPS value)
        {
            return Task.Run(() =>
            {
                var find = _police_area_static_repo.FindList(p => p.PoliceId == value.PoliceID && p.Year == value.Year && p.Month == value.Month && p.Day == value.Day && p.HH == value.HH, "", false);

                //计算区域
                var areas = CheckInAreaByGps(value.GPS_X, value.GPS_Y);

                if(areas ==null || areas.Count <= 0)
                {
                    if(find != null)
                    {
                        var findQita = find.Select(p => p.AreaName == "其他");
                        if(findQita == null)
                        {
                            AddPoliceAreaStatic("其他", value);
                        }
                        else
                        {
                            //不处理
                        }

                    } else
                    {
                        AddPoliceAreaStatic("其他", value);
                        return;
                    }
                } else
                {
                    for (int i = 0; i < areas.Count; i++)
                    {
                        string areaName = areas[i];

                        if (find != null)
                        {
                            var findSelectArea = find.Select(p => p.AreaName == areaName);

                            if (findSelectArea == null)
                            {
                                AddPoliceAreaStatic(areaName, value);
                            }
                            else
                            {
                                //不处理
                            }
                        } else
                        {
                            AddPoliceAreaStatic(areaName, value);
                        }
                    }

                }

            });
        }


        void AddPoliceAreaStatic(string area, PoliceGPS value)
        {
            _police_area_static_repo.Add(new PoliceGPSAreaStatic()
            {
                AreaName = area,
                Day = value.Day,
                HH = value.HH,
                Month = value.Month,
                PoliceId = value.PoliceID,
                Year = value.Year
            });
        }


    }
}
