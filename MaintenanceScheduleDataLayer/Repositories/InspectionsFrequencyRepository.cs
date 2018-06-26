using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Entities;
using MaintenanceScheduleDataLayer.EFContext;

namespace MaintenanceScheduleDataLayer.Repositories
{
    class InspectionsFrequencyRepository : IRepository<InspectionsFrequency>
    {
        private MaintenanceScheduleContext context;

        public InspectionsFrequencyRepository(MaintenanceScheduleContext context)
        {
            this.context = context;
        }

        public void Create(InspectionsFrequency t)
        {
            context.InspectionsFrequencies.Add(t);
        }
                
        public InspectionsFrequency Read(int id)
        {
            return context.InspectionsFrequencies.First(x => x.InspectionsFrequencyId == id);
        }

        public void Update(InspectionsFrequency t)
        {
            InspectionsFrequency oldInspectionsFrequency = Read(t.InspectionsFrequencyId);
            UpdateEntityReflection.Update(oldInspectionsFrequency, t);
            context.Entry(oldInspectionsFrequency).State = EntityState.Modified;
        }

        public void Delete(InspectionsFrequency t)
        {
            InspectionsFrequency inspectionsFrequency = Read(t.InspectionsFrequencyId);
            context.InspectionsFrequencies.Remove(inspectionsFrequency);
        }

        public IEnumerable<InspectionsFrequency> GetAll()
        {
            return context.InspectionsFrequencies.ToList();
        }
    }
}
