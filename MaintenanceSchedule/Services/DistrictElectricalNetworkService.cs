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
    class DistrictElectricalNetworkService : IDistrictElectricalNetworkService
    {
        IUnitOfWork dataBase;

        public DistrictElectricalNetworkService(IUnitOfWork dataBase)
        {
            this.dataBase = dataBase;
        }

        public void Create(DistrictElectricalNetwork t)
        {
            dataBase.DistricElectricalNetworks.Create(t);
            dataBase.Save();
        }

        public void Delete(DistrictElectricalNetwork t)
        {
            dataBase.DistricElectricalNetworks.Delete(t);
            dataBase.Save();
        }

        public DistrictElectricalNetwork Get(int id)
        {
            return dataBase.DistricElectricalNetworks.Read(id);
        }

        public ObservableCollection<DistrictElectricalNetwork> GetAll()
        {
            return new ObservableCollection<DistrictElectricalNetwork>(dataBase.DistricElectricalNetworks.GetAll());
        }

        public void Update(DistrictElectricalNetwork t)
        {
            dataBase.DistricElectricalNetworks.Update(t);
            dataBase.Save();
        }
    }
}
