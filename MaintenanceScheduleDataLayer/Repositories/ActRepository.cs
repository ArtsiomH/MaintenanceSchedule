using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Entities;
using MaintenanceScheduleDataLayer.EFContext;
using System;
using System.Threading.Tasks;

namespace MaintenanceScheduleDataLayer.Repositories
{
    class ActRepository : IRepository<Act>
    {
        private MaintenanceScheduleContext context;

        public ActRepository(MaintenanceScheduleContext context)
        {
            this.context = context;            
        }

        public void Create(Act t)
        {
            context.Acts.Add(t);
        }

        public Act Read(int id)
        {
            return context.Acts.Find(id);
        }

        public void Update(Act t)
        {
            Act oldAct = context.Acts.Find(t.ActId);
            UpdateEntityReflection.Update(oldAct, t);
            context.Entry(oldAct).State = EntityState.Modified;
        }

        public void Delete(Act t)
        {
            Act act = Read(t.ActId);
            context.Acts.Remove(act);
        }

        public IEnumerable<Act> GetAll()
        {
            return context.Acts.Include(x => x.AdditionalDevices)
                               .Include(x => x.Devices)
                               .ToList();
        }

        public Task<Act> ReadAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
