using MaintenanceSchedule.Commands;
using MaintenanceSchedule.Interfaces;
using MaintenanceSchedule.View.ChangeObjectViews;
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
    class SubstationViewModel : BaseViewModel, IDataErrorInfo
    {
        private Substation substation;
        private Substation oldSubstation;
        private ObservableCollection<Team> teams;
        private ObservableCollection<TransformerType> transformerTypes;
        private ObservableCollection<DistrictElectricalNetwork> districtElectricalNetworks;
        private ObservableCollection<InspectionsFrequency> inspectionsFrequencies;
        private RelayCommand addTeam;
        private RelayCommand addTransformerType;
        private RelayCommand addDistrictElectricalNetwork;
        private RelayCommand addInspectionsFrequency;
        private RelayCommand save;
        private RelayCommand cancel;
        private bool check = false;
        private List<string> errors = new List<string>();
        private string inputYear;
        private string transformerCount;
        

        public Substation Substation
        {
            get
            {
                return substation;
            }
            set
            {
                substation = value;
                OnProtpertyChange(nameof(Substation));
            }
        }

        public string Name
        {
            get
            {
                return substation.Name;
            }
            set
            {
                substation.Name = value;
                OnProtpertyChange(nameof(Name));
            }
        }

        public string InputYear
        {
            get
            {
                return inputYear;
            }
            set
            {
                inputYear = value;
                OnProtpertyChange(nameof(InputYear));
            }
        }

        public string TransformerCount
        {
            get
            {
                return transformerCount;
            }
            set
            {
                transformerCount = value;
                OnProtpertyChange(nameof(TransformerCount));
            }
        }

        public Team Team
        {
            get
            {
                return substation.Team;
            }
            set
            {
                substation.Team = value;
                OnProtpertyChange(nameof(Team));
            }
        }

        public TransformerType TransformerType
        {
            get
            {
                return substation.TransformerType;
            }
            set
            {
                substation.TransformerType = value;
                OnProtpertyChange(nameof(TransformerType));
            }
        }
        
        public DistrictElectricalNetwork DistrictElectricalNetwork
        {
            get
            {
                return substation.DistrictElectricalNetwork;
            }
            set
            {
                substation.DistrictElectricalNetwork = value;
                OnProtpertyChange(nameof(DistrictElectricalNetwork));
            }
        }

        public InspectionsFrequency InspectionsFrequency
        {
            get
            {
                return substation.InspectionsFrequency;
            }
            set
            {
                substation.InspectionsFrequency = value;
                OnProtpertyChange(nameof(InspectionsFrequency));
            }
        }

        public ObservableCollection<Team> Teams
        {
            get
            {
                return teams;
            }
            set
            {
                teams = value;
                OnProtpertyChange(nameof(Teams));
            }
        }       

        public ObservableCollection<TransformerType> TransformerTypes
        {
            get
            {
                return transformerTypes;
            }
            set
            {
                transformerTypes = value;
                OnProtpertyChange(nameof(TransformerTypes));
            }
        }

        public ObservableCollection<DistrictElectricalNetwork> DistrictElectricalNetworks
        {
            get
            {
                return districtElectricalNetworks;
            }
            set
            {
                districtElectricalNetworks = value;
                OnProtpertyChange(nameof(DistrictElectricalNetworks));
            }
        }

        public ObservableCollection<InspectionsFrequency> InspectionsFrequencies
        {
            get
            {
                return inspectionsFrequencies;
            }
            set
            {
                inspectionsFrequencies = value;
                OnProtpertyChange(nameof(InspectionsFrequencies));
            }
        }

        public RelayCommand AddTeam
        {
            get
            {
                return addTeam ?? (addTeam = new RelayCommand(o =>
                {
                    Team team = new Team();
                    TeamView view = new TeamView();
                    view.DataContext = new TeamViewModel(serviceUnitOfWork, team);
                    if (view.ShowDialog() == true)
                    {
                        serviceUnitOfWork.Teams.Create(team);
                        Teams = serviceUnitOfWork.Teams.GetAll();
                    }
                }));
            }
        }

        public RelayCommand AddTransformerType
        {
            get
            {
                return addTransformerType ?? (addTransformerType = new RelayCommand(o =>
                {
                    TransformerType newTransformeType = new TransformerType();
                    TransformerTypeView view = new TransformerTypeView();
                    view.DataContext = new TransformerTypeViewModel(serviceUnitOfWork, newTransformeType);
                    if (view.ShowDialog() == true)
                    {
                        serviceUnitOfWork.TransformerTypes.Create(newTransformeType);
                        TransformerTypes = serviceUnitOfWork.TransformerTypes.GetAll();
                    }
                }));
            }
        }

        public RelayCommand AddDistrictElectricalNetwork
        {
            get
            {
                return addDistrictElectricalNetwork ?? (addDistrictElectricalNetwork = new RelayCommand(o =>
                {
                    DistrictElectricalNetwork newDistrictElectricalNetwork = new DistrictElectricalNetwork();
                    DistrictElectricalNetworkView view = new DistrictElectricalNetworkView();
                    view.DataContext = new DistrictElectricalNetworkViewModel(serviceUnitOfWork, newDistrictElectricalNetwork);
                    if (view.ShowDialog() == true)
                    {
                        serviceUnitOfWork.DistrictElectricalNetworks.Create(newDistrictElectricalNetwork);
                        DistrictElectricalNetworks = serviceUnitOfWork.DistrictElectricalNetworks.GetAll();
                    }
                }));
            }
        }

        public RelayCommand AddInspectionsFrequency
        {
            get
            {
                return addInspectionsFrequency ?? (addInspectionsFrequency = new RelayCommand(o =>
                {
                    InspectionsFrequency newInspectionsFrequency = new InspectionsFrequency();
                    InspectionsFrequencyView view = new InspectionsFrequencyView();
                    view.DataContext = new InspectionsFrequencyViewModel(serviceUnitOfWork, newInspectionsFrequency);
                    if (view.ShowDialog() == true)
                    {
                        serviceUnitOfWork.InspectionsFrequencies.Create(newInspectionsFrequency);
                        InspectionsFrequencies = serviceUnitOfWork.InspectionsFrequencies.GetAll();
                    }
                }));
            }
        }

        public RelayCommand Save
        {
            get
            {
                return save ?? (save = new RelayCommand(o =>
                {
                    oldSubstation.Name = Name;
                    oldSubstation.SortingOrder = substation.SortingOrder;
                    oldSubstation.Team = substation.Team;
                    oldSubstation.TransformerType = substation.TransformerType;
                    oldSubstation.DistrictElectricalNetwork = substation.DistrictElectricalNetwork;
                    oldSubstation.InspectionsFrequency = substation.InspectionsFrequency;
                    oldSubstation.InputYear = int.Parse(InputYear);
                    oldSubstation.TransformerCount = int.Parse(TransformerCount);
                    ((Window)o).DialogResult = true;
                }, o => check));
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
                            if (Name == null) return error;
                            error = "Поле не должно быть пустым";
                            if (Name == string.Empty) break;
                            errors.Remove(error + columnName);
                            error = "Имя должно быть уникальным";
                            if (serviceUnitOfWork.Substations.GetAll().FirstOrDefault(x => x.Name.Equals(Name, StringComparison.CurrentCultureIgnoreCase)
                                                                                           && !x.Name.Equals(oldSubstation.Name, StringComparison.CurrentCultureIgnoreCase)) != null) break;
                            errors.Remove(error + columnName);
                            error = string.Empty;
                            break;
                        }
                    case nameof(InputYear):
                        {
                            if (InputYear == null) return error;
                            error = "Поле не должно быть пустым";
                            if (InputYear == string.Empty) break;
                            errors.Remove(error + columnName);
                            int number;
                            error = "Значение должно быть целочисленным числом";
                            try { number = Convert.ToInt32(InputYear); }
                            catch { break; }
                            errors.Remove(error + columnName);
                            error = string.Empty;
                            break;
                        }
                    case nameof(TransformerCount):
                        {
                            if (TransformerCount == null) return error;
                            error = "Поле не должно быть пустым";
                            if (TransformerCount == string.Empty) break;
                            errors.Remove(error + columnName);
                            int number;
                            error = "Значение должно быть целочисленным числом";
                            try { number = Convert.ToInt32(TransformerCount); }
                            catch { break; }                            
                            errors.Remove(error + columnName);
                            error = "Значение должно быть больше 0";
                            if (number < 1) break;
                            errors.Remove(error + columnName);
                            error = string.Empty;                            
                            break;
                        }
                }
                string errorRecord = error + columnName;
                if (error != string.Empty && !errors.Contains(errorRecord)) errors.Add(errorRecord);
                if (Name == null ||
                    InputYear == null ||
                    TransformerCount == null ||
                    substation.Team == null ||
                    substation.TransformerType == null ||
                    substation.InspectionsFrequency == null ||
                    errors.Count != 0) check = false;
                else check = true;
                return error;
            }
        }

        public SubstationViewModel(IServiceUnitOfWork serviceUnitOfWork, Substation substation)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
            oldSubstation = substation;
            Substation newSubstation = new Substation();
            
            newSubstation.Name = substation.Name;
            newSubstation.InputYear = substation.InputYear;
            newSubstation.Team = substation.Team;
            newSubstation.TransformerType = substation.TransformerType;
            newSubstation.DistrictElectricalNetwork = substation.DistrictElectricalNetwork;
            newSubstation.InspectionsFrequency = substation.InspectionsFrequency;           
            Substation = newSubstation;

            if (substation.Name != null)
            {
                InputYear = substation.InputYear.ToString();
                TransformerCount = substation.TransformerCount.ToString();
            }

            Teams = serviceUnitOfWork.Teams.GetAll();
            TransformerTypes = serviceUnitOfWork.TransformerTypes.GetAll();
            DistrictElectricalNetworks = serviceUnitOfWork.DistrictElectricalNetworks.GetAll();
            InspectionsFrequencies = serviceUnitOfWork.InspectionsFrequencies.GetAll();
        }
    }
}