using MaintenanceScheduleDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceSchedule.Interfaces
{
    interface IMaintainedEquipmentByCycleService
    {
        void MarkActualRecord(MaintainedEquipmentByCycle equpment, DateTime date, MaintenanceType type);
        MaintenanceCycle GetCurrentCycle(MaintainedEquipmentByCycle equpment);
	}
}
