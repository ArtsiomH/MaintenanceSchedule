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
		public int PlannedDay { get; set; }
		public string PlannedMonth { get; set; }
		public int ActualDay { get; set; }
		public string ActualMonth { get; set; }
        public DateTime? ActualMaintenanceDate { get; set; }
        public MaintenanceType ActualMaintenanceType { get; set; }
		public bool IsPlanned { get; set; }	
		public int MaintenanceRecordId { get; set; }	
        public List<string> MaintenanceTypes { get; set; }
    }
}
