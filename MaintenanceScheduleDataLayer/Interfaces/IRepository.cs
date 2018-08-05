using System.Collections.Generic;
using System.Threading.Tasks;

namespace MaintenanceScheduleDataLayer.Interfaces
{
    public interface IRepository<T>
    {
        void Create(T t);
        T Read(int id);
        void Update(T t);
        void Delete(T t);
        IEnumerable<T> GetAll();
        Task<T> ReadAsync(int id);        
    }
}
