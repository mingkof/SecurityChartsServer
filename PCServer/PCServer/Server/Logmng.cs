using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MKServerWeb.Server
{
    public class Logmng
    {
        //private static ILogger<Logmng> _logger;
        //public Logmng(ILogger<Logmng> logger)
        //{
        //    _logger = logger;
        //}

        public static Logger Logger 
        {
            get {
                return LogManager.GetLogger("Example");
            }
        }

    }
}
