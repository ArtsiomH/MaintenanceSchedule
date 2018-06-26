using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Entities;
using MaintenanceScheduleDataLayer.EFContext;

namespace MaintenanceScheduleDataLayer.Repositories
{
    class ManagementOrganizationRepository : IRepository<ManagementOrganization>
    {
        private MaintenanceScheduleContext context;

        public ManagementOrganizationRepository(MaintenanceScheduleContext context)
        {
            this.context = context;
        }

        public void Create(ManagementOrganization t)
        {
            context.ManagementOrganizations.Add(t);
        }
                
        public ManagementOrganization Read(int id)
        {
            return context.ManagementOrganizations.Find(id);
        }

        public void Update(ManagementOrganization t)
        {
            ManagementOrganization oldManagementOrganization = Read(t.ManagementOrganizationId);
            UpdateEntityReflection.Update(oldManagementOrganization, t);
            context.Entry(oldManagementOrganization).State = EntityState.Modified;
        }

        public void Delete(ManagementOrganization t)
        {
            ManagementOrganization managementOrganization = Read(t.ManagementOrganizationId);
            context.Entry(managementOrganization).State = EntityState.Modified;
        }

        public IEnumerable<ManagementOrganization> GetAll()
        {
            return context.ManagementOrganizations.Include(x => x.Attachments).ToList();
        }
    }
}
