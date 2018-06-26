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
    class InspectionsFrequencyViewModel : BaseViewModel, IDataErrorInfo
    {
        private InspectionsFrequency inspectionsFrequency;
        private InspectionsFrequency oldInspectionsFrequency;
        private string name;
        private string count;
        private bool check = false;
        private List<string> errors = new List<string>();

        private RelayCommand save;
        private RelayCommand cancel;

        public InspectionsFrequency InspectionsFrequency
        {
            get
            {
                return inspectionsFrequency;
            }
            set
            {
                inspectionsFrequency = value;
                OnProtpertyChange(nameof(InspectionsFrequency));
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnProtpertyChange(nameof(Name));
            }
        }

        public string Count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
                OnProtpertyChange(nameof(Count));
            }
        }

        public RelayCommand Save
        {
            get
            {
                return save ?? (save = new RelayCommand(o =>
                {
                    oldInspectionsFrequency.Name = Name;
                    oldInspectionsFrequency.Count = int.Parse(count);
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
                if (Name == null && Count == null) return string.Empty;
                string error = string.Empty;
                switch (columnName)
                {
                    case nameof(Name):
                        {
                            error = "Необходимо уникальное название";
                            if (serviceUnitOfWork.DistrictElectricalNetworks.GetAll().FirstOrDefault(x => x.Name == Name) != null) break;
                            errors.Remove(error + columnName);
                            error = "Поле не должно быть пустым";
                            if (Name == string.Empty) break;
                            errors.Remove(error + columnName);
                            error = string.Empty;
                            break;
                        }

                    case nameof(Count):
                        {
                            int number;
                            error = "Значение должно быть целочисленным числом";
                            try { number = Convert.ToInt32(Count); }
                            catch { break; }
                            errors.Remove(error + columnName);
                            error = "Значение должно лежать в пределе от 1 до 12";
                            if (number > 12 || number <= 0) break;
                            errors.Remove(error + columnName);
                            error = "Поле не должно быть пустым";
                            if (Count == string.Empty) break;
                            errors.Remove(error + columnName);
                            error = string.Empty;
                            break;
                        }
                }
                string errorRecord = error + columnName;
                if (error != string.Empty && !errors.Contains(error + columnName)) errors.Add(error + columnName);
                if (errors.Count == 0) check = true;
                else check = false;
                return error;
            }
        }

        public InspectionsFrequencyViewModel(IServiceUnitOfWork serviceUnitOfWork, InspectionsFrequency inspectionsFrequency)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
            oldInspectionsFrequency = inspectionsFrequency;
            InspectionsFrequency newInspectionsFrequency = new InspectionsFrequency();
            newInspectionsFrequency.Name = InspectionsFrequency.Name;
            newInspectionsFrequency.Count = inspectionsFrequency.Count;
            InspectionsFrequency = newInspectionsFrequency;
        }
    }
}

