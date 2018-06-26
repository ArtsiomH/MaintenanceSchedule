using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Entities;
using MaintenanceScheduleDataLayer.EFContext;

namespace MaintenanceScheduleDataLayer.Repositories
{
    class AttachmentRepository : IRepository<Attachment>
    {
        private MaintenanceScheduleContext context;

        public AttachmentRepository(MaintenanceScheduleContext context)
        {
            this.context = context;
        }

        public void Create(Attachment t)
        {
            context.Attachments.Add(t);
        }

        public Attachment Read(int id)
        {
            return context.Attachments
                .Include(x => x.RelayDevices.Select(m => m.ElementBase))
                .Include(x => x.RelayDevices.Select(m => m.MaintenanceRecords
                                       .Select(t => t.PlannedMaintenanceType)))
                .Include(x => x.RelayDevices)                       
                .Include(x => x.AdditionalDevices.Select(m => m.MaintenanceRecords
                                                 .Select(t => t.PlannedMaintenanceType)))
                .First(x => x.AttachmentId == id);
        }

        public void Update(Attachment t)
        {
            Attachment oldAttachment = Read(t.AttachmentId);
            UpdateEntityReflection.Update(oldAttachment, t);
            context.Entry(oldAttachment).State = EntityState.Modified;
        }

        public void Delete(Attachment t)
        {
            Attachment attachment = Read(t.AttachmentId);
            context.Attachments.Remove(attachment);
        }

        public IEnumerable<Attachment> GetAll()
        {
            return context.Attachments
                .Include(x => x.Substation)
                .Include(x => x.VoltageClass)
                .Include(x => x.ManagementOrganization)                              
                .ToList();
        }
    }
}
