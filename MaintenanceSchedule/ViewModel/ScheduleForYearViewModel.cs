using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MaintenanceSchedule.Interfaces;
using System.Collections.ObjectModel;
using MaintenanceScheduleDataLayer.Entities;
using MaintenanceSchedule.Commands;
using MaintenanceSchedule.ViewModel.ChangeObjectViewModels;
using System.Windows;

namespace MaintenanceSchedule.ViewModel
{
    /// <summary>
    /// ViewModel для представления списка графиков
    /// </summary>
    class ScheduleForYearViewModel : BaseViewModel
    {
        private ObservableCollection<Schedule> m_schedules;
        private Schedule m_selectedSchedule;
        private ContentControl m_contentControl;

        private RelayCommand m_open;
        private RelayCommand m_create;
        private RelayCommand m_delete;

        public Schedule SelectedSchedule
        {
            get
            {
                return m_selectedSchedule;
            }

            set
            {
                m_selectedSchedule = value;
                OnProtpertyChange(nameof(SelectedSchedule));
            }
        }

        public ObservableCollection<Schedule> Schedules
        {
            get
            {
                return m_schedules;
            }

            set
            {
                m_schedules = value;
                OnProtpertyChange(nameof(Schedules));
            }
        }

        public RelayCommand Open
        {
            get
            {
                return m_open ?? (m_open = new RelayCommand(o =>
                {
                    ScheduleViewModel viewModel = new ScheduleViewModel(serviceUnitOfWork, m_contentControl, m_selectedSchedule);
					((Window)o).DialogResult = true;
				}));
            }
        }

        public RelayCommand Create
        {
            get
            {
                return m_create ?? (m_create = new RelayCommand(o =>
                {
					serviceUnitOfWork.Schedules.Create(new Schedule());
                    Schedules = serviceUnitOfWork.Schedules.GetAll(); 
                }));
            }
        }

        public RelayCommand Delete
        {
            get
            {
                return m_delete ?? (m_delete = new RelayCommand(o =>
                {
                    serviceUnitOfWork.Schedules.Delete(m_selectedSchedule);
                    Schedules = serviceUnitOfWork.Schedules.GetAll();
                }));
            }
        }

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="serviceUnitOfWork">общий интерфейс сервиса</param>
        public ScheduleForYearViewModel(IServiceUnitOfWork serviceUnitOfWork, ContentControl content)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
            m_contentControl = content;

            Schedules = serviceUnitOfWork.Schedules.GetAll();
            if (Schedules == null)
            {
                Schedules = new ObservableCollection<Schedule>();
            }
        }
    }
}
