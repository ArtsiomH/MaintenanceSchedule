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
    class AdditionalDevicesViewModel : BaseViewModel
    {
        public ObservableCollection<AdditionalDevice> additionalDevices;
        private ObservableCollection<MaintenanceRecord> maintenanceRecords;

        private AdditionalDevice selectedAdditionalDevice;
		private MaintenanceRecord sougthMaintenanceRecord;
		private RelayCommand add;
        private RelayCommand change;
        private RelayCommand delete;
        private RelayCommand mark;
		private RelayCommand reschedule;

        public ObservableCollection<AdditionalDevice> AdditionalDevices
        {
            get
            {
                return additionalDevices;
            }
            set
            {
                additionalDevices = value;
                OnProtpertyChange(nameof(AdditionalDevices));
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

        public AdditionalDevice SelectedAdditionalDevice
        {
            get
            {
                return selectedAdditionalDevice;
            }
            set
            {
                if (value == null) return;
                selectedAdditionalDevice = value;
                AdditionalDevice additionalDevice = serviceUnitOfWork.AdditionalDevices.Get(value.MaintainedEquipmentId);
                MaintenanceRecords = new ObservableCollection<MaintenanceRecord>(additionalDevice.MaintenanceRecords);
                sougthMaintenanceRecord = selectedAdditionalDevice.MaintenanceRecords.FirstOrDefault(x => x.ActualMaintenanceDate == null && x.IsPlanned == true && x.IsRescheduled == false);               
                OnProtpertyChange(nameof(SelectedAdditionalDevice));
            }
        }

        public RelayCommand Add
        {
            get
            {
                return add ?? (add = new RelayCommand(o =>
                {
                    AdditionalDevice additionalDevice = new AdditionalDevice();
                    AdditionalDeviceView view = new AdditionalDeviceView();
                    view.DataContext = new AdditionalDeviceViewModel(serviceUnitOfWork, additionalDevice, ActionType.Create);
                    if (view.ShowDialog() == true)
                    {
                        serviceUnitOfWork.AdditionalDevices.Create(additionalDevice);
                        AdditionalDevices = serviceUnitOfWork.AdditionalDevices.GetAll();
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
                    if (selectedAdditionalDevice.Name == null) return;
                    AdditionalDeviceView view = new AdditionalDeviceView();
                    view.DataContext = new AdditionalDeviceViewModel(serviceUnitOfWork, selectedAdditionalDevice, ActionType.Update);
                    if (view.ShowDialog() == true)
                    {
                        serviceUnitOfWork.AdditionalDevices.Update(selectedAdditionalDevice);
                        AdditionalDevices = serviceUnitOfWork.AdditionalDevices.GetAll();
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

                }));
            }
        }

        public RelayCommand Mark
        {
            get
            {
                return mark ?? (mark = new RelayCommand(o =>
                {
                    if (SelectedAdditionalDevice == null) return;
                    MarkRecordView view = new MarkRecordView();
                    view.DataContext = new MarkRecordViewModel(serviceUnitOfWork, SelectedAdditionalDevice);
                    if (view.ShowDialog() == true)
                    {
                        SelectedAdditionalDevice = SelectedAdditionalDevice;
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
					if (SelectedAdditionalDevice == null) return;
					serviceUnitOfWork.AdditionalDevices.RescheduleRecord(SelectedAdditionalDevice, sougthMaintenanceRecord);
					SelectedAdditionalDevice = serviceUnitOfWork.AdditionalDevices.Get(selectedAdditionalDevice.MaintainedEquipmentId);
				},
				o => sougthMaintenanceRecord != null));
			}
		}

		public AdditionalDevicesViewModel(IServiceUnitOfWork serviceUnitOfWork)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;            
            AdditionalDevices = serviceUnitOfWork.AdditionalDevices.GetAll();
        }
    }
}
