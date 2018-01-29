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
}
