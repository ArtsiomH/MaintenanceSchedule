using MaintenanceSchedule.Interfaces;
using MaintenanceSchedule.Model;
using MaintenanceScheduleDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MaintenanceSchedule.ViewModel
{
    class ScheduleViewModel : BaseViewModel
    {
        private ObservableCollection<ScheduleRecordModel> m_scheduleRecordModels;
        private Schedule m_schedule;

        public ObservableCollection<ScheduleRecordModel> ScheduleRecordModels
        {
            get
            {
                return m_scheduleRecordModels;
            }

            set
            {
                m_scheduleRecordModels = value;
                OnProtpertyChange(nameof(ScheduleRecordModels));
            }
        }

        public ScheduleViewModel(IServiceUnitOfWork serviceUnitOfWork, ContentControl control, Schedule schedule)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
            m_schedule = schedule;

            ScheduleRecordModels = serviceUnitOfWork.ScheduleRecordModels.GetAll(schedule.Year, serviceUnitOfWork);
            control = new View.AdditionalDevicesView();

        }
    }
}
