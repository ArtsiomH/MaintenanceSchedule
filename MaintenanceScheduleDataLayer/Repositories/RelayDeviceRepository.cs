using MaintenanceScheduleDataLayer.EFContext;
using MaintenanceScheduleDataLayer.Entities;
using MaintenanceScheduleDataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceScheduleDataLayer.Repositories
{
    class RelayDeviceRepository : IRepository<RelayDevice>
    {
        private MaintenanceScheduleContext context;

        public RelayDeviceRepository(MaintenanceScheduleContext context)
        {
            this.context = context;
        }

        public void Create(RelayDevice t)
        {
            context.RelayDevices.Add(t);
        }

        public RelayDevice Read(int id)
        {
            return context.RelayDevices.Include(x => x.MaintenanceRecords.Select(m => m.PlannedMaintenanceType))
                                           .Include(x => x.CombineDevices.Select(m => m.MaintenanceRecords.Select(t => t.PlannedMaintenanceType)))
                                           .First(x => x.MaintainedEquipmentId == id);
        }

        public async Task<RelayDevice> ReadAsync(int id)
        {            
            return await Task.Run(() =>
            {
                using (MaintenanceScheduleContext dbContext = new MaintenanceScheduleContext("ProbLoc"))
                {
                    return dbContext.RelayDevices
                                    .Include(x => x.Attachment.Substation)
                                    .Include(x => x.Act)
                                    .Include(x => x.Manufacturer)
                                    .Include(x => x.ElementBase)
                                    .Include(x => x.DeviceType)
                                    .Include(x => x.NormalMaintenanceCycle)
                                    .Include(x => x.NormalMaintenanceCycle.MaintenanceYears.Select(y => y.MaintenanceType))
                                    .Include(x => x.ReducedMaintenanceCycle)                                    
                                    .Include(x => x.MaintenanceRecords.Select(m => m.PlannedMaintenanceType))                                    
                                    .Include(x => x.CombineDevices.Select(m => m.MaintenanceRecords.Select(t => t.PlannedMaintenanceType)))
                                    .First(x => x.MaintainedEquipmentId == id);
                }
            }); 
        }

        public void Update(RelayDevice t)
        {
            RelayDevice oldRelayDevice = context.RelayDevices.First(x => x.MaintainedEquipmentId == t.MaintainedEquipmentId);
            UpdateEntityReflection.Update(oldRelayDevice, t);
            context.Entry(oldRelayDevice).State = EntityState.Modified;
        }

        public void Delete(RelayDevice t)
        {
            RelayDevice relayDevice = context.RelayDevices.First(x => x.MaintainedEquipmentId == t.MaintainedEquipmentId);
            context.Devices.Remove(relayDevice);                        
        }

        public IEnumerable<RelayDevice> GetAll()
        {
            return context.RelayDevices
                .Include(x => x.Attachment)
                .Include(x => x.Act)
                .Include(x => x.Manufacturer)
                .Include(x => x.ElementBase)
                .Include(x => x.DeviceType)
                .Include(x => x.NormalMaintenanceCycle)
                .Include(x => x.ReducedMaintenanceCycle)
                .ToList();
        }
    }
}
