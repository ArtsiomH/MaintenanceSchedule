using System.Collections.ObjectModel;
using MaintenanceSchedule.Interfaces;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Entities;
using System.Linq;
using System;

namespace MaintenanceSchedule.Services
{
    class RelayDeviceService : BaseDeviceService, IRelayDeviceService
    {
        public RelayDeviceService(IUnitOfWork dataBase) : base(dataBase)
        {   }

        public void Create(RelayDevice t)
        {
            CreateRecords(t);           
            dataBase.RelayDevices.Create(t);
            dataBase.Save();
        }

        public void Delete(RelayDevice t)
        {
            dataBase.Devices.Delete(t);
            dataBase.Save();
        }

        public RelayDevice Get(int id)
        {
            return dataBase.RelayDevices.Read(id);
        }

        public ObservableCollection<RelayDevice> GetAll()
        {
            return new ObservableCollection<RelayDevice>(dataBase.RelayDevices.GetAll());
        }

		public void RescheduleRecord(RelayDevice relayDevice, MaintenanceRecord record)
		{
			record.IsRescheduled = true;
			MaintenanceRecord addedRecord = new MaintenanceRecord();
			MaintenanceRecord nextRecord = relayDevice.MaintenanceRecords.OrderBy(x => x.PlannedMaintenanceDate)
				.FirstOrDefault(x => x.PlannedMaintenanceDate > record.PlannedMaintenanceDate);

			if (nextRecord != null && nextRecord.PlannedMaintenanceDate.Year != record.PlannedMaintenanceDate.Year + 1)
			{
				addedRecord.PlannedMaintenanceDate = record.PlannedMaintenanceDate.AddYears(1);
				addedRecord.MaintainedEquipment = relayDevice;
				addedRecord.PlannedMaintenanceType = record.PlannedMaintenanceType;
				dataBase.MaintenanceRecords.Create(addedRecord);
			}
			else if (nextRecord != null && nextRecord.PlannedMaintenanceType.Name.Contains("В"))
			{
				return;
			}
			else if (record.PlannedMaintenanceType.Name.Contains("В"))
			{
				nextRecord.PlannedMaintenanceType = GetCurrentCycle(relayDevice).MaintenanceYears.First(x => x.MaintenanceType.Name.Contains("В")).MaintenanceType;
				dataBase.MaintenanceRecords.Update(nextRecord);
			}
			else if (record.PlannedMaintenanceType.Name.Contains("К") && !nextRecord.PlannedMaintenanceType.Name.Contains("К"))
			{
				nextRecord.PlannedMaintenanceType = GetCurrentCycle(relayDevice).MaintenanceYears.First(x => x.MaintenanceType.Name.Contains("К")).MaintenanceType;
				dataBase.MaintenanceRecords.Update(nextRecord);
			}
			else return;
			dataBase.Save();			
		}

		public void Update(RelayDevice t)
        {
            UpdateRecords(t);
            dataBase.RelayDevices.Update(t);
            dataBase.Save();
        }        
    }
}