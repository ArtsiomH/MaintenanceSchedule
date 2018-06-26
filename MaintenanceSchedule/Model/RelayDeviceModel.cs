using MaintenanceScheduleDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceSchedule.Model
{
    class RelayDeviceModel : MaintainedEquipmentModel
    {
        public int MaintenancePeriod { get; set; }
        public DateTime? LastRecovery { get; set; }       
        public ElementBase ElementBase { get; set; }
    }
}
