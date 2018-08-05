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
    class DeviceTypeRepository : IRepository<DeviceType>
    {
        private MaintenanceScheduleContext context;

        public DeviceTypeRepository(MaintenanceScheduleContext context)
        {
            this.context = context;
        }

        public void Create(DeviceType t)
        {
            context.DeviceTypes.Add(t);
        }

        public DeviceType Read(int id)
        {
            return context.DeviceTypes.Find(id);
        }

        public void Update(DeviceType t)
        {
            DeviceType oldDeviceType = Read(t.DeviceTypeId);
            UpdateEntityReflection.Update(oldDeviceType, t);
            context.Entry(oldDeviceType).State = EntityState.Modified;
        }

        public void Delete(DeviceType t)
        {
            DeviceType deviceType = Read(t.DeviceTypeId);
            context.DeviceTypes.Remove(deviceType);
        }

        public IEnumerable<DeviceType> GetAll()
        {
            return context.DeviceTypes.Include(x => x.Devices).ToList();
        }

        public Task<DeviceType> ReadAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
