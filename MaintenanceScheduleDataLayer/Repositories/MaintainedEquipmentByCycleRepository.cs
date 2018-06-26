using MaintenanceScheduleDataLayer.EFContext;
using MaintenanceScheduleDataLayer.Entities;
using MaintenanceScheduleDataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceScheduleDataLayer.Repositories
{
    class MaintainedEquipmentByCycleRepository : IRepository<MaintainedEquipmentByCycle>
    {
        private MaintenanceScheduleContext context;

        public MaintainedEquipmentByCycleRepository(MaintenanceScheduleContext context)
        {
            this.context = context;
        }

        public void Create(MaintainedEquipmentByCycle t)
        {
            context.MaintainedEquipmentsByCycle.Add(t);
        }

        public MaintainedEquipmentByCycle Read(int id)
        {
            return context.MaintainedEquipmentsByCycle                
                .First(x => x.MaintainedEquipmentId == id);
        }

        public void Update(MaintainedEquipmentByCycle t)
        {
            MaintainedEquipmentByCycle oldMaintainedEquipmentByCycle = Read(t.MaintainedEquipmentId);
            UpdateEntityReflection.Update(oldMaintainedEquipmentByCycle, t);
            context.Entry(oldMaintainedEquipmentByCycle).State = EntityState.Modified;
        }

        public void Delete(MaintainedEquipmentByCycle t)
        {
            MaintainedEquipmentByCycle maintainedEquipmentByCycle = Read(t.MaintainedEquipmentId);
            context.MaintainedEquipmentsByCycle.Remove(maintainedEquipmentByCycle);
        }

        public IEnumerable<MaintainedEquipmentByCycle> GetAll()
        {
            return context.MaintainedEquipmentsByCycle
                .ToList();
        }

    }
}
