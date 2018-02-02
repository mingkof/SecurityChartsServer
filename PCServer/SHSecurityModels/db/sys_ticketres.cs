using System;
using System.Collections.Generic;
using System.Text;

namespace SHSecurityModels
{
   public class sys_ticketres
    {
        public long ID { get; set; }
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
        public string CheckInName { get; set; }

    }
}
