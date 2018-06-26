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

namespace MaintenanceSchedule.Services
{
    class DeviceTypeService : IDeviceTypeService
    {
        IUnitOfWork dataBase;

        public DeviceTypeService(IUnitOfWork dataBase)
        {
            this.dataBase = dataBase;
        }

        public void Create(DeviceType t)
        {
            throw new NotImplementedException();
        }

        public void Delete(DeviceType t)
        {
            throw new NotImplementedException();
        }

        public DeviceType Get(int id)
        {
            return dataBase.DeviceTypes.Read(id);
        }

        public ObservableCollection<DeviceType> GetAll()
        {
            return new ObservableCollection<DeviceType>(dataBase.DeviceTypes.GetAll());
        }

        public void Update(DeviceType t)
        {
            throw new NotImplementedException();
        }
    }
}
