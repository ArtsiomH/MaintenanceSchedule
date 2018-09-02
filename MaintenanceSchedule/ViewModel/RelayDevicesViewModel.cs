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
    class RelayDevicesViewModel : BaseViewModel
    {
        public ObservableCollection<RelayDevice> relayDevices;
        private ObservableCollection<MaintenanceRecord> maintenanceRecords;
        private ObservableCollection<AdditionalWorkModel> additionalDeviceModels;
        
        private RelayDevice selectedRelayDevice;
		private MaintenanceRecord sougthMaintenanceRecord;
        private RelayCommand add;
        private RelayCommand change;
        private RelayCommand delete;
        private RelayCommand mark;
		private RelayCommand reschedule;
		private bool canMark = false;

        public ObservableCollection<RelayDevice> RelayDevices
        {
            get
            {
                return relayDevices;
            }
            set
            {
                relayDevices = value;
                OnProtpertyChange(nameof(RelayDevices));
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

        private ObservableCollection<AdditionalWorkModel> AdditionalDeviceModels
        {
            get
            {
                return additionalDeviceModels;
            }
            set
            {
                additionalDeviceModels = value;
                OnProtpertyChange(nameof(AdditionalDeviceModels));
            }
        }

        public RelayDevice SelectedRelayDevice
        {
            get
            {
                return selectedRelayDevice;
            }
            set
            {
                if (value == null) return;
                selectedRelayDevice = value;
                RelayDevice relayDevice = serviceUnitOfWork.RelayDevices.Get(SelectedRelayDevice.MaintainedEquipmentId);
                MaintenanceRecords = new ObservableCollection<MaintenanceRecord>(relayDevice.MaintenanceRecords);
                AdditionalDeviceModels = serviceUnitOfWork.AdditionalWorkModels.GetAll(selectedRelayDevice);
                sougthMaintenanceRecord = selectedRelayDevice.MaintenanceRecords.FirstOrDefault(x => x.ActualMaintenanceDate == null && x.IsPlanned == true && x.IsRescheduled == false);                
                OnProtpertyChange(nameof(SelectedRelayDevice));
            }
        }

        public RelayCommand Add
        {
            get
            {
                return add ?? (add = new RelayCommand(o =>
                {
                    RelayDevice relayDevice = new RelayDevice();
                    RelayDeviceView view = new RelayDeviceView();
                    view.DataContext = new RelayDeviceViewModel(serviceUnitOfWork, relayDevice, ActionType.Create);
                    if (view.ShowDialog() == true)
                    {
                        RelayDevices = serviceUnitOfWork.RelayDevices.GetAll();
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
                    if (selectedRelayDevice == null) return;
                    RelayDeviceView view = new RelayDeviceView();
                    view.DataContext = new RelayDeviceViewModel(serviceUnitOfWork, selectedRelayDevice, ActionType.Update);
                   
                    if (view.ShowDialog() == true)
                    {
                        RelayDevices = serviceUnitOfWork.RelayDevices.GetAll();
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
                    if (selectedRelayDevice == null) return;
                    serviceUnitOfWork.RelayDevices.Delete(selectedRelayDevice);
                    RelayDevices = serviceUnitOfWork.RelayDevices.GetAll();
                    
                    SelectedRelayDevice = new RelayDevice();
                }));
            }
        } 
        
        public RelayCommand Mark
        {
            get
            {
                return mark ?? (mark = new RelayCommand(o =>
                {
                    if (SelectedRelayDevice == null) return;
                    MarkRecordView view = new MarkRecordView();
                    view.DataContext = new MarkRecordViewModel(serviceUnitOfWork, SelectedRelayDevice);
                    if (view.ShowDialog() == true)
                    {
                        SelectedRelayDevice = SelectedRelayDevice;
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
					if (SelectedRelayDevice == null) return;
					serviceUnitOfWork.RelayDevices.RescheduleRecord(SelectedRelayDevice, sougthMaintenanceRecord);
					SelectedRelayDevice = serviceUnitOfWork.RelayDevices.Get(selectedRelayDevice.MaintainedEquipmentId);
				},
				o => sougthMaintenanceRecord != null));
			}
		}

		public RelayDevicesViewModel(IServiceUnitOfWork serviceUnitOfWork)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
            RelayDevices = serviceUnitOfWork.RelayDevices.GetAll();
        }
    }
}