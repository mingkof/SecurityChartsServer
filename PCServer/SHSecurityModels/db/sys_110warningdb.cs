using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SHSecurityModels
{
    public class sys_110warningdb
    {
        [Key]
        public string JJD_ID { get; set; }

        public string AF_ADDR { get; set; }


        public string YEAR { get; set; }

        public string MONTH { get; set; }

        public string DAY { get; set; }

        public string HH { get; set; }

        public string MM { get; set; }

        public string SS { get; set; }


        public string BJAY1 { get; set; }

        public string BJAY2 { get; set; }

        public string BJAY3 { get; set; }

        public string BJAY4 { get; set; }

        public string BJAY5 { get; set; }

        public string FKAY1 { get; set; }

        public string FKAY2 { get; set; }

        public string FKAY3 { get; set; }

        public string FKAY4 { get; set; }

        public string FKAY5 { get; set; }

        public string QY { get; set; }

        public string KYE_AREAS { get; set; }

        public string ROAD { get; set; }

        public string JJY_NAME { get; set; }

        public string JJY_ID { get; set; }

        public string CJDW { get; set; }

        public string CJY_NAME { get; set; }

        public string CJY_ID { get; set; }

        public string COMMET { get; set; }

        public string AMAP_GPS_X { get; set; }

        public string AMAP_GPS_Y { get; set; }

        public string BJ_PHONE { get; set; }

        [Column(TypeName = "bigint")]
        public int TIMESIGN { get; set; }

        //[Column(TypeName = "bigint")]
        //public int STEP1 { get; set; }

        //[Column(TypeName = "bigint")]
        //public int STEP2 { get; set; }

        //[Column(TypeName = "bigint")]
        //public int STEP3 { get; set; }

        ////预留
        //public string BAK1 { get; set; }
        //public string BAK2 { get; set; }
        //public string BAK3 { get; set; }
    }
}
