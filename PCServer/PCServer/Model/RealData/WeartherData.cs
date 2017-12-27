using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MKServerWeb.Model.RealData
{
    public class WeartherData
    {
        public string AirQuality { get; set; }

        public int BigTemperature { get; set; }

        public int Humidity { get; set; }

        public int SmallTemperature { get; set; }

        public string Weather { get; set; }

        public string Wind { get; set; }
    }
}
