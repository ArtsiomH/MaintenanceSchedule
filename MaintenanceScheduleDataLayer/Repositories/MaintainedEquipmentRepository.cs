using MaintenanceScheduleDataLayer.EFContext;
using MaintenanceScheduleDataLayer.Entities;
using MaintenanceScheduleDataLayer.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MaintenanceScheduleDataLayer.Repositories
{
    class MaintainedEquipmentRepository : IRepository<MaintainedEquipment>
    {
        private MaintenanceScheduleContext context;

        public MaintainedEquipmentRepository(MaintenanceScheduleContext context)
        {
            this.context = context;
        }

        public void Create(MaintainedEquipment t)
        {
            context.MaintainedEquipments.Add(t);
        }

        public MaintainedEquipment Read(int id)
        {
            return context.MaintainedEquipments.Include(x => x.MaintenanceRecords)
                                               .First(x => x.MaintainedEquipmentId == id);
        }

        public void Update(MaintainedEquipment t)
        {
            MaintainedEquipment oldMaintainedEquipment = context.MaintainedEquipments.Find(t.MaintainedEquipmentId);
            UpdateEntityReflection.Update(oldMaintainedEquipment, t);
            context.Entry(oldMaintainedEquipment).State = EntityState.Modified;
        }

        public void Delete(MaintainedEquipment t)
        {
            MaintainedEquipment maintainedEquipment = Read(t.MaintainedEquipmentId);
            context.MaintainedEquipments.Remove(maintainedEquipment);
        }

        public IEnumerable<MaintainedEquipment> GetAll()
        {
            return context.MaintainedEquipments.ToList();
        }
    }
}
