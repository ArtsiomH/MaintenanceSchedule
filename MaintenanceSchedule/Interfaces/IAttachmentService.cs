using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaintenanceSchedule.Model;
using MaintenanceScheduleDataLayer.Entities;
using System.Collections.ObjectModel;

namespace MaintenanceSchedule.Interfaces
{
    interface IAttachmentService : IBaseService<Attachment>
    {
        ObservableCollection<Attachment> Find(Func<Attachment, bool> predicate);
    }
}
