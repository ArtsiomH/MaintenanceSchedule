using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Entities;
using MaintenanceScheduleDataLayer.EFContext;

namespace MaintenanceScheduleDataLayer.Repositories
{
    class MaintenanceCycleRepository : IRepository<MaintenanceCycle>
    {
        private MaintenanceScheduleContext context;

        public MaintenanceCycleRepository(MaintenanceScheduleContext context)
        {
            this.context = context;
        }

        public void Create(MaintenanceCycle t)
        {
            context.MaintenanceCycles.Add(t);
        }
                
        public MaintenanceCycle Read(int id)
        {
            return context.MaintenanceCycles.Include(x => x.MaintenanceYears)
                                            .Include(x => x.NormalEquipments)
                                            .Include(x => x.ReducedEquipments)                                            
                                            .First(x => x.MaintenanceCycleId == id);
        }

        public void Update(MaintenanceCycle t)
        {
            MaintenanceCycle oldMaintenanceCycle = Read(t.MaintenanceCycleId);
            UpdateEntityReflection.Update(oldMaintenanceCycle, t);
            context.Entry(oldMaintenanceCycle).State = EntityState.Modified;
        }

        public void Delete(MaintenanceCycle t)
        {
            MaintenanceCycle maintenanceCycle = Read(t.MaintenanceCycleId);
            context.MaintenanceCycles.Remove(maintenanceCycle);
        }

        public IEnumerable<MaintenanceCycle> GetAll()
        {
            return context.MaintenanceCycles.Include(x => x.MaintenanceYears.Select(t => t.MaintenanceType))
                                            .ToList();
        }
    }
}