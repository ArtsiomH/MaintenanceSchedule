using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaintenanceSchedule.Interfaces;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Repositories;
using MaintenanceScheduleDataLayer.Entities;
using MaintenanceSchedule.Model;

namespace MaintenanceSchedule.Services
{
    class AttachmentService : IAttachmentService
    {
        IUnitOfWork dataBase;

        public AttachmentService(IUnitOfWork dataBase)
        {
            this.dataBase = dataBase;
        }

        public void Create(Attachment t)
        {
           dataBase.Attachments.Create(t);
        }

        public void Delete(Attachment t)
        {
            dataBase.Attachments.Delete(t);
        }

        public Attachment Get(int id)
        {
            return dataBase.Attachments.Read(id);
        }

        public ObservableCollection<Attachment> GetAll()
        {
            return new ObservableCollection<Attachment>(dataBase.Attachments.GetAll());
        }

        public void Update(Attachment t)
        {
            dataBase.Attachments.Update(t);
        }

        public ObservableCollection<Attachment> Find(Func<Attachment, bool> predicate)
        {
            return new ObservableCollection<Attachment>(dataBase.Attachments.GetAll().Where(predicate));
        }        
    }
}
