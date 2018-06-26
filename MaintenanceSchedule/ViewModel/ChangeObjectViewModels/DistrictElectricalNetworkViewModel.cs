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
    class DistrictElectricalNetworkViewModel : BaseViewModel, IDataErrorInfo
    {
        private DistrictElectricalNetwork districtElectricalNetworkType;
        private DistrictElectricalNetwork oldDistrictElectricalNetwork;
        private string name;
        private string head;
        private bool check = false;
        private List<string> errors = new List<string>();

        private RelayCommand save;
        private RelayCommand cancel;

        public DistrictElectricalNetwork DistrictElectricalNetwork
        {
            get
            {
                return districtElectricalNetworkType;
            }
            set
            {
                districtElectricalNetworkType = value;
                OnProtpertyChange(nameof(DistrictElectricalNetwork));
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

        public string Head
        {
            get
            {
                return head;
            }
            set
            {
                head = value;
                OnProtpertyChange(nameof(Head));
            }
        }

        public RelayCommand Save
        {
            get
            {
                return save ?? (save = new RelayCommand(o =>
                {                    
                    oldDistrictElectricalNetwork.Name = Name;
                    oldDistrictElectricalNetwork.Head = Head;
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
                            if (serviceUnitOfWork.DistrictElectricalNetworks.GetAll().FirstOrDefault(x => x.Name == Name) != null) break;
                            errors.Remove(error + columnName);
                            error = "Поле не должно быть пустым";
                            if (Name == string.Empty) break;
                            errors.Remove(error + columnName);
                            error = string.Empty;
                            break;
                        }

                    case nameof(Head):
                        {
                            if (Head == null) return string.Empty;
                            error = "Неуобходимо уникальное имя";
                            if (serviceUnitOfWork.DistrictElectricalNetworks.GetAll().FirstOrDefault(x => x.Head == Head) != null) break;                            
                            errors.Remove(error + columnName);
                            error = "Поле не должно быть пустым";
                            if (Head == string.Empty) break;
                            errors.Remove(error + columnName);
                            error = string.Empty;
                            break;
                        }
                }
                string errorRecord = error + columnName;
                if (error != string.Empty && !errors.Contains(errorRecord)) errors.Add(errorRecord);
                if (Head != null ||
                    Name != null ||
                    errors.Count == 0) check = true;
                else check = false;
                return error;
            }
        }

        public DistrictElectricalNetworkViewModel(IServiceUnitOfWork serviceUnitOfWork, DistrictElectricalNetwork districtElectricalNetwork)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
            oldDistrictElectricalNetwork = districtElectricalNetwork;
            DistrictElectricalNetwork newDistrictElectricalNetwork = new DistrictElectricalNetwork();
            newDistrictElectricalNetwork.Name = districtElectricalNetwork.Name;
            newDistrictElectricalNetwork.Head = districtElectricalNetwork.Head;
            DistrictElectricalNetwork = newDistrictElectricalNetwork;
        }
    }
}
