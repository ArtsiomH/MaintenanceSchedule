using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceScheduleDataLayer.Entities
{
    public class MaintenanceCycle
    {
        public int MaintenanceCycleId { get; set; }
       
        [MaxLength(255), Index(IsUnique = true), Required]
        public string Name { get; set; }
        [MaxLength(100)]
        public string ShowName { get; set; }              

        public List<MaintainedEquipmentByCycle> NormalEquipments { get; set; }
        public List<MaintainedEquipmentByCycle> ReducedEquipments { get; set; }
        public List<MaintenanceYear> MaintenanceYears { get; set; }        
    }
}
