using MaintenanceScheduleDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceSchedule.Model
{
    class AdditionalDeviceModel : MaintainedEquipmentModel
    {                   
        public int MaintenancePeriod { get; set; }
        public DateTime? LastRecovery { get; set; }
    }
}
