using System;
using System.Collections.ObjectModel;
using MaintenanceScheduleDataLayer.Entities;

namespace MaintenanceSchedule.Interfaces
{
    interface IBaseService<T> where T : class
    {
        void Create(T t);
        void Delete(T t);
        T Get(int id);
        ObservableCollection<T> GetAll();      
        void Update(T t);        
    }
}
