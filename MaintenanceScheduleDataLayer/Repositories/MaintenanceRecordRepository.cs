using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Entities;
using MaintenanceScheduleDataLayer.EFContext;
using System.Diagnostics;
using System;

namespace MaintenanceScheduleDataLayer.Repositories
{
    class MaintenanceRecordRepository : IRepository<MaintenanceRecord>
    {
        private MaintenanceScheduleContext context;

        public MaintenanceRecordRepository(MaintenanceScheduleContext context)
        {
            this.context = context;
        }

        public void Create(MaintenanceRecord t)
        {
            context.MaintenanceRecords.Add(t);
        }
                
        public MaintenanceRecord Read(int id)
        {
            try
            {
                return context.MaintenanceRecords.Find(id);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return new MaintenanceRecord();
            
        }

        public void Update(MaintenanceRecord t)
        {
            MaintenanceRecord oldMaintenanceRecord = Read(t.MaintenanceRecordId);
            UpdateEntityReflection.Update(oldMaintenanceRecord, t);
            context.Entry(oldMaintenanceRecord).State = EntityState.Modified;
        }

        public void Delete(MaintenanceRecord t)
        {
            try
            {
                MaintenanceRecord maintenanceRecord = Read(t.MaintenanceRecordId);
                context.MaintenanceRecords.Remove(maintenanceRecord);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }            
        }

        public IEnumerable<MaintenanceRecord> GetAll()
        {
            return context.MaintenanceRecords.Include(x => x.PlannedMaintenanceType)
                                             .Include(x => x.ActualMaintenanceType)
                                             .Include(x => x.MaintainedEquipment)
                                             .ToList();
        }
    }
}
