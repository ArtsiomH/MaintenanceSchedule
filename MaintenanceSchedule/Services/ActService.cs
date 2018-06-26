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
    class ActService : IActService
    {
        IUnitOfWork dataBase;

        public ActService(IUnitOfWork dataBase)
        {
            this.dataBase = dataBase;
        }

        public void Create(Act t)
        {
            dataBase.Acts.Create(t);
        }

        public void Delete(Act t)
        {
            dataBase.Acts.Delete(t);
        }

        public Act Get(int id)
        {
            return dataBase.Acts.Read(id);
        }

        public ObservableCollection<Act> GetAll()
        {
            return new ObservableCollection<Act>(dataBase.Acts.GetAll());
        }

        public void Update(Act t)
        {
            dataBase.Acts.Update(t);
        }
    }
}
