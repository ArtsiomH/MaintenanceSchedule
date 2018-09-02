using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaintenanceSchedule.Model;
using MaintenanceScheduleDataLayer.Entities;

namespace MaintenanceSchedule.Interfaces
{
    interface IAdditionalDeviceService : IBaseService<AdditionalDevice>
    {
		void RescheduleRecord(AdditionalDevice additionalDevice, MaintenanceRecord record);
	}
}
