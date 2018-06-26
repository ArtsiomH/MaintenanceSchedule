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
    interface ISubstationService : IBaseService<Substation>
    {
        void MarkActualRecord(Substation substation, DateTime date);
    }
}
