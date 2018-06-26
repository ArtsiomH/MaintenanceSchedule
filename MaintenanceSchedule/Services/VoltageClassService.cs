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
    class VoltageClassService : IVoltageClassService
    {
        IUnitOfWork dataBase;

        public VoltageClassService(IUnitOfWork dataBase)
        {
            this.dataBase = dataBase;
        }

        public void Create(VoltageClass t)
        {
            throw new NotImplementedException();
        }

        public void Delete(VoltageClass t)
        {
            throw new NotImplementedException();
        }

        public VoltageClass Get(int id)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<VoltageClass> GetAll()
        {
            return new ObservableCollection<VoltageClass>(dataBase.VoltageClasses.GetAll());
        }

        public void Update(VoltageClass t)
        {
            throw new NotImplementedException();
        }
    }
}
