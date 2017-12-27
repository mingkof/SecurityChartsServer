using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MKServerWeb.Model.RealData
{
    public class TrafficRoadState
    {
        public string RoadName { get; set; }

        public string TrafficAvgSpeed { get; set; }

        public string TrafficData { get; set; }

        public TrafficRoadState(string roadName, string trafficAvgSpeed, string trafficData)
        {
            RoadName = roadName;
            TrafficAvgSpeed = trafficAvgSpeed;
            TrafficData = trafficData;
        }
    }
}
