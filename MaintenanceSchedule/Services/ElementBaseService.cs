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
    class ElementBaseService : IElementBaseService
    {
        IUnitOfWork dataBase;

        public ElementBaseService(IUnitOfWork dataBase)
        {
            this.dataBase = dataBase;
        }

        public void Create(ElementBase t)
        {
            throw new NotImplementedException();
        }

        public void Delete(ElementBase t)
        {
            throw new NotImplementedException();
        }

        public ElementBase Get(int id)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<ElementBase> GetAll()
        {
            return new ObservableCollection<ElementBase>(dataBase.ElementBases.GetAll());
        }

        public void Update(ElementBase t)
        {
            throw new NotImplementedException();
        }
    }
}
