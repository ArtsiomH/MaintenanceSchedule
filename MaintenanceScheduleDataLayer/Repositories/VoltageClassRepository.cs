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
    class VoltageClassRepository : IRepository<VoltageClass>
    {
        private MaintenanceScheduleContext context;

        public VoltageClassRepository(MaintenanceScheduleContext context)
        {
            this.context = context;
        }

        public void Create(VoltageClass t)
        {
            context.VoltageClasses.Add(t);
        }
                
        public VoltageClass Read(int id)
        {
            return context.VoltageClasses.Find(id);
        }

        public void Update(VoltageClass t)
        {
            VoltageClass oldVoltageClass = Read(t.VoltageClassId);
            UpdateEntityReflection.Update(oldVoltageClass, t);
            context.Entry(oldVoltageClass).State = EntityState.Modified;
        }

        public void Delete(VoltageClass t)
        {
            VoltageClass voltageClass = Read(t.VoltageClassId);
            context.VoltageClasses.Remove(voltageClass);
        }

        public IEnumerable<VoltageClass> GetAll()
        {
            return context.VoltageClasses.Include(x => x.Attachments).ToList();
        }

        public Task<VoltageClass> ReadAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
