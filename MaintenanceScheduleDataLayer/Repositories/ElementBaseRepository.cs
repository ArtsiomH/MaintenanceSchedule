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
    class ElementBaseRepository : IRepository<ElementBase>
    {
        private MaintenanceScheduleContext context;

        public ElementBaseRepository(MaintenanceScheduleContext context)
        {
            this.context = context;
        }

        public void Create(ElementBase t)
        {
            context.ElementBases.Add(t);
        }

        public ElementBase Read(int id)
        {
            return context.ElementBases.Find(id);
        }

        public void Update(ElementBase t)
        {
            ElementBase oldElementBase = Read(t.ElementBaseId);
            UpdateEntityReflection.Update(oldElementBase, t);
            context.Entry(oldElementBase).State = EntityState.Modified;
        }

        public void Delete(ElementBase t)
        {
            ElementBase elementBase = Read(t.ElementBaseId);
            context.ElementBases.Remove(elementBase);
        }

        public IEnumerable<ElementBase> GetAll()
        {
            return context.ElementBases.Include(x => x.Devices).ToList();
        }

        public Task<ElementBase> ReadAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
