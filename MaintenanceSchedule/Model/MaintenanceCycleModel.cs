using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceSchedule.Model
{
    class MaintenanceCycleModel
    {
        public int MaintenanceCycleId { get; set; }
        public string Name { get; set; }
        public string ShowName { get; set; }
        public string [] MaintenanceTypes { get; set; }

        public MaintenanceCycleModel()
        {
            MaintenanceTypes = new string[9];
        }
    }
}
