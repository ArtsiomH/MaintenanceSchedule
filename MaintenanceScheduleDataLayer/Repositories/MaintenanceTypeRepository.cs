using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Entities;
using MaintenanceScheduleDataLayer.EFContext;
using System;
using System.Threading.Tasks;

namespace MaintenanceScheduleDataLayer.Repositories
{
    class MaintenanceTypeRepository : IRepository<MaintenanceType>
    {
        private MaintenanceScheduleContext context;

        public MaintenanceTypeRepository(MaintenanceScheduleContext context)
        {
            this.context = context;
        }

        public void Create(MaintenanceType t)
        {
            context.MaintenanceTypes.Add(t);
        }
                
        public MaintenanceType Read(int id)
        {
            return context.MaintenanceTypes.Find(id);
        }

        public void Update(MaintenanceType t)
        {
            MaintenanceType oldMaintenanceType = Read(t.MaintenanceTypeId);
            UpdateEntityReflection.Update(oldMaintenanceType, t);
            context.Entry(oldMaintenanceType).State = EntityState.Modified;
        }

        public void Delete(MaintenanceType t)
        {
            MaintenanceType maintenanceType = Read(t.MaintenanceTypeId);
            context.MaintenanceTypes.Remove(maintenanceType);
        }

        public IEnumerable<MaintenanceType> GetAll()
        {
            return context.MaintenanceTypes.ToList();
        }

        public Task<MaintenanceType> ReadAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
