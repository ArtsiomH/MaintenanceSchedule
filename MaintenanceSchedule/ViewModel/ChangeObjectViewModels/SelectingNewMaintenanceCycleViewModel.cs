using MaintenanceSchedule.Commands;
using MaintenanceSchedule.Interfaces;
using MaintenanceScheduleDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MaintenanceSchedule.ViewModel.ChangeObjectViewModels
{
    class SelectingNewMaintenanceCycleViewModel : BaseViewModel, IDataErrorInfo
    {
        private ObservableCollection<MaintenanceCycle> maintenanceCycles;
        private MaintenanceCycle oldMaintenanceCycle;
        private MaintenanceCycle maintenanceCycle;
        private bool check = false;
        private RelayCommand save;
        private RelayCommand cancel;

        public ObservableCollection<MaintenanceCycle> MaintenanceCycles
        {
            get
            {
                return maintenanceCycles;
            }
            set
            {
                maintenanceCycles = value;
                OnProtpertyChange(nameof(MaintenanceCycles));
            }
        }

        public MaintenanceCycle MaintenanceCycle
        {
            get
            {
                return maintenanceCycle;
            }
            set
            {
                maintenanceCycle = value;
                OnProtpertyChange(nameof(MaintenanceCycle));
            }
        }

        public RelayCommand Save
        {
            get
            {
                return save ?? (save = new RelayCommand(o =>
                {
                    serviceUnitOfWork.MaintenanceCycles.Delete(oldMaintenanceCycle, maintenanceCycle);               
                    ((Window)o).DialogResult = true;
                }, 
                o => check));
            }
        }

        public RelayCommand Cancel
        {
            get
            {
                return cancel ?? (cancel = new RelayCommand(o =>
                {
                    ((Window)o).DialogResult = false;
                }));
            }
        }

        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (maintenanceCycle == null) return string.Empty;
                check = true;
                return string.Empty;
            }
        }

        public SelectingNewMaintenanceCycleViewModel(IServiceUnitOfWork serviceUnitOfWork, MaintenanceCycle maintenanceCycle)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;

            oldMaintenanceCycle = maintenanceCycle;
        }
    }
}
