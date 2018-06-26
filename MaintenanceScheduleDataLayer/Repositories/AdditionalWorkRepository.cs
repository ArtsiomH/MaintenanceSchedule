using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Entities;
using MaintenanceScheduleDataLayer.EFContext;

namespace MaintenanceScheduleDataLayer.Repositories
{
    class AdditionalWorkRepository : IRepository<AdditionalWork>
    {
        private MaintenanceScheduleContext context;

        public AdditionalWorkRepository(MaintenanceScheduleContext context)
        {
            this.context = context;
        }

        public void Create(AdditionalWork t)
        {            
            context.AdditionalWorks.Add(t);
        }

        public AdditionalWork Read(int id)
        {
            return context.AdditionalWorks
                          .Include(x => x.MaintenanceRecords.Select(t => t.PlannedMaintenanceType))
                          .Include(x => x.Devices.Select(m => m.MaintenanceRecords.Select(t => t.PlannedMaintenanceType)))
                          .First(x => x.MaintainedEquipmentId == id);
        }

        public void Update(AdditionalWork t)
        {
            AdditionalWork oldCombineDevice = Read(t.MaintainedEquipmentId);
            UpdateEntityReflection.Update(oldCombineDevice, t);
            context.Entry(oldCombineDevice).State = EntityState.Modified;
        }

        public void Delete(AdditionalWork t)
        {
            AdditionalWork additionalWork = Read(t.MaintainedEquipmentId);
            context.AdditionalWorks.Remove(additionalWork);
        }

        public IEnumerable<AdditionalWork> GetAll()
        {
            return context.AdditionalWorks.Include(x => x.Substation)                        
                                         .Include(x => x.NormalMaintenanceCycle)
                                         .Include(x => x.ReducedMaintenanceCycle)                                         
                                         .ToList();
        }
    }
}
