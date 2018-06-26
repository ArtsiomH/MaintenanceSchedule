using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MaintenanceSchedule.Interfaces;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Entities;

namespace MaintenanceSchedule.Services
{
    class SubstationService : ISubstationService
    {
        IUnitOfWork dataBase;

        public SubstationService(IUnitOfWork dataBase)
        {
            this.dataBase = dataBase;
        }

        public void Create(Substation t)
        {
            MaintenanceType type = dataBase.MaintenanceTypes.GetAll().First(x => x.Name == "осмотр");
            MaintenanceRecord record;
            int section = 12 / t.InspectionsFrequency.Count;
            for (int year = t.InputYear.Value; year <= DateTime.Now.Year + 10; year++)
            {                
                for (int i = 1; i <= t.InspectionsFrequency.Count; i++)
                {
                    record = new MaintenanceRecord();
                    if (year < DateTime.Now.Year)
                    {
                        record.PlannedMaintenanceDate = new DateTime(year, section * i, 1);
                        record.PlannedMaintenanceType = type;
                        record.ActualMaintenanceDate = new DateTime(year, section * i, 1);
                        record.ActualMaintenanceType = type;
                        record.IsPlanned = true;
                    }
                    else if (year == DateTime.Now.Year && DateTime.Now.Month > 2)
                    {
                        if (DateTime.Now.Month > section * i)
                        {
                            record.PlannedMaintenanceDate = new DateTime(year, section * i, 1);
                            record.ActualMaintenanceDate = new DateTime(year, section * i, 1);
                            record.PlannedMaintenanceType = type;
                            record.ActualMaintenanceType = type;
                        }
                        else if (t.InspectionsFrequency.Count == 1)
                        {
                            record.PlannedMaintenanceDate = new DateTime(year, section * i / 2, 1);
                        }
                        else
                        {
                            record.PlannedMaintenanceDate = new DateTime(year, section * i, 1);
                        }
                        record.PlannedMaintenanceType = type;
                        record.IsPlanned = true;
                    }
                    else
                    {
                        if (t.InspectionsFrequency.Count == 12)
                        {
                            record.PlannedMaintenanceDate = new DateTime(year, section * i, 1);
                        }
                        else
                        {
                            record.PlannedMaintenanceDate = new DateTime(year, 1, 1);
                        }
                        record.PlannedMaintenanceType = type;
                        record.IsPlanned = false;                  
                    }
                    record.IsRescheduled = false;
                    record.MaintainedEquipment = t;
                    dataBase.MaintenanceRecords.Create(record);
                }
            }            
            dataBase.Substations.Create(t);
            dataBase.Save();
        }

        public void Delete(Substation t)
        {
            dataBase.Substations.Delete(t);
            dataBase.Save();
        }

        public Substation Get(int id)
        {
            return dataBase.Substations.Read(id);
        }

        public ObservableCollection<Substation> GetAll()
        {
            return new ObservableCollection<Substation>(dataBase.Substations.GetAll());
        }



        public void Update(Substation t)
        {            
            List<MaintenanceRecord> deleteMaintenanceRecords = t.MaintenanceRecords.Where(x => x.ActualMaintenanceDate == null
                                                                                           && !(x.IsPlanned || x.IsRescheduled)).ToList();
            foreach (MaintenanceRecord deletedRecord in deleteMaintenanceRecords)
            {
                dataBase.MaintenanceRecords.Delete(deletedRecord);
            }
            dataBase.Save();
            MaintenanceType type = dataBase.MaintenanceTypes.GetAll().First(x => x.Name == "осмотр");
            List<MaintenanceRecord> maintenanceRecords = dataBase.Substations.Read(t.MaintainedEquipmentId).MaintenanceRecords;
            MaintenanceRecord record;
            int section = 12 / t.InspectionsFrequency.Count;             
            int currentYear = 0;            
            if (maintenanceRecords.Count == 0)
            {
                currentYear = t.InputYear.Value;
            }
            else
            {
                currentYear = t.MaintenanceRecords.Max(x => x.PlannedMaintenanceDate.Year) + 1;
            }
            for (int year = currentYear; year <= currentYear + 10; year++)
            {
                for (int i = 1; i <= t.InspectionsFrequency.Count; i++)
                {
                    record = new MaintenanceRecord();
                    if (t.InspectionsFrequency.Count == 12)
                    {
                        record.PlannedMaintenanceDate = new DateTime(year, section * i, 1);
                    }
                    else
                    {
                        record.PlannedMaintenanceDate = new DateTime(year, 1, 1);
                    }
                    record.PlannedMaintenanceType = type;                    
                    record.IsPlanned = false;
                    record.IsRescheduled = false;
                    record.MaintainedEquipment = t;
                    dataBase.MaintenanceRecords.Create(record);
                }
            }
            dataBase.Substations.Update(t);
            dataBase.Save();
        }

        public void MarkActualRecord(Substation substation, DateTime date)
        {
            MaintenanceRecord record = substation.MaintenanceRecords.FirstOrDefault(x => x.ActualMaintenanceDate == null && x.IsPlanned == true && x.IsRescheduled == false);
            if (record == null) return;
            MaintenanceType type = dataBase.MaintenanceTypes.GetAll().First(x => x.Name == "осмотр");
            record.ActualMaintenanceDate = date;
            record.ActualMaintenanceType = type;
            dataBase.Save();
        }
    }
}