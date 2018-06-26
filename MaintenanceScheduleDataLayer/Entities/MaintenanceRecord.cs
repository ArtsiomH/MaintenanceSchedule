using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceScheduleDataLayer.Entities
{
    public class MaintenanceRecord
    {
        public int MaintenanceRecordId { get; set; }

        public DateTime PlannedMaintenanceDate { get; set; }
        public DateTime? ActualMaintenanceDate { get; set; }
        public string Description { get; set; }
        public bool IsPlanned { get; set; }
        public bool IsRescheduled { get; set; }

        public int? PlannedMaintenanceTypeId { get; set; }
        [InverseProperty("PlannedMaintenanceRecords")]
        [ForeignKey("PlannedMaintenanceTypeId")]
        public MaintenanceType PlannedMaintenanceType { get; set; }

        public int? ActualMaintenanceTypeId { get; set; }
        [InverseProperty("ActualMaintenanceRecords")]
        [ForeignKey("ActualMaintenanceTypeId")]
        public MaintenanceType ActualMaintenanceType { get; set; }

        public int MaintainedEquipmentId { get; set; }
        public MaintainedEquipment MaintainedEquipment { get; set; } 
    }
}
