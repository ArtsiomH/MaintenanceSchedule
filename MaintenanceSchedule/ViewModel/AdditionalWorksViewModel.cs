using MaintenanceSchedule.Commands;
using MaintenanceSchedule.Enums;
using MaintenanceSchedule.Interfaces;
using MaintenanceSchedule.Model;
using MaintenanceSchedule.View;
using MaintenanceSchedule.View.ChangeObjectViews;
using MaintenanceSchedule.ViewModel.ChangeObjectViewModels;
using MaintenanceScheduleDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceSchedule.ViewModel
{
    class AdditionalWorksViewModel : BaseViewModel
    {
        public ObservableCollection<AdditionalWork> additionalWorks;
        private ObservableCollection<MaintenanceRecord> maintenanceRecords;
        private ObservableCollection<RelayDeviceModel> relaydeviceModels;

        private AdditionalWork selectedAdditionalWork;
		private MaintenanceRecord sougthMaintenanceRecord;
		private RelayCommand add;
        private RelayCommand change;
        private RelayCommand delete;
        private RelayCommand mark;
		private RelayCommand reschedule;

        public ObservableCollection<AdditionalWork> AdditionalWorks
        {
            get
            {
                return additionalWorks;
            }
            set
            {
                additionalWorks = value;
                OnProtpertyChange(nameof(additionalWorks));
            }
        }

        public ObservableCollection<MaintenanceRecord> MaintenanceRecords
        {
            get
            {
                return maintenanceRecords;
            }
            set
            {
                maintenanceRecords = value;
                OnProtpertyChange(nameof(MaintenanceRecords));
            }
        }

        public ObservableCollection<RelayDeviceModel> RelayDeviceModels
        {
            get
            {
                return relaydeviceModels;
            }
            set
            {
                relaydeviceModels = value;
                OnProtpertyChange(nameof(RelayDeviceModels));
            }
        }

        public AdditionalWork SelectedAdditionalWork
        {
            get
            {
                return selectedAdditionalWork;
            }
            set
            {
                if (value == null) return;
                selectedAdditionalWork = serviceUnitOfWork.AdditionalWorks.Get(value.MaintainedEquipmentId); 
                MaintenanceRecords = new ObservableCollection<MaintenanceRecord>(selectedAdditionalWork.MaintenanceRecords);                
                RelayDeviceModels = serviceUnitOfWork.RelayDeviceModels.GetAll(SelectedAdditionalWork);
				sougthMaintenanceRecord = selectedAdditionalWork.MaintenanceRecords.FirstOrDefault(x => x.ActualMaintenanceDate == null && x.IsPlanned == true && x.IsRescheduled == false);
                OnProtpertyChange(nameof(SelectedAdditionalWork));
            }
        }

        public RelayCommand Add
        {
            get
            {
                return add ?? (add = new RelayCommand(o =>
                {
                    AdditionalWork additionalWork = new AdditionalWork();
                    AdditionalWorkView view = new AdditionalWorkView();
                    view.DataContext = new AdditionalWorkViewModel(serviceUnitOfWork, additionalWork, ActionType.Create);
                    if (view.ShowDialog() == true)
                    {                        
                        additionalWorks = serviceUnitOfWork.AdditionalWorks.GetAll();
                    }
                }));
            }
        }

        public RelayCommand Change
        {
            get
            {
                return change ?? (change = new RelayCommand(o =>
                {
                    if (SelectedAdditionalWork == null) return;
                    AdditionalWorkView view = new AdditionalWorkView();
                    view.DataContext = new AdditionalWorkViewModel(serviceUnitOfWork, selectedAdditionalWork, ActionType.Update);
                    if (view.ShowDialog() == true)
                    {                        
                        additionalWorks = serviceUnitOfWork.AdditionalWorks.GetAll();
                    }
                }));
            }
        }

        public RelayCommand Delete
        {
            get
            {
                return delete ?? (delete = new RelayCommand(o =>
                {
                    if (SelectedAdditionalWork == null) return;
                    serviceUnitOfWork.AdditionalWorks.Delete(selectedAdditionalWork);
                    additionalWorks = serviceUnitOfWork.AdditionalWorks.GetAll();
                    MaintenanceRecords = null;
                    RelayDeviceModels = null;                   
                }));
            }
        }

        public RelayCommand Mark
        {
            get
            {
                return mark ?? (mark = new RelayCommand(o =>
                {
                    if (SelectedAdditionalWork == null) return;
                    MarkRecordView view = new MarkRecordView();
                    view.DataContext = new MarkRecordViewModel(serviceUnitOfWork, SelectedAdditionalWork);
                    if (view.ShowDialog() == true)
                    {
                        SelectedAdditionalWork = SelectedAdditionalWork;
                    }
                },
                o => sougthMaintenanceRecord != null));
            }
        }

		public RelayCommand Reschsedule
		{
			get
			{
				return reschedule ?? (reschedule = new RelayCommand(o =>
				{
					if (SelectedAdditionalWork == null) return;
					serviceUnitOfWork.AdditionalWorks.RescheduleRecord(SelectedAdditionalWork, sougthMaintenanceRecord);
					SelectedAdditionalWork = serviceUnitOfWork.AdditionalWorks.Get(selectedAdditionalWork.MaintainedEquipmentId);
				},
				o => sougthMaintenanceRecord != null));
			}
		}

		public AdditionalWorksViewModel(IServiceUnitOfWork serviceUnitOfWork)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
            additionalWorks = serviceUnitOfWork.AdditionalWorks.GetAll();
        }
    }
}