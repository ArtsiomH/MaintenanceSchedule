using System.Collections.ObjectModel;
using MaintenanceSchedule.Interfaces;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Entities;
using System.Linq;

namespace MaintenanceSchedule.Services
{
    class AdditionalDeviceService : BaseDeviceService, IAdditionalDeviceService
    {
        public AdditionalDeviceService(IUnitOfWork dataBase) : base(dataBase)
        {    }

        public void Create(AdditionalDevice t)
        {
            CreateRecords(t);
            dataBase.AdditionalDevices.Create(t);
            dataBase.Save();
        }

        public void Delete(AdditionalDevice t)
        {
            dataBase.AdditionalDevices.Delete(t);
        }

        public AdditionalDevice Get(int id)
        {
            return dataBase.AdditionalDevices.Read(id);
        }

        public ObservableCollection<AdditionalDevice> GetAll()
        {
            return new ObservableCollection<AdditionalDevice>(dataBase.AdditionalDevices.GetAll());
        }

		public void RescheduleRecord(AdditionalDevice additionalDevice, MaintenanceRecord record)
		{
			record.IsRescheduled = true;
			MaintenanceRecord addedRecord = new MaintenanceRecord();
			MaintenanceRecord nextRecord = additionalDevice.MaintenanceRecords.OrderBy(x => x.PlannedMaintenanceDate)
				.FirstOrDefault(x => x.PlannedMaintenanceDate > record.PlannedMaintenanceDate);

			if (nextRecord != null && nextRecord.PlannedMaintenanceDate.Year != record.PlannedMaintenanceDate.Year + 1)
			{
				addedRecord.PlannedMaintenanceDate = record.PlannedMaintenanceDate.AddYears(1);
				addedRecord.MaintainedEquipment = additionalDevice;
				addedRecord.PlannedMaintenanceType = record.PlannedMaintenanceType;
				dataBase.MaintenanceRecords.Create(addedRecord);
			}
			else if (nextRecord != null && nextRecord.PlannedMaintenanceType.Name.Contains("В"))
			{
				return;
			}
			else if (record.PlannedMaintenanceType.Name.Contains("В"))
			{
				nextRecord.PlannedMaintenanceType = GetCurrentCycle(additionalDevice).MaintenanceYears.First(x => x.MaintenanceType.Name.Contains("В")).MaintenanceType;
				dataBase.MaintenanceRecords.Update(nextRecord);
			}
			else if (record.PlannedMaintenanceType.Name.Contains("К") && !nextRecord.PlannedMaintenanceType.Name.Contains("К"))
			{
				nextRecord.PlannedMaintenanceType = GetCurrentCycle(additionalDevice).MaintenanceYears.First(x => x.MaintenanceType.Name.Contains("К")).MaintenanceType;
				dataBase.MaintenanceRecords.Update(nextRecord);
			}
			else return;
			dataBase.Save();
		}

		public void Update(AdditionalDevice t)
        {
            UpdateRecords(t);
            dataBase.AdditionalDevices.Update(t);
            dataBase.Save();
        }
    }
}
