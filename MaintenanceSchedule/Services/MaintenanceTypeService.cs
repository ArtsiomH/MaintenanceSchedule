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
    class MaintenanceTypeService : IMaintenanceTypeService
    {
        IUnitOfWork dataBase;

        public MaintenanceTypeService(IUnitOfWork dataBase)
        {
            this.dataBase = dataBase;
        }

        public void Create(MaintenanceType t)
        {
            dataBase.MaintenanceTypes.Create(t);
            dataBase.Save();
        }

        public void Delete(MaintenanceType t)
        {
            throw new NotImplementedException();
        }

        public MaintenanceType Get(int id)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<MaintenanceType> GetAll()
        {
            return new ObservableCollection<MaintenanceType>(dataBase.MaintenanceTypes.GetAll());
        }

        public void Update(MaintenanceType t)
        {
            throw new NotImplementedException();
        }
    }
}
