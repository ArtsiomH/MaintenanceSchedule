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
    class ManufacturerRepository : IRepository<Manufacturer>
    {
        private MaintenanceScheduleContext context;

        public ManufacturerRepository(MaintenanceScheduleContext context)
        {
            this.context = context;
        }

        public void Create(Manufacturer t)
        {
            context.Manufacturers.Add(t);
        }
                
        public Manufacturer Read(int id)
        {
            return context.Manufacturers.Find(id);
        }

        public void Update(Manufacturer t)
        {
            Manufacturer oldManufacturer = Read(t.ManufacturerId);
            UpdateEntityReflection.Update(oldManufacturer, t);
            context.Entry(oldManufacturer).State = EntityState.Modified;
        }

        public void Delete(Manufacturer t)
        {
            Manufacturer manufacturer = Read(t.ManufacturerId);
            context.Manufacturers.Remove(manufacturer);
        }

        public IEnumerable<Manufacturer> GetAll()
        {
            return context.Manufacturers.Include(x => x.Devices).ToList();
        }

        public Task<Manufacturer> ReadAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
