using MaintenanceScheduleDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceSchedule.Model
{
    class MaintainedEquipmentModel
    {
        public string Name { get; set; }
        public int MaintainedEquipmentId { get; set; }
        public int? InputYear { get; set; }
        public DateTime LastMaintenanceDate { get; set; }
        public MaintenanceType LastMaintenanceType { get; set; }
        public DateTime ActualMaintenanceDate { get; set; }
        public MaintenanceType ActualMaintenanceType { get; set; }
    }
}
