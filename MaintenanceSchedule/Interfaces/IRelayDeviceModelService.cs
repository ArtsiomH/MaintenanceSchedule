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
    interface IRelayDeviceModelService
    {
        RelayDeviceModel Get(int relayDeviceId);
        ObservableCollection<RelayDeviceModel> GetAll(Attachment attachment);
        ObservableCollection<RelayDeviceModel> GetAll(AdditionalWork combineDevice);
    }
}
