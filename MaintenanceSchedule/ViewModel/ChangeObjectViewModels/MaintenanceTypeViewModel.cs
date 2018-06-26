using MaintenanceSchedule.Commands;
using MaintenanceSchedule.Interfaces;
using MaintenanceScheduleDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MaintenanceSchedule.ViewModel.ChangeObjectViewModels
{
    class MaintenanceTypeViewModel : BaseViewModel, IDataErrorInfo
    {
        private MaintenanceType MaintenanceType { get; set; }
        private List<MaintenanceType> maintenanceTypes { get; set; }
        private bool check = false;
        private List<string> errors = new List<string>();

        private RelayCommand save;
        private RelayCommand cancel;

        public string Name
        {
            get
            {
                return MaintenanceType.Name;
            }
            set
            {
                MaintenanceType.Name = value;
                OnProtpertyChange(nameof(Name));
            }
        }

        public RelayCommand Save
        {
            get
            {
                return save ?? (save = new RelayCommand(o =>
                {
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
                string error = string.Empty;
                switch (columnName)
                {
                    case nameof(Name):
                        {
                            if (Name == null) return string.Empty;
                            error = "Необходимо уникальное название";
                            if (serviceUnitOfWork.MaintenanceTypes.GetAll().FirstOrDefault(x => x.Name.Equals(Name, StringComparison.CurrentCultureIgnoreCase)) != null) break;
                            errors.Remove(error + columnName);
                            error = "Поле не должно быть пустым";
                            if (Name == string.Empty) break;
                            errors.Remove(error + columnName);
                            error = string.Empty;
                            break;
                        }                    
                }
                string errorRecord = error + columnName;
                if (error != string.Empty && !errors.Contains(errorRecord)) errors.Add(errorRecord);
                if (errors.Count != 0 ||
                    Name == null) check = false;
                else check = true;
                return error;
            }
        }

        public MaintenanceTypeViewModel(IServiceUnitOfWork serviceUnitOfWork)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
            maintenanceTypes = new List<MaintenanceType>(serviceUnitOfWork.MaintenanceTypes.GetAll());
            MaintenanceType = new MaintenanceType();            
        }

        public MaintenanceTypeViewModel(IServiceUnitOfWork serviceUnitOfWork, MaintenanceType maintenanceType)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
            maintenanceTypes = new List<MaintenanceType>(serviceUnitOfWork.MaintenanceTypes.GetAll());
            MaintenanceType = maintenanceType;
        }
    }
}
