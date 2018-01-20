using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SHSecurityModels
{
   public class sys_sipport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string pushToPort { get; set; }
        public string pushToIp { get; set; }
        public int sipSession { get; set; }

        public string CameraId { get; set; }
    }
}
