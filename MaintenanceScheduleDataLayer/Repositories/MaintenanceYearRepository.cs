using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Entities;
using MaintenanceScheduleDataLayer.EFContext;

namespace MaintenanceScheduleDataLayer.Repositories
{
    class MaintenanceYearRepository : IRepository<MaintenanceYear>
    {
        private MaintenanceScheduleContext context;

        public MaintenanceYearRepository(MaintenanceScheduleContext context)
        {
            this.context = context;
        }

        public void Create(MaintenanceYear t)
        {
            context.MaintenanceYears.Add(t);
        }
                
        public MaintenanceYear Read(int id)
        {
            return context.MaintenanceYears.First(x => x.MaintenanceYearId == id);
        }

        public void Update(MaintenanceYear t)
        {
            MaintenanceYear oldMaintenanceYear = Read(t.MaintenanceYearId);
            UpdateEntityReflection.Update(oldMaintenanceYear, t);
            context.Entry(oldMaintenanceYear).State = EntityState.Modified;
        }

        public void Delete(MaintenanceYear t)
        {
            MaintenanceYear maintenanceYear = Read(t.MaintenanceYearId);
            context.MaintenanceYears.Remove(maintenanceYear);
        }

        public IEnumerable<MaintenanceYear> GetAll()
        {
            return context.MaintenanceYears.Include(x => x.MaintenanceCycle)
                                           .Include(x => x.MaintenanceType)
                                           .ToList();
        }
    }
}
