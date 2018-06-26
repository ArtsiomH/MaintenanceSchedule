using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceScheduleDataLayer.Entities
{
    [Table("MaintainedEquipmentsByCycle")]
    public class MaintainedEquipmentByCycle : MaintainedEquipment
    {     
        public int? NormalMaintenanceCycleId { get; set; }
        [InverseProperty("NormalEquipments")]
        [ForeignKey("NormalMaintenanceCycleId")]
        public MaintenanceCycle NormalMaintenanceCycle { get; set; }

        public int? ReducedMaintenanceCycleId { get; set; }
        [InverseProperty("ReducedEquipments")]
        [ForeignKey("ReducedMaintenanceCycleId")]
        public MaintenanceCycle ReducedMaintenanceCycle { get; set; }
    }
}
