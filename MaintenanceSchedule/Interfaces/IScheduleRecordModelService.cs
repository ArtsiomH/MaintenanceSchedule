using System.Collections.ObjectModel;
using MaintenanceSchedule.Model;

namespace MaintenanceSchedule.Interfaces
{
    interface IScheduleRecordModelService
    {
        ObservableCollection<ScheduleRecordModel> GetAll(int year, IServiceUnitOfWork serviceUnitOfWork);
		void SetPlannedMonth(ScheduleRecordModel scheduleRecordModel, string month);
		void SetActualMonth(ScheduleRecordModel scheduleRecordModel, string month);
		void SetActualType(ScheduleRecordModel scheduleRecordModel, string type);
	}
}
