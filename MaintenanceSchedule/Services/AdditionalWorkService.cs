using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MaintenanceSchedule.Interfaces;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Entities;

namespace MaintenanceSchedule.Services
{
    class AdditionalWorkService : MaintainedEquipmentByCycleService, IAdditionalWorkService
    {       
        public AdditionalWorkService(IUnitOfWork dataBase) : base(dataBase)
        {  }

        public void Create(AdditionalWork t)
        {
            CreateRecords(t);
            dataBase.AdditionalWorks.Create(t);
            dataBase.Save();
        }

        public void CreateRecords(AdditionalWork t)
        {            
            MaintenanceCycle maintenanceCycle = dataBase.MaintenanceCycles.Read(t.NormalMaintenanceCycle.MaintenanceCycleId);
            int lastYear = maintenanceCycle.MaintenanceYears.Max(x => x.Year);
            int initialYear = t.InputYear.Value;            
            for (int i = initialYear, maintainedYear = 0, currentYear= 0; i <= DateTime.Now.Year + 11; i++, maintainedYear++, currentYear++)
            {
                addNewRecord(maintenanceCycle, t, maintainedYear, initialYear, currentYear);
                if (maintainedYear == lastYear) maintainedYear = 0;
            }
        }

        public void Delete(AdditionalWork t)
        {
            dataBase.AdditionalWorks.Delete(t);
            dataBase.Save();
        }

        public AdditionalWork Get(int id)
        {
            return dataBase.AdditionalWorks.Read(id);
        }

        public ObservableCollection<AdditionalWork> GetAll()
        {
            return new ObservableCollection<AdditionalWork>(dataBase.AdditionalWorks.GetAll());
        }

        public void Update(AdditionalWork t)
        {
            UpdateRecords(t);
            AdditionalWork oldCombineDevice = dataBase.AdditionalWorks.Read(t.MaintainedEquipmentId);            
            if (oldCombineDevice.Devices.Count != 0)
            {
                oldCombineDevice.Devices.Clear();
            }
            if (t.Devices.Count != 0)
            {
                oldCombineDevice.Devices.AddRange(t.Devices);
            }
            dataBase.AdditionalWorks.Update(t);
            dataBase.Save();
        }

        public void UpdateRecords(AdditionalWork t)
        {
            AdditionalWork oldCombineDevice = dataBase.AdditionalWorks.Read(t.MaintainedEquipmentId);
            MaintenanceCycle maintenanceCycle = dataBase.MaintenanceCycles.Read(t.NormalMaintenanceCycle.MaintenanceCycleId);
            updateCombineDeviceRecords(maintenanceCycle, oldCombineDevice);
            dataBase.Save();
        }

        public void UpdateRecords(MaintenanceCycle newMaintenanceCycle, AdditionalWork t)
        {
            updateCombineDeviceRecords(newMaintenanceCycle, t);
        }

        public void UpdateRecords(MaintenanceCycle deletedMaintenanceCycle, MaintenanceCycle newMaintenanceCycle, AdditionalWork t)
        {
            t.NormalMaintenanceCycle = newMaintenanceCycle;
            t.ReducedMaintenanceCycle = newMaintenanceCycle;
            updateCombineDeviceRecords(newMaintenanceCycle, t);
        }


        private void updateCombineDeviceRecords(MaintenanceCycle newMaintenanceCycle, AdditionalWork t)
        {
                      
            List<MaintenanceRecord> deleteMaintenanceRecords = dataBase.AdditionalWorks.Read(t.MaintainedEquipmentId).MaintenanceRecords.FindAll(x => x.ActualMaintenanceDate == null
                                                                                                                                                && !(x.IsPlanned || x.IsRescheduled));
            foreach (MaintenanceRecord record in deleteMaintenanceRecords)
            {
                dataBase.MaintenanceRecords.Delete(record);
            }
            List<MaintenanceRecord> maintenanceRecords = dataBase.AdditionalWorks.Read(t.MaintainedEquipmentId).MaintenanceRecords;
            int lastYear = newMaintenanceCycle.MaintenanceYears.Max(x => x.Year);
            int initialYear;
            if (t.MaintenanceRecords.Count == 0)
            {
                initialYear = t.InputYear.Value;
                for (int i = 0, maintainedYear = 0; i <= 10; i++, maintainedYear++)
                {
                    addNewRecord(newMaintenanceCycle, t, maintainedYear, initialYear, i);
                    if (maintainedYear == lastYear) maintainedYear = 0;
                }
            }
            else
            {
                MaintenanceCycle oldMaintenanceCycle = dataBase.MaintenanceCycles.Read(t.NormalMaintenanceCycle.MaintenanceCycleId);
                int nextMaintainedYear;
                initialYear = maintenanceRecords.Last(x => x.PlannedMaintenanceType.Name == oldMaintenanceCycle.MaintenanceYears[0].MaintenanceType.Name).PlannedMaintenanceDate.Year;
                nextMaintainedYear = maintenanceRecords.Last().PlannedMaintenanceDate.Year - initialYear + 1;
                for (int i = 0, maintainedYear = nextMaintainedYear; i <= 10; i++, maintainedYear++)
                {
                    if (checkYear(initialYear + nextMaintainedYear + i)) continue;
                    addNewRecord(newMaintenanceCycle, t, maintainedYear, initialYear, i + nextMaintainedYear);
                    if (maintainedYear == lastYear) maintainedYear = 0;
                }
            }
        }

		public void RescheduleRecord(AdditionalWork additionalWork, MaintenanceRecord record)
		{
			record.IsRescheduled = true;
			MaintenanceRecord addedRecord = new MaintenanceRecord();
			MaintenanceRecord nextRecord = additionalWork.MaintenanceRecords.OrderBy(x => x.PlannedMaintenanceDate)
				.FirstOrDefault(x => x.PlannedMaintenanceDate > record.PlannedMaintenanceDate);

			if (nextRecord != null && nextRecord.PlannedMaintenanceDate.Year != record.PlannedMaintenanceDate.Year + 1)
			{
				addedRecord.PlannedMaintenanceDate = record.PlannedMaintenanceDate.AddYears(1);
				addedRecord.MaintainedEquipment = additionalWork;
				addedRecord.PlannedMaintenanceType = record.PlannedMaintenanceType;
				dataBase.MaintenanceRecords.Create(addedRecord);
			}
			else if (record.PlannedMaintenanceType.Name.Contains("К") && !nextRecord.PlannedMaintenanceType.Name.Contains("К"))
			{
				nextRecord.PlannedMaintenanceType = GetCurrentCycle(additionalWork).MaintenanceYears.First(x => x.MaintenanceType.Name.Contains("К")).MaintenanceType;
				dataBase.MaintenanceRecords.Update(nextRecord);
			}
			else return;
			dataBase.Save();
		}

		public ObservableCollection<AdditionalWork> Find(Func<AdditionalWork, bool> predicate)
        {
            return new ObservableCollection<AdditionalWork>(dataBase.AdditionalWorks.GetAll().Where(predicate));
        }
    }
}
