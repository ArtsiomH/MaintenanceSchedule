using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Entities;
using MaintenanceScheduleDataLayer.EFContext;
using System;
using System.Threading.Tasks;

namespace MaintenanceScheduleDataLayer.Repositories
{
    class DeviceRepository : IRepository<Device>
    {
        private MaintenanceScheduleContext context;

        public DeviceRepository(MaintenanceScheduleContext context)
        {
            this.context = context;
        }

        public void Create(Device t)
        {
            context.Devices.Add(t);
        }

        public Device Read(int id)
        {
            Device device = context.Devices.Include(x => x.MaintenanceRecords.Select(m => m.PlannedMaintenanceType))
                                  .First(x => x.MaintainedEquipmentId == id);
            device.MaintenanceRecords = device.MaintenanceRecords.OrderBy(x => x.PlannedMaintenanceDate).ToList();
            return device;
        }

        public void Update(Device t)
        {
            Device oldDevice = Read(t.MaintainedEquipmentId);
            UpdateEntityReflection.Update(oldDevice, t);
            context.Entry(oldDevice).State = EntityState.Modified;
        }

        public void Delete(Device t)
        {
            Device device = Read(t.MaintainedEquipmentId);
            context.Devices.Remove(device);
        }

        public IEnumerable<Device> GetAll()
        {
            return context.Devices
                          .Include(x => x.NormalMaintenanceCycle)
                          .Include(x => x.ReducedMaintenanceCycle)                
                          .ToList();
        }

        public Task<Device> ReadAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
