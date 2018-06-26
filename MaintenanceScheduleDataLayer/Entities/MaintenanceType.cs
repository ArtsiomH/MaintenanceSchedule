using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceScheduleDataLayer.Entities
{

    public class MaintenanceType
    {
        public int MaintenanceTypeId { get; set; }

        [MaxLength(50)]
        [Index(IsUnique = true), Required]
        public string Name { get; set; }

        public List<MaintenanceYear> MaintenanceYears { get; set; }

        public List<MaintenanceRecord> PlannedMaintenanceRecords { get; set; }

        public List<MaintenanceRecord> ActualMaintenanceRecords { get; set; }
    }
}
