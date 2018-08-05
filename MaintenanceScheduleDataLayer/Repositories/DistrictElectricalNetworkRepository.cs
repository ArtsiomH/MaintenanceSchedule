using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Entities;
using MaintenanceScheduleDataLayer.EFContext;
using System.Threading.Tasks;

namespace MaintenanceScheduleDataLayer.Repositories
{
    class DistrictElectricalNetworkRepository : IRepository<DistrictElectricalNetwork>
    {
        private MaintenanceScheduleContext context;

        public DistrictElectricalNetworkRepository(MaintenanceScheduleContext context)
        {
            this.context = context;
        }

        public void Create(DistrictElectricalNetwork t)
        {
            context.DistrictElectricalNetworks.Add(t);
        }
        
        public DistrictElectricalNetwork Read(int id)
        {
            return context.DistrictElectricalNetworks.First(x => x.DistrictElectricalNetworkId == id);
        }

        public void Update(DistrictElectricalNetwork t)
        {
            DistrictElectricalNetwork oldDistricElectricalNetwork = Read(t.DistrictElectricalNetworkId);
            UpdateEntityReflection.Update(oldDistricElectricalNetwork, t);
            context.Entry(oldDistricElectricalNetwork).State = EntityState.Modified;
        }

        public void Delete(DistrictElectricalNetwork t)
        {
            DistrictElectricalNetwork districElectricalNetwork = Read(t.DistrictElectricalNetworkId);
            context.DistrictElectricalNetworks.Remove(districElectricalNetwork);
        }

        public IEnumerable<DistrictElectricalNetwork> GetAll()
        {
            return context.DistrictElectricalNetworks.ToList();
        }

        public Task<DistrictElectricalNetwork> ReadAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
