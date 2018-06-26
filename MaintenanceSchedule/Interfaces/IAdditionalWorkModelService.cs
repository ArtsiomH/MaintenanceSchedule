using MaintenanceSchedule.Model;
using MaintenanceScheduleDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceSchedule.Interfaces
{
    interface IAdditionalWorkModelService
    {
        ObservableCollection<AdditionalWorkModel> GetAll(Substation substation);
        ObservableCollection<AdditionalWorkModel> GetAll(RelayDevice device);
    }
}
