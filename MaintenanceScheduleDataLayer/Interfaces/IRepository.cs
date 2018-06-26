using System.Collections.Generic;

namespace MaintenanceScheduleDataLayer.Interfaces
{
    public interface IRepository<T>
    {
        void Create(T t);
        T Read(int id);
        void Update(T t);
        void Delete(T t);
        IEnumerable<T> GetAll();
    }
}
