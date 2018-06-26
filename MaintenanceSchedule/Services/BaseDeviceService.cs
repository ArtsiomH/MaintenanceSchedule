using System;
using System.Collections.Generic;
using System.Linq;
using MaintenanceScheduleDataLayer.Entities;
using MaintenanceScheduleDataLayer.Interfaces;

namespace MaintenanceSchedule.Services
{
    class BaseDeviceService : MaintainedEquipmentByCycleService
    {
        public BaseDeviceService(IUnitOfWork dataBase) : base(dataBase)
        {    }

        public void CreateRecords(Device t)
        {
            MaintenanceCycle maintenanceCycle = dataBase.MaintenanceCycles.Read(t.NormalMaintenanceCycle.MaintenanceCycleId);
            bool changeCycle = false;
            int startReducedCycle = 0;
            if (t.Act != null && t.Act.Name != "Не указан")
            {
                if (t.Act.CreationDate.Month >= 10) startReducedCycle = t.Act.CreationDate.Year + 1;
                else startReducedCycle = t.Act.CreationDate.Year;
                changeCycle = true;
            }
            MaintenanceCycle continuationCycle = getContinuationCycle(maintenanceCycle);
            MaintenanceYear recoverYear = maintenanceCycle.MaintenanceYears.First(x => x.MaintenanceType.Name.Contains("В"));
            int initialYear = t.InputYear.Value;
            int currentYear = 0;
            for (int i = currentYear; i <= recoverYear.Year; i++)
            {
                addNewRecord(maintenanceCycle, t, i, initialYear, currentYear);
                currentYear++;
            }
            for (int i = initialYear + currentYear, maintainedYear = 1; i <= DateTime.Now.Year + 11; i++, maintainedYear++)
            {
                if (changeCycle == true & (initialYear + currentYear) == startReducedCycle)
                {
                    maintenanceCycle = dataBase.MaintenanceCycles.Read(t.ReducedMaintenanceCycle.MaintenanceCycleId);
                    continuationCycle = getContinuationCycle(maintenanceCycle);
                    recoverYear = maintenanceCycle.MaintenanceYears.First(x => x.MaintenanceType.Name.Contains("В"));
                }
                addNewRecord(continuationCycle, t, maintainedYear, initialYear, currentYear);
                currentYear++;
                if (maintainedYear == recoverYear.Year) maintainedYear = 0;
            }
        }

        public void UpdateRecords(Device t)
        {
            Device oldDevice = dataBase.Devices.Read(t.MaintainedEquipmentId);
            MaintenanceCycle maintenanceCycle = null;
            if (t.Act == null || t.Act.Name == "Не указан")
            {
                if (/*(oldDevice.NormalMaintenanceCycle.MaintenanceCycleId != t.NormalMaintenanceCycle.MaintenanceCycleId) ||*/
                    ((oldDevice.Act != null || oldDevice.Act.Name != "Не указан") && (t.Act == null || t.Act.Name == "Не указан")))
                {
                    maintenanceCycle = dataBase.MaintenanceCycles.Read(t.NormalMaintenanceCycle.MaintenanceCycleId);
                }
            }
            else if (/*(oldDevice.ReducedMaintenanceCycle.MaintenanceCycleId != t.ReducedMaintenanceCycle.MaintenanceCycleId) ||*/
                      ((oldDevice.Act == null || oldDevice.Act.Name == "Не указан") && (t.Act != null || t.Act.Name != "Не указан")))
            {
                maintenanceCycle = dataBase.MaintenanceCycles.Read(t.ReducedMaintenanceCycle.MaintenanceCycleId);
            }
            if (maintenanceCycle != null)
            {
                updateDeviceRecords(maintenanceCycle, oldDevice);
            }
        }

        public void UpdateRecords(MaintenanceCycle newMaintenanceCycle, Device t)
        {                      
            MaintenanceCycle maintenanceCycle = null;
            if (t.Act == null || t.Act.Name == "Не указан"
                && t.NormalMaintenanceCycle.MaintenanceCycleId == newMaintenanceCycle.MaintenanceCycleId)
            {
                maintenanceCycle = newMaintenanceCycle;
            }
            else if (t.ReducedMaintenanceCycle.MaintenanceCycleId == newMaintenanceCycle.MaintenanceCycleId)
            {
                maintenanceCycle = newMaintenanceCycle;
            }
            if (maintenanceCycle != null)
            {                
                updateDeviceRecords(maintenanceCycle, t);
            }
        }

        public void UpdateRecords(MaintenanceCycle deletedMaintenanceCycle, MaintenanceCycle newMaintenanceCycle, Device t)
        {
            if (t.NormalMaintenanceCycle.MaintenanceCycleId == deletedMaintenanceCycle.MaintenanceCycleId)
            {
                t.NormalMaintenanceCycle = newMaintenanceCycle;
            }
            if (t.ReducedMaintenanceCycle.MaintenanceCycleId == deletedMaintenanceCycle.MaintenanceCycleId)
            {
                t.ReducedMaintenanceCycle = newMaintenanceCycle;                               
            }
            UpdateRecords(newMaintenanceCycle, t);
        }

        /// <summary>
        /// Обновление графика устройства по циклу ТО
        /// </summary>
        /// <param name="maintenanceCycle">Новый цикл технического обслуживания</param>
        /// <param name="device">Устройство для которого обновляется графи</param>
        protected void updateDeviceRecords(MaintenanceCycle maintenanceCycle, Device device)
        {
            List<MaintenanceRecord> deleteMaintenanceRecords = dataBase.Devices.Read(device.MaintainedEquipmentId).MaintenanceRecords.FindAll(x => x.ActualMaintenanceDate == null
                                                                                                                                              && !(x.IsPlanned || x.IsRescheduled));            
            foreach (MaintenanceRecord record in deleteMaintenanceRecords)
            {
                dataBase.MaintenanceRecords.Delete(record);
            }
            MaintenanceCycle continuationCycle = getContinuationCycle(maintenanceCycle);
            MaintenanceYear recoverYear = maintenanceCycle.MaintenanceYears.First(x => x.MaintenanceType.Name.Contains("В"));
            if (device.MaintenanceRecords.Count == 0)
            {
                int initialYear = device.InputYear.Value;
                int currentYear = 0;
                for (int i = 0; i <= recoverYear.Year; i++, currentYear++)
                {
                    //if (checkYear(initialYear + currentYear)) continue;
                    addNewRecord(maintenanceCycle, device, i, initialYear, currentYear);                    
                }
                for (int i = currentYear, maintainedYear = 1; i <= DateTime.Now.Year + 10; i++, maintainedYear++, currentYear++)
                {
                    if (maintainedYear > recoverYear.Year) maintainedYear = 1;
                    //if (checkYear(initialYear + currentYear)) continue;
                    addNewRecord(continuationCycle, device, maintainedYear, initialYear, currentYear);                   
                }
            }
            else
            {
                MaintenanceRecord lastRecoverMaintenanceRecord = device.MaintenanceRecords.FindLast(x => x.PlannedMaintenanceType.Name.Contains("В") || 
                                                                                                         x.PlannedMaintenanceType.Name.Contains("Н"));                
                                
                int initialYear = lastRecoverMaintenanceRecord.PlannedMaintenanceDate.Year;
                int nextMaintainedYear;
                if (DateTime.Now.Month > 1)
                {
                    nextMaintainedYear = DateTime.Now.Year - initialYear + 1;
                }
                else
                {
                    nextMaintainedYear = DateTime.Now.Year - initialYear;
                }
                int checkPeriod = recoverYear.Year - maintenanceCycle.MaintenanceYears.Last(x => x.MaintenanceType.Name.Contains("К")).Year;
                if (checkYear(initialYear + recoverYear.Year))
                {
                    addNewRecord(maintenanceCycle, device, recoverYear.Year, initialYear, nextMaintainedYear);
                    lastRecoverMaintenanceRecord = device.MaintenanceRecords.Last();
                    initialYear = lastRecoverMaintenanceRecord.PlannedMaintenanceDate.Year;
                    nextMaintainedYear = 1;
                }     
                else if(checkPeriod != 1)
                {                 
                    if (maintenanceCycle.MaintenanceYears.FirstOrDefault(x => x.Year == nextMaintainedYear - 1 && x.MaintenanceType.Name.Contains("К")) != null)
                    {
                        addNewRecord(maintenanceCycle, device, nextMaintainedYear - 1, initialYear, nextMaintainedYear);
                        nextMaintainedYear++;
                    }
                    else if (device.MaintenanceRecords.Last().PlannedMaintenanceType.Name.Contains("К") &&
                        maintenanceCycle.MaintenanceYears.FirstOrDefault(x => x.Year == nextMaintainedYear && x.MaintenanceType.Name.Contains("К")) != null &&
                        maintenanceCycle.MaintenanceYears.FirstOrDefault(x => x.Year == nextMaintainedYear + 1 && x.MaintenanceType.Name.Contains("К")) == null)
                    {
                        addNewRecord(maintenanceCycle, device, nextMaintainedYear, initialYear, nextMaintainedYear + 1);
                        nextMaintainedYear += 2;
                    }
                }
                int currentYear = 0;
                if (lastRecoverMaintenanceRecord.PlannedMaintenanceType.Name.Contains("Н"))
                {
                    for (int i = nextMaintainedYear; i <= recoverYear.Year; i++, currentYear++)
                    {
                        //if (checkYear(initialYear + nextMaintainedYear + currentYear)) continue;
                        addNewRecord(maintenanceCycle, device, i, initialYear, nextMaintainedYear + currentYear);
                    }
                    for (int i = currentYear, maintainedYear = 1; i <= 10; i++, maintainedYear++, currentYear++)
                    {
                        if (maintainedYear > recoverYear.Year) maintainedYear = 1;
                        //if (checkYear(initialYear + nextMaintainedYear + currentYear)) continue;
                        addNewRecord(continuationCycle, device, maintainedYear, initialYear, nextMaintainedYear + currentYear);
                    }
                }
                else if (lastRecoverMaintenanceRecord.PlannedMaintenanceType.Name.Contains("В"))
                {
                    for (int i = currentYear, maintainedYear = nextMaintainedYear; i <= 10; i++, maintainedYear++, currentYear++)
                    {
                        if (maintainedYear > recoverYear.Year) maintainedYear = 1;
                        //if (checkYear(initialYear + nextMaintainedYear + currentYear)) continue;                        
                        addNewRecord(continuationCycle, device, maintainedYear, initialYear, nextMaintainedYear + currentYear);
                    }
                }
            }
            //удалить потом
            //device.LastRecovery = device.MaintenanceRecords.FindLast(x => x.ActualMaintenanceType != null && (x.ActualMaintenanceType.Name.Contains("В") ||
            //                                                                                                 x.ActualMaintenanceType.Name.Contains("Н"))).ActualMaintenanceDate;
            dataBase.Save();
        }
    }
}