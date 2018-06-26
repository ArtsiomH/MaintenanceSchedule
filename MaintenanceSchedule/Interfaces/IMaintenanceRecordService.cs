using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaintenanceSchedule.Model;
using MaintenanceScheduleDataLayer.Entities;
using System.Collections.ObjectModel;

namespace MaintenanceSchedule.Interfaces
{
    interface IMaintenanceRecordService : IBaseService<MaintenanceRecord>
    {
        ObservableCollection<MaintenanceRecord> Find(Func<MaintenanceRecord, bool> predicate);
    }
}
