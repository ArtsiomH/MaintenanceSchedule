using MaintenanceSchedule.Interfaces;
using MaintenanceScheduleDataLayer.Entities;
using MaintenanceScheduleDataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MaintenanceSchedule.Services
{
    public class MaintainedEquipmentByCycleService : IMaintainedEquipmentByCycleService
    {
        protected IUnitOfWork dataBase;

        public MaintainedEquipmentByCycleService(IUnitOfWork dataBase)
        {
            this.dataBase = dataBase;
        }

        protected void addNewRecord(MaintenanceCycle maintenanceCycle, MaintainedEquipment equipment, int maintainedYear, int initialYear, int currentYear)
        {
            MaintenanceRecord newRecord = new MaintenanceRecord();
            MaintenanceYear year = maintenanceCycle.MaintenanceYears.FirstOrDefault(x => x.Year == maintainedYear);
            if (year != null)
            {
                newRecord = new MaintenanceRecord();
                newRecord.MaintainedEquipment = equipment;
                if (DateTime.Now.Year > (initialYear + currentYear))
                {
                    newRecord.ActualMaintenanceDate = new DateTime(initialYear + currentYear, 1, 1);
                    newRecord.ActualMaintenanceType = dataBase.MaintenanceTypes.GetAll().First(x => x.Name == year.MaintenanceType.Name);
                    newRecord.IsPlanned = true;
                }
                else if (DateTime.Now.Year == (initialYear + currentYear))
                {
                    newRecord.IsPlanned = true;
                }
                newRecord.PlannedMaintenanceDate = new DateTime(initialYear + currentYear, 1, 1);
                newRecord.PlannedMaintenanceType = dataBase.MaintenanceTypes.GetAll().First(x => x.Name == year.MaintenanceType.Name);
                dataBase.MaintenanceRecords.Create(newRecord);
            }
        }

        protected MaintenanceCycle getContinuationCycle(MaintenanceCycle maintenanceCycle)
        {
            MaintenanceCycle continuationCycle = new MaintenanceCycle();
            continuationCycle.MaintenanceYears = new List<MaintenanceYear>();
            MaintenanceYear recoverYear = maintenanceCycle.MaintenanceYears.First(x => x.MaintenanceType.Name.Contains("В"));
            foreach (MaintenanceYear cycleYear in maintenanceCycle.MaintenanceYears)
            {
                MaintenanceYear newYear;
                newYear = new MaintenanceYear();
                newYear.MaintenanceType = new MaintenanceType();
                if (cycleYear.Year == 0)
                {
                    newYear.MaintenanceType.Name = recoverYear.MaintenanceType.Name;
                }
                else if (cycleYear.Year == 1)
                {
                    MaintenanceYear desiredYear = maintenanceCycle.MaintenanceYears.FirstOrDefault(x => x.Year == (recoverYear.Year - 1));
                    if (desiredYear != null)
                    {
                        newYear.MaintenanceType.Name = desiredYear.MaintenanceType.Name;
                    }
                    else continue;
                }
                else
                {
                    newYear.MaintenanceType.Name = cycleYear.MaintenanceType.Name;
                }
                newYear.Year = cycleYear.Year;
                continuationCycle.MaintenanceYears.Add(newYear);
            }
            return continuationCycle;
        }

        public void MarkActualRecord(MaintainedEquipmentByCycle equpment, DateTime date, MaintenanceType type)
        {
            MaintenanceRecord record = equpment.MaintenanceRecords.FirstOrDefault(x => x.ActualMaintenanceDate == null && x.IsPlanned == true && x.IsRescheduled == false);
            if (record == null) return;
            record.ActualMaintenanceDate = date;
            record.ActualMaintenanceType = type;
            if ((equpment is Device) && type.Name.Contains("В") || type.Name.Contains("Н"))
            {
                ((Device)equpment).LastRecovery = date;
            }
            dataBase.Save();
        }

        protected bool checkYear(int year)
        {
            return (DateTime.Now.Year > year) || (DateTime.Now.Year == year && DateTime.Now.Month > 1);
        }

        public MaintenanceCycle GetCurrentCycle(MaintainedEquipmentByCycle equpment)
        {
            MaintenanceCycle maintenanceCycle = null;
            if (equpment is Device)
            {
                if (((Device)equpment).Act == null || ((Device)equpment).Act.Name == "Не указан")
                {
                    return equpment.NormalMaintenanceCycle;
                }
                else
                {
                    return equpment.ReducedMaintenanceCycle;
                }
            }
            else if (equpment is AdditionalWork)
            {
                return equpment.NormalMaintenanceCycle;
            }
            return maintenanceCycle;
        }
    }
}