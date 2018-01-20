using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SHSecurityModels
{
   public class sys_wifitable
    {
        public int ID { get; set; }

        
        public string MAC { get; set; }
        
        public string BRAND { get; set; }
        
        public string CACHE_SSID { get; set; }
        
        public string CAPTURE_TIME { get; set; }
       
        public int TERMINAL_FIELD_STRENGTH { get; set; }
       
        public int IDENTIFICATOIN_TYPE { get; set; }
       
        public int CERTIFICATE_CODE { get; set; }

       
        public string SSID_POSITION { get; set; }
       
        public string ACCESS_AP_MAC { get; set; }

       
        public int ACCESS_AP_CHANNEL { get; set; }
       
        public int ACCESS_AP_ENCRYPTION_TYPE { get; set; }

       
        public string X_COORDINATE { get; set; }
       
        public string Y_COORDINATE { get; set; }
       
        public string NETBAR_WACODE { get; set; }
       
        public string COLLECTION_EQUIPMENT_ID { get; set; }
       
        public string COLLECTION_EQUIPMENT_LONGITUDE { get; set; }
       
        public string COLLECTION_EQUIPMENT_LATITUDE { get; set; }

    }
}
