using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Entities;
using MaintenanceScheduleDataLayer.EFContext;

namespace MaintenanceScheduleDataLayer.Repositories
{
    class SubstationRepository : IRepository<Substation>
    {
        private MaintenanceScheduleContext context;

        public SubstationRepository(MaintenanceScheduleContext context)
        {
            this.context = context;
        }

        public void Create(Substation t)
        {
            context.Substations.Add(t);
        }              

        public Substation Read(int id)
        {
            return context.Substations
                          .Include(x => x.Attachments.Select(m => m.ManagementOrganization))
                          .Include(x => x.Attachments.Select(m => m.VoltageClass))                                                     
                          .Include(x => x.MaintenanceRecords)                          
                          .Include(x => x.AdditionalWorks.Select(m => m.MaintenanceRecords
                                                        .Select(t => t.PlannedMaintenanceType)))
                          .First(x => x.MaintainedEquipmentId == id);
        }

        public void Update(Substation t)
        {
            Substation oldSubstation = Read(t.MaintainedEquipmentId);
            UpdateEntityReflection.Update(oldSubstation, t);
            context.Entry(oldSubstation).State = EntityState.Modified;
        }

        public void Delete(Substation t)
        {
            Substation substation = Read(t.MaintainedEquipmentId);
            context.Substations.Remove(substation);
        }

        public IEnumerable<Substation> GetAll()
        {
            return context.Substations.Include(x => x.Team)
                .Include(x => x.Team)
                .Include(x => x.TransformerType)
                .Include(x => x.DistrictElectricalNetwork)
                .Include(x => x.InspectionsFrequency)                
                .ToList();
        }
    }
}
