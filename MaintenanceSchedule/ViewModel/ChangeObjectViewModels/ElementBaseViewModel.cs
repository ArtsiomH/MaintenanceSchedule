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
    class ElementBaseViewModel : BaseViewModel, IDataErrorInfo
    {
        private ElementBase elementBase;
        private ElementBase oldElementBase;
        private bool check = false;
        private List<string> errors = new List<string>();

        private RelayCommand save;
        private RelayCommand cancel;

        public ElementBase ElementBase
        {
            get
            {
                return elementBase;
            }
            set
            {
                elementBase = value;
                OnProtpertyChange(nameof(ElementBase));
            }
        }

        public string Name
        {
            get
            {
                return elementBase.Name;
            }
            set
            {
                elementBase.Name = value;
                OnProtpertyChange(nameof(Name));
            }
        }

        public string Description
        {
            get
            {
                return elementBase.Description;
            }
            set
            {
                elementBase.Description = value;
                OnProtpertyChange(nameof(Description));
            }
        }

        public RelayCommand Save
        {
            get
            {
                return save ?? (save = new RelayCommand(o =>
                {
                    oldElementBase.Name = Name;
                    oldElementBase.Description = Description;
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
                            if (serviceUnitOfWork.DeviceTypes.GetAll().FirstOrDefault(x => x.Name == Name) != null) break;
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

        public ElementBaseViewModel(IServiceUnitOfWork serviceUnitOfWork, ElementBase elementBase)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
            oldElementBase = elementBase;
            ElementBase newElementBase = new ElementBase();
            newElementBase.Name = elementBase.Name;
            newElementBase.Description = elementBase.Description;
            ElementBase = newElementBase;
        }
    }
}
