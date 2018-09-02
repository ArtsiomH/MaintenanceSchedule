using System.Collections.ObjectModel;
using MaintenanceSchedule.Interfaces;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Entities;

namespace MaintenanceSchedule.Services
{
    class RelayDeviceService : BaseDeviceService, IRelayDeviceService
    {
        public RelayDeviceService(IUnitOfWork dataBase) : base(dataBase)
        {   }

        public void Create(RelayDevice t)
        {
            CreateRecords(t);           
            dataBase.RelayDevices.Create(t);
            dataBase.Save();
        }

        public void Delete(RelayDevice t)
        {
            dataBase.Devices.Delete(t);
            dataBase.Save();
        }

        public RelayDevice Get(int id)
        {
            return dataBase.RelayDevices.Read(id);
        }

        public ObservableCollection<RelayDevice> GetAll()
        {
            return new ObservableCollection<RelayDevice>(dataBase.RelayDevices.GetAll());
        }

        public void Update(RelayDevice t)
        {
            UpdateRecords(t);
            dataBase.RelayDevices.Update(t);
            dataBase.Save();
        }        
    }
}