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
    interface IAdditionalWorkService : IBaseService<AdditionalWork>
    {
        ObservableCollection<AdditionalWork> Find(Func<AdditionalWork, bool> predicate);
		void RescheduleRecord(AdditionalWork additionalWork, MaintenanceRecord record);
	}
}
