using MaintenanceScheduleDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceSchedule.Model
{
    class ScheduleRecordModel : MaintainedEquipmentModel
    {
        public string Substation { get; set; }
        public string Attachment { get; set; }
        public DateTime? ActualMaintenanceDate { get; set; }
        public MaintenanceType ActualMaintenanceType { get; set; }
        public List<string> MaintenanceTypes { get; set; }
    }
}
