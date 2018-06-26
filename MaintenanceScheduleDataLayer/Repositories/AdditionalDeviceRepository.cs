﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Entities;
using MaintenanceScheduleDataLayer.EFContext;

namespace MaintenanceScheduleDataLayer.Repositories
{
    class AdditionalDeviceRepository : IRepository<AdditionalDevice>
    {
        private MaintenanceScheduleContext context;

        public AdditionalDeviceRepository(MaintenanceScheduleContext context)
        {
            this.context = context;
        }

        public void Create(AdditionalDevice t)
        {
            context.AdditionalDevices.Add(t);
        }
        
        public AdditionalDevice Read(int id)
        {
            return context.AdditionalDevices
                          .Include(x => x.MaintenanceRecords.Select(m => m.PlannedMaintenanceType))
                          .First(x => x.MaintainedEquipmentId == id);
        }

        public void Update(AdditionalDevice t)
        {
            AdditionalDevice oldAdditionalDevice = Read(t.MaintainedEquipmentId);
            UpdateEntityReflection.Update(oldAdditionalDevice, t);
            context.Entry(oldAdditionalDevice).State = EntityState.Modified;
        }

        public void Delete(AdditionalDevice t)
        {
            AdditionalDevice additionalDevice = Read(t.MaintainedEquipmentId);
            context.AdditionalDevices.Remove(additionalDevice);
        }

        public IEnumerable<AdditionalDevice> GetAll()
        {
            return context.AdditionalDevices.Include(x => x.Attachment)
                                            .Include(x => x.Act)
                                            .Include(x => x.NormalMaintenanceCycle)
                                            .Include(x => x.ReducedMaintenanceCycle)
                                            .ToList();
        }
    }
}
