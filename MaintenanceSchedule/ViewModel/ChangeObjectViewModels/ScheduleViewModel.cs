using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using MaintenanceSchedule.Commands;
using MaintenanceSchedule.Interfaces;
using MaintenanceSchedule.Model;
using MaintenanceSchedule.View.ChangeObjectViews;
using MaintenanceScheduleDataLayer.Entities;

namespace MaintenanceSchedule.ViewModel.ChangeObjectViewModels
{
    class ScheduleViewModel : BaseViewModel
    {
        private ObservableCollection<ScheduleRecordModel> m_scheduleRecordModels;
		private HashSet<ScheduleRecordModel> m_collection;
        private Schedule m_schedule;
		private ObservableCollection<string> months;
		private ScheduleRecordModel selectedScheduleRecordModel;
		private string selectedPlannedMaintenanceDate;
		private RelayCommand changeItem;
		private RelayCommand selectedPlannedMonth;
		private RelayCommand selectedActualMonth;
		private RelayCommand selectedActualType;
		private RelayCommand getCollection;
		private bool isNew = true;
		private bool canChangeRow = false;
		private string m_selectedManagementOrganization;
		private string m_selectedTeam;
		private string m_selectedSubstation;
		private string m_selectedAttachment;
		private string m_selectedElementBase;

		public List<string> Substations { get; set; }
		public List<string> Teams { get; set; }
		public List<string> ManagementOrganizations { get; set; }
		public List<string> Attachments { get; set; }
		public List<string> ElementBases { get; set; }		

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

		public RelayCommand GetCollection
		{
			get
			{
				return getCollection ?? new RelayCommand(o =>
				{
					FilterCollection();
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

		public string SelectedManagementOrganization
		{
			get
			{
				return m_selectedManagementOrganization;
			}
			set
			{
				m_selectedManagementOrganization = value;
				OnProtpertyChange(nameof(SelectedManagementOrganization));
			}
		}

		public string SelectedTeam
		{
			get
			{
				return m_selectedTeam;
			}
			set
			{
				m_selectedTeam = value;
				OnProtpertyChange(nameof(SelectedTeam));
			}
		}

		public string SelectedSubstation
		{
			get
			{
				return m_selectedSubstation;
			}
			set
			{
				m_selectedSubstation = value;
				OnProtpertyChange(nameof(SelectedSubstation));
			}
		}

		public string SelectedAttachment
		{
			get
			{
				return m_selectedAttachment;
			}
			set
			{
				m_selectedAttachment = value;
				OnProtpertyChange(nameof(SelectedAttachment));
			}
		}

		public string SelectedElementBase
		{
			get
			{
				return m_selectedElementBase;
			}
			set
			{
				m_selectedElementBase = value;
				OnProtpertyChange(nameof(SelectedElementBase));
			}
		}

		public ScheduleViewModel(IServiceUnitOfWork serviceUnitOfWork, ContentControl control, Schedule schedule)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
            m_schedule = schedule;
			m_collection = new HashSet<ScheduleRecordModel>(serviceUnitOfWork.ScheduleRecordModels.GetAll(schedule.Year, serviceUnitOfWork).OrderBy(x => x.Substation));
			ScheduleRecordModels = new ObservableCollection<ScheduleRecordModel>(m_collection);
			List<string> list = new List<string>();
			list.Add("");
			list.AddRange(DateTimeFormatInfo.CurrentInfo.MonthNames.Take(12));			
			ScheduleView view = new ScheduleView();
			view.DataContext = this;			
			control.Content = view;
			Months = new ObservableCollection<string>(list);
			isNew = false;
			Substations = new List<string>();
			Substations.Add("Подстанции");
			Substations.AddRange(serviceUnitOfWork.Substations.GetAll().Select(p => p.Name));
			SelectedSubstation = Substations[0];
			Teams = new List<string>();
			Teams.Add("Бригады");
			Teams.AddRange(serviceUnitOfWork.Teams.GetAll().Select(p => p.Name));
			SelectedTeam = Teams[0];
			ManagementOrganizations = new List<string>();
			ManagementOrganizations.Add("Организации управления");
			ManagementOrganizations.AddRange(serviceUnitOfWork.ManagementOrganizations.GetAll().Select(p => p.Name));
			SelectedManagementOrganization = ManagementOrganizations[0];
			Attachments = new List<string>();
			Attachments.Add("Присоединения");
			Attachments.AddRange(serviceUnitOfWork.Attachments.GetAll().Select(p => p.Name));
			SelectedAttachment = Attachments[0];
			ElementBases = new List<string>();
			ElementBases.Add("Элементные базы");
			ElementBases.AddRange(serviceUnitOfWork.ElementBases.GetAll().Select(p => p.Name));
			SelectedElementBase = ElementBases[0];
		}

		private void FilterCollection()
		{
			ScheduleRecordModels.Clear();
			List<ScheduleRecordModel> records = new List<ScheduleRecordModel>();
			foreach (ScheduleRecordModel record in m_collection)
			{
				if (m_selectedManagementOrganization != "Организации управления")
				{
					if (m_selectedManagementOrganization != record.ManagementOrganization) continue;
				}
				if (m_selectedTeam != "Бригады")
				{
					if (m_selectedTeam != record.Team) continue;
				}
				if (m_selectedSubstation != "Подстанции")
				{
					if (m_selectedSubstation != record.Substation) continue;
				}
				if (m_selectedAttachment != "Присоединения")
				{
					if (m_selectedAttachment != record.Attachment) continue;
				}
				if (m_selectedElementBase != "Элементные базы")
				{
					if (m_selectedElementBase != record.ElementBase) continue;
				}
				ScheduleRecordModels.Add(record);
			}
			
			//ScheduleRecordModels = new ObservableCollection<ScheduleRecordModel>(records);
		}
    }
}