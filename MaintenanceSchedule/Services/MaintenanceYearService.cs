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
    class MaintenanceYearService : IMaintenanceYearService
    {
        IUnitOfWork dataBase;

        public MaintenanceYearService(IUnitOfWork dataBase)
        {
            this.dataBase = dataBase;
        }

        public void Create(MaintenanceYear t)
        {
            throw new NotImplementedException();
        }

        public void Delete(MaintenanceYear t)
        {
            throw new NotImplementedException();
        }

        public MaintenanceYear Get(int id)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<MaintenanceYear> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(MaintenanceYear t)
        {
            throw new NotImplementedException();
        }
    }
}
