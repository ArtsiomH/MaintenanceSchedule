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
    class InspectionsFrequencyService : IInspectionsFrequencyService
    {
        IUnitOfWork dataBase;

        public InspectionsFrequencyService(IUnitOfWork dataBase)
        {
            this.dataBase = dataBase;
        }

        public void Create(InspectionsFrequency t)
        {
            throw new NotImplementedException();
        }

        public void Delete(InspectionsFrequency t)
        {
            throw new NotImplementedException();
        }

        public InspectionsFrequency Get(int id)
        {            
            return dataBase.InspectionsFrequencies.Read(id);
        }

        public ObservableCollection<InspectionsFrequency> GetAll()
        {
            return new ObservableCollection<InspectionsFrequency>(dataBase.InspectionsFrequencies.GetAll());
        }

        public void Update(InspectionsFrequency t)
        {
            throw new NotImplementedException();
        }
    }
}
