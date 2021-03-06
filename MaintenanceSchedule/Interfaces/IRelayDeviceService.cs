﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaintenanceSchedule.Model;
using MaintenanceScheduleDataLayer.Entities;

namespace MaintenanceSchedule.Interfaces
{
    interface IRelayDeviceService : IBaseService<RelayDevice>
    {
		void RescheduleRecord(RelayDevice relayDevice, MaintenanceRecord record);
	}
}
