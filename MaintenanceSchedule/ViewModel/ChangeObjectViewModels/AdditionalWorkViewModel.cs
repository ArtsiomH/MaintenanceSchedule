using MaintenanceSchedule.Commands;
using MaintenanceSchedule.Enums;
using MaintenanceSchedule.Interfaces;
using MaintenanceSchedule.Model;
using MaintenanceSchedule.View;
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
    class AdditionalWorkViewModel : BaseViewModel, IDataErrorInfo
    {
        private AdditionalWork additionalWork;
        private AdditionalWork oldAdditionalWork;
        private ObservableCollection<Substation> substations;
        private ObservableCollection<MaintenanceCycleModel> maintenanceCycleModels;
        private MaintenanceCycleModel normalMaintenanceCycleModel;
        private ObservableCollection<RelayDevice> relayDevices;
        private ObservableCollection<RelayDevice> addedRelayDevices;
        private RelayCommand addSubstation;
        private RelayCommand addMaintenanceCycle;
        private RelayCommand addDevice;
        private RelayCommand removeDevice;
        private RelayCommand save;
        private RelayCommand cancel;
        private bool check = false;
        private List<string> errors = new List<string>();
        private string inputYear;
        private ActionType actionType;

        public AdditionalWork AdditionalWork
        {
            get
            {
                return additionalWork;
            }
            set
            {
                additionalWork = value;
                OnProtpertyChange(nameof(additionalWork));
            }
        }

        public string Name
        {
            get
            {
                return additionalWork.Name;
            }
            set
            {
                additionalWork.Name = value;
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

        public Substation Substation
        {
            get
            {
                return additionalWork.Substation;
            }
            set
            {
                if (value == null) return;
                if (oldAdditionalWork.Substation.MaintainedEquipmentId == value.MaintainedEquipmentId && additionalWork.Devices != null)
                {
                    AddedRelayDevices = new ObservableCollection<RelayDevice>(additionalWork.Devices);
                    RelayDevices = new ObservableCollection<RelayDevice>(serviceUnitOfWork.RelayDevices.GetAll().Where(x => x.Attachment.Substation.MaintainedEquipmentId == value.MaintainedEquipmentId).
                                                                                                                 Except(AddedRelayDevices).
                                                                                                                 OrderBy(x => x.Attachment.Name + x.Name));
                }
                else if (additionalWork.Substation == null || additionalWork.Substation.MaintainedEquipmentId != value.MaintainedEquipmentId)
                {
                    RelayDevices = new ObservableCollection<RelayDevice>(serviceUnitOfWork.RelayDevices.GetAll().
                                                                       Where(x => x.Attachment.Substation.MaintainedEquipmentId == value.MaintainedEquipmentId).
                                                                       OrderBy(x => x.Attachment.Name + x.Name));
                    AddedRelayDevices.Clear();
                }
                additionalWork.Substation = value;
                
                OnProtpertyChange(nameof(Substation));
            }
        }

        public MaintenanceCycleModel NormalMaintenanceCycleModel
        {
            get
            {
                return normalMaintenanceCycleModel;
            }
            set
            {
                normalMaintenanceCycleModel = value;
                OnProtpertyChange(nameof(NormalMaintenanceCycleModel));
            }
        }

        public ObservableCollection<Substation> Substations
        {
            get
            {
                return substations;
            }
            set
            {
                substations = value;
                OnProtpertyChange(nameof(Substations));
            }
        }

        public ObservableCollection<MaintenanceCycleModel> MaintenanceCycleModels
        {
            get
            {
                return maintenanceCycleModels;
            }
            set
            {
                maintenanceCycleModels = value;
                OnProtpertyChange(nameof(MaintenanceCycleModels));
            }
        }

        public ObservableCollection<RelayDevice> RelayDevices
        {
            get
            {
                return relayDevices;
            }
            set
            {
                relayDevices = value;
                OnProtpertyChange(nameof(RelayDevices));
            }
        }

        public ObservableCollection<RelayDevice> AddedRelayDevices
        {
            get
            {
                return addedRelayDevices;
            }
            set
            {
                addedRelayDevices = value;
                OnProtpertyChange(nameof(AddedRelayDevices));
            }
        }

        public RelayCommand AddSubstation
        {
            get
            {
                return addSubstation ?? (addSubstation = new RelayCommand(o =>
                {
                    Substation substation = new Substation();
                    SubstationView view = new SubstationView();
                    view.DataContext = new SubstationViewModel(serviceUnitOfWork, substation);
                    if (view.ShowDialog() == true)
                    {
                        serviceUnitOfWork.Substations.Create(substation);
                        Substations = serviceUnitOfWork.Substations.GetAll();
                    }
                }));
            }
        }


        public RelayCommand AddMaintenanceCycle
        {
            get
            {
                return addMaintenanceCycle ?? (addMaintenanceCycle = new RelayCommand(o =>
                {
                    MaintenanceCycleModel maintenanceCycleModel = new MaintenanceCycleModel();
                    MaintenanceCycleView view = new MaintenanceCycleView();
                    view.DataContext = new MaintenanceCycleViewModel(serviceUnitOfWork, maintenanceCycleModel, ActionType.Create);
                    if (view.ShowDialog() == true)
                    {
                        serviceUnitOfWork.MaintenanceCycleModels.Create(maintenanceCycleModel);
                        MaintenanceCycleModels = new ObservableCollection<MaintenanceCycleModel>(serviceUnitOfWork.MaintenanceCycleModels.GetAll().Where(x => x.MaintenanceTypes.
                                                                                                                                                  Where(y => {
                                                                                                                                                      if (y != null)
                                                                                                                                                      {
                                                                                                                                                          return !y.Contains("Н");
                                                                                                                                                      }
                                                                                                                                                      return false;
                                                                                                                                                  }).Count() != 0));
                    }
                }));
            }
        }

        public RelayCommand AddDevice
        {
            get
            {
                return addDevice ?? (addDevice = new RelayCommand(o =>
                {
                    if (o == null) return;
                    RelayDevice device = o as RelayDevice;
                    AddedRelayDevices.Add(device);
                    RelayDevices.Remove(device);
                }));
            }
        }

        public RelayCommand RemoveDevice
        {
            get
            {
                return removeDevice ?? (removeDevice = new RelayCommand(o =>
                {
                    if (o == null) return;
                    RelayDevice device = o as RelayDevice;                                        
                    RelayDevices.Add(device);
                    RelayDevices = new ObservableCollection<RelayDevice>(RelayDevices.
                        OrderBy(x => x.Attachment.Name + x.Name));
                    AddedRelayDevices.Remove(device);                                        
                }));
            }
        }

        public RelayCommand Save
        {
            get
            {
                return save ?? (save = new RelayCommand(o =>
                {
                    additionalWork.Name = Name;
                    additionalWork.InputYear = int.Parse(InputYear);
                    additionalWork.ReducedMaintenanceCycle = additionalWork.NormalMaintenanceCycle;
                    additionalWork.Devices = AddedRelayDevices.ToList();
                    additionalWork.Substation = Substation;
                    if (actionType == ActionType.Create)
                    {
                        serviceUnitOfWork.AdditionalWorks.Create(additionalWork);
                    }
                    else if (actionType == ActionType.Update)
                    {
                        additionalWork.MaintainedEquipmentId = oldAdditionalWork.MaintainedEquipmentId;
                        serviceUnitOfWork.AdditionalWorks.Update(additionalWork);
                    }
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
                }
                string errorRecord = error + columnName;
                if (error != string.Empty && !errors.Contains(errorRecord)) errors.Add(errorRecord);
                if (Name == null ||
                    InputYear == null ||
                    additionalWork.Substation == null ||
                    additionalWork.NormalMaintenanceCycle == null ||                    
                    errors.Count != 0) check = false;
                else check = true;
                return error;
            }
        }

        public AdditionalWorkViewModel(IServiceUnitOfWork serviceUnitOfWork, AdditionalWork additionalWork,
            ActionType actionType)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
            this.actionType = actionType;

            oldAdditionalWork = additionalWork;

            AdditionalWork newAdditionalWork = new AdditionalWork();
            this.additionalWork = newAdditionalWork;
            newAdditionalWork.Name = additionalWork.Name;
            newAdditionalWork.InputYear = additionalWork.InputYear;
            newAdditionalWork.NormalMaintenanceCycle = additionalWork.NormalMaintenanceCycle;
            newAdditionalWork.ReducedMaintenanceCycle = additionalWork.ReducedMaintenanceCycle;
            newAdditionalWork.Substation = additionalWork.Substation;
            newAdditionalWork.Devices = additionalWork.Devices; 

            if (actionType == ActionType.Update)
            {
                InputYear = additionalWork.InputYear.ToString();
                additionalWork = newAdditionalWork;
                NormalMaintenanceCycleModel = serviceUnitOfWork.MaintenanceCycleModels.
                    Get(additionalWork.NormalMaintenanceCycle);             
            }
            else
            {
                RelayDevices = new ObservableCollection<RelayDevice>();
                AddedRelayDevices = new ObservableCollection<RelayDevice>();
            }
            
            Substations = serviceUnitOfWork.Substations.GetAll();
            Substation = additionalWork.Substation;
            MaintenanceCycleModels = new ObservableCollection<MaintenanceCycleModel>(serviceUnitOfWork.
                MaintenanceCycleModels.GetAll().Where(x => x.MaintenanceTypes.Where(y => 
                {
                    if (y != null)
                    {
                        return !y.Contains("Н");
                    }
                    return false;
                }).Count() != 0));

        }
    }
}
