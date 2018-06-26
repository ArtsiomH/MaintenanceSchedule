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
    class VoltageClassViewModel : BaseViewModel, IDataErrorInfo
    {
        private VoltageClass voltageClass;
        private VoltageClass oldVoltageClass;
        private bool check = false;
        private List<string> errors = new List<string>();

        private RelayCommand save;
        private RelayCommand cancel;

        public VoltageClass VoltageClass
        {
            get
            {
                return voltageClass;
            }
            set
            {
                voltageClass = value;
                OnProtpertyChange(nameof(VoltageClass));
            }
        }

        public string Name
        {
            get
            {
                return voltageClass.Name;
            }
            set
            {
                voltageClass.Name = value;
                OnProtpertyChange(nameof(Name));
            }
        }

        public RelayCommand Save
        {
            get
            {
                return save ?? (save = new RelayCommand(o =>
                {
                    oldVoltageClass.Name = Name;
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
                if (Name == null) return string.Empty;
                string error = string.Empty;
                switch (columnName)
                {
                    case nameof(Name):
                        {
                            error = "Необходимо уникальное название";
                            if (serviceUnitOfWork.VoltageClasses.GetAll().FirstOrDefault(x => x.Name == Name) != null) break;
                            errors.Remove(error);
                            error = "Поле не должно быть пустым";
                            if (Name == string.Empty) break;
                            errors.Remove(error);
                            error = string.Empty;
                            break;
                        }                    
                }
                if (error != string.Empty && !errors.Contains(error)) errors.Add(error);
                if (errors.Count == 0) check = true;
                else check = false;
                return error;
            }
        }

        public VoltageClassViewModel(IServiceUnitOfWork serviceUnitOfWork, VoltageClass voltageClass)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
            oldVoltageClass = voltageClass;
            VoltageClass newVoltageClass = new VoltageClass();
            newVoltageClass.Name = voltageClass.Name;
            VoltageClass = newVoltageClass;
        }
    }
}
