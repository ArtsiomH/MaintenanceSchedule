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
    class MaintenanceRecordService : IMaintenanceRecordService
    {
        IUnitOfWork dataBase;

        public MaintenanceRecordService(IUnitOfWork dataBase)
        {
            this.dataBase = dataBase;
        }

        public void Create(MaintenanceRecord t)
        {
            throw new NotImplementedException();
        }

        public void Delete(MaintenanceRecord t)
        {
            throw new NotImplementedException();
        }

        public MaintenanceRecord Get(int id)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<MaintenanceRecord> GetAll()
        {
            return new ObservableCollection<MaintenanceRecord>(dataBase.MaintenanceRecords.GetAll());
        }

        public void Update(MaintenanceRecord t)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<MaintenanceRecord> Find(Func<MaintenanceRecord, bool> predicate)
        {
            return new ObservableCollection<MaintenanceRecord>(dataBase.MaintenanceRecords.GetAll().Where(predicate));
        }
    }
}
