using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceScheduleDataLayer.Entities
{
    public class MaintainedEquipment
    {
        public int MaintainedEquipmentId { get; set; }

        public int? InputYear { get; set; }

        public List<MaintenanceRecord> MaintenanceRecords { get; set; }
    }
}
