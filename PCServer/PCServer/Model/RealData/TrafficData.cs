using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MKServerWeb.Model.RealData
{
    public class TrafficData
    {
        public string TrafficDataForAll { get; set; }

        public string TrafficAvgSpeed { get; set; }

        public TrafficRoadState[] TopsRoads;
    }
}
