using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceScheduleDataLayer.Entities
{
    [Table("Devices")]
    public class Device : MaintainedEquipmentByCycle
    {        
        public int? ExpiryYear { get; set; }
        public int MaintenancePeriod { get; set; }
        public DateTime? LastRecovery { get; set; }

        public int? ActId { get; set; }
        public Act Act { get; set; }
    }
}
