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

namespace MaintenanceSchedule.ViewModel
{
    class MarkInspectionViewModel : BaseViewModel, IDataErrorInfo
    {
        private Substation substation;
        private string date;
        private RelayCommand save;
        private RelayCommand cancel;
        private bool check = false;
        private List<string> errors = new List<string>();

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
                    serviceUnitOfWork.Substations.MarkActualRecord(substation, DateTime.Parse(date));
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
                    Date == string.Empty) check = false;
                else check = true;
                return error;
            }
        }

        public MarkInspectionViewModel(IServiceUnitOfWork serviceUnitOfWork, Substation substation)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
            this.substation = substation;            
        }
    }
}
