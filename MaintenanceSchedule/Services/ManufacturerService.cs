using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaintenanceSchedule.Interfaces;
using MaintenanceScheduleDataLayer.Repositories;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Entities;

namespace MaintenanceSchedule.Services
{
    class ManufacturerService : IManufacturerService
    {
        IUnitOfWork dataBase;

        public ManufacturerService(IUnitOfWork dataBase)
        {
            this.dataBase = dataBase;
        }

        public void Create(Manufacturer t)
        {
            throw new NotImplementedException();
        }

        public void Delete(Manufacturer t)
        {
            throw new NotImplementedException();
        }

        public Manufacturer Get(int id)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Manufacturer> GetAll()
        {
            return new ObservableCollection<Manufacturer>(dataBase.Manufacturers.GetAll());
        }

        public void Update(Manufacturer t)
        {
            throw new NotImplementedException();
        }
    }
}
