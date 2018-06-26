using System.Collections.ObjectModel;
using MaintenanceSchedule.Interfaces;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Entities;

namespace MaintenanceSchedule.Services
{
    class AdditionalDeviceService : BaseDeviceService, IAdditionalDeviceService
    {
        public AdditionalDeviceService(IUnitOfWork dataBase) : base(dataBase)
        {    }

        public void Create(AdditionalDevice t)
        {
            CreateRecords(t);
            dataBase.AdditionalDevices.Create(t);
            dataBase.Save();
        }

        public void Delete(AdditionalDevice t)
        {
            dataBase.AdditionalDevices.Delete(t);
        }

        public AdditionalDevice Get(int id)
        {
            return dataBase.AdditionalDevices.Read(id);
        }

        public ObservableCollection<AdditionalDevice> GetAll()
        {
            return new ObservableCollection<AdditionalDevice>(dataBase.AdditionalDevices.GetAll());
        }

        public void Update(AdditionalDevice t)
        {
            UpdateRecords(t);
            dataBase.AdditionalDevices.Update(t);
            dataBase.Save();
        }
    }
}
