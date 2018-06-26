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
    class ActViewModel : BaseViewModel, IDataErrorInfo
    {
        private Act act;
        private Act oldAct;

        private bool check = false;
        private List<string> errors = new List<string>();

        private string date;

        private RelayCommand save;
        private RelayCommand cancel;

        public Act Act
        {
            get
            {
                return act;
            }
            set
            {
                act = value;
                OnProtpertyChange(nameof(Act));
            }
        }

        public string Name
        {
            get
            {
                return act.Name;
            }
            set
            {
                act.Name = value;
                OnProtpertyChange(nameof(Name));
            }
        }

        public string Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
                OnProtpertyChange(nameof(Date));
            }
        }

        public RelayCommand Save
        {
            get
            {
                return save ?? (save = new RelayCommand(o =>
                {
                    oldAct.Name = Name;
                    oldAct.CreationDate = Convert.ToDateTime(date);
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
                            if (serviceUnitOfWork.Acts.GetAll().FirstOrDefault(x => x.Name == Name) != null) break;
                            errors.Remove(error + columnName);
                            error = "Поле не должно быть пустым";
                            if (Name == string.Empty) break;
                            errors.Remove(error + columnName);
                            error = string.Empty;
                            break;
                        }
                    case nameof(Date):
                        {
                            if (Date == null) return string.Empty;
                            error = "Неверный формат даты";
                            DateTime newDate;
                            try
                            {
                                newDate = Convert.ToDateTime(Date);
                            }
                            catch (FormatException)
                            {
                                break;
                            }
                            error = "Необходимо указать дату";
                            if (Date == string.Empty) break;                           
                            errors.Remove(error + columnName);
                            error = string.Empty;
                            break;
                        }
                }
                string errorRecord = error + columnName;
                if (error != string.Empty && !errors.Contains(errorRecord)) errors.Add(errorRecord);
                if (errors.Count != 0 ||
                    Name == null ||
                    Date == string.Empty) check = false;
                else check = true;
                return error;
            }
        }

        public ActViewModel(IServiceUnitOfWork serviceUnitOfWork, Act act)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
            oldAct = act;
            Act newAct = new Act();
            newAct.Name = act.Name;
            newAct.CreationDate = act.CreationDate;
            Act = newAct;
        }
    }
}