using MaintenanceSchedule.Commands;
using MaintenanceSchedule.Interfaces;
using MaintenanceSchedule.Model;
using MaintenanceSchedule.View.ChangeObjectViews;
using MaintenanceScheduleDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MaintenanceSchedule.ViewModel.ChangeObjectViewModels
{
    class ScheduleViewModel : BaseViewModel
    {
        private ObservableCollection<ScheduleRecordModel> m_scheduleRecordModels;
        private Schedule m_schedule;
		private ObservableCollection<string> months;
		private ScheduleRecordModel selectedScheduleRecordModel;
		private string selectedPlannedMaintenanceDate;
		private RelayCommand changeItem;
		private RelayCommand selectedPlannedMonth;
		private RelayCommand selectedActualMonth;
		private RelayCommand selectedActualType;
		private bool isNew = true;
		private bool canChangeRow = false;

		public RelayCommand SelectedPlannedMonth
		{
			get
			{
				return selectedPlannedMonth ?? new RelayCommand(o =>
				{
					string month = o as string;
					if (canChangeRow
						//&& !string.IsNullOrEmpty(month) 
						&& (string.IsNullOrEmpty(selectedScheduleRecordModel.PlannedMonth)
							|| selectedScheduleRecordModel.PlannedMonth != month))
					{ 
						serviceUnitOfWork.ScheduleRecordModels.SetPlannedMonth(selectedScheduleRecordModel, month);
						selectedScheduleRecordModel.PlannedMonth = month;
						List<ScheduleRecordModel> records = m_scheduleRecordModels.ToList();
						ScheduleRecordModels = null;
						ScheduleRecordModels = new ObservableCollection<ScheduleRecordModel>(records);
						OnProtpertyChange(nameof(SelectedScheduleRecordModel));
						OnProtpertyChange(nameof(SelectedScheduleRecordModel.IsPlanned));
					}								
				});
			}
		}

		public RelayCommand SelectedActualMonth
		{
			get
			{
				return selectedActualMonth ?? new RelayCommand(o =>
				{
					string month = o as string;
					if (canChangeRow
						&& !string.IsNullOrEmpty(month)
						&& (string.IsNullOrEmpty(selectedScheduleRecordModel.ActualMonth)
							|| selectedScheduleRecordModel.ActualMonth != month))
					{
						serviceUnitOfWork.ScheduleRecordModels.SetActualMonth(selectedScheduleRecordModel, month);
						OnProtpertyChange(nameof(SelectedScheduleRecordModel));
					}
				});
			}
		}

		public RelayCommand SelectedActualType
		{
			get
			{
				return selectedActualType ?? new RelayCommand(o =>
				{
					string type = o as string;
					if (canChangeRow
						&& !string.IsNullOrEmpty(type)
						&& (selectedScheduleRecordModel.ActualMaintenanceType == null
							|| selectedScheduleRecordModel.ActualMaintenanceType.Name != type))
					{
						serviceUnitOfWork.ScheduleRecordModels.SetActualType(selectedScheduleRecordModel, type);
						OnProtpertyChange(nameof(SelectedScheduleRecordModel));
					}
				});
			}
		}

		public RelayCommand ChangeItem
		{
			get
			{
				return changeItem ?? new RelayCommand(o =>
				{
					ScheduleRecordModel record = o as ScheduleRecordModel;
					if (record != null
						&& selectedScheduleRecordModel != null
						&& record == selectedScheduleRecordModel)
					{
						canChangeRow = true;
					}
					else canChangeRow = false;
				});
			}
		}

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

		public ScheduleRecordModel SelectedScheduleRecordModel
		{
			get
			{

				return selectedScheduleRecordModel;
			}
			set
			{
				selectedScheduleRecordModel = value;
				OnProtpertyChange(nameof(SelectedScheduleRecordModel));
			}
		}

		public ObservableCollection<string> Months
		{
			get
			{
				return months;
			}
			set
			{
				months = value;
				OnProtpertyChange(nameof(Months));
			}
		}

		public ScheduleViewModel(IServiceUnitOfWork serviceUnitOfWork, ContentControl control, Schedule schedule)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
            m_schedule = schedule;

            ScheduleRecordModels = new ObservableCollection<ScheduleRecordModel>(serviceUnitOfWork.ScheduleRecordModels.GetAll(schedule.Year, serviceUnitOfWork).OrderBy(x => x.Substation));
			List<string> list = new List<string>();
			list.Add("");
			list.AddRange(DateTimeFormatInfo.CurrentInfo.MonthNames.Take(12));			
			ScheduleView view = new ScheduleView();
			view.DataContext = this;			
			control.Content = view;
			Months = new ObservableCollection<string>(list);
			isNew = false;
		}
    }
}
