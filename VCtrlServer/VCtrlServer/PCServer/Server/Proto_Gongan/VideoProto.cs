using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCServer.Server.Proto_Gongan
{
    public class VidioProtoCS
    {
        public string type { get; set; }
        public string value { get; set; }
        public string autho { get; set; } 
    }

    public class VidioProtoSC
    {
        public string type { get; set; }
        public string value { get; set; }
        public string autho { get; set; }
    }

    public class SipProtoSC
    {
        public string type { get; set; }
        public string value { get; set; }
        public string sdp { get; set; }
        public string port { get; set; }
        public string ip { get; set; }
    }

}