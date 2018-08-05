using System.Collections.ObjectModel;
using MaintenanceSchedule.Model;

namespace MaintenanceSchedule.Interfaces
{
    interface IScheduleRecordModelService
    {
        ObservableCollection<ScheduleRecordModel> GetAll(int year, IServiceUnitOfWork serviceUnitOfWork);
    }
}
