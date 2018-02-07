using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SHSecurityModels
{
    public class HongWaiPeopleData
    {
        [Key]
        public int key { get; set; }
        public string sn { get; set; }
        public string type { get; set; }
        public string count { get; set; }
        public int timeStamp { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
    }

    public class JsonHongWaiStruct
    {
        public string sn;
        public string type;
        public string count;
        public int timeStamp;
        public string Year;
        public string Month;
        public string Day;
    }
}
