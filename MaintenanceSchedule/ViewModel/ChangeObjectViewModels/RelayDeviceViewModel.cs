using MaintenanceSchedule.Commands;
using MaintenanceSchedule.Enums;
using MaintenanceSchedule.Interfaces;
using MaintenanceSchedule.Model;
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
    class RelayDeviceViewModel : BaseViewModel, IDataErrorInfo
    {
        private RelayDevice relayDevice;
        private RelayDevice oldRelayDevice;
        private Substation substation;
        private ObservableCollection<Substation> substations;
        private ObservableCollection<Attachment> attachments;
        private ObservableCollection<DeviceType> deviceTypes;
        private ObservableCollection<ElementBase> elementBases;
        private ObservableCollection<Manufacturer> manufacturers;
        private ObservableCollection<Act> acts;
        private ObservableCollection<MaintenanceCycle> maintenanceCycles;
        private ObservableCollection<MaintenanceCycleModel> maintenanceCycleModels;
        private MaintenanceCycleModel normalMaintenanceCycleModel;
        private MaintenanceCycleModel reducedMaitenenanceCycleModel;
        private RelayCommand addAttachment;
        private RelayCommand addDeviceType;
        private RelayCommand addElementBase;
        private RelayCommand addManufacturer;
        private RelayCommand addAct;
        private RelayCommand addMaintenanceCycle;
        private RelayCommand save;
        private RelayCommand cancel;
        private bool check = false;
        private List<string> errors = new List<string>();
        private string inputYear;
        private string maintenancePeriod;
        private ActionType actionType;

        public RelayDevice RelayDevice
        {
            get
            {
                return relayDevice;
            }
            set
            {
                relayDevice = value;
                OnProtpertyChange(nameof(RelayDevice));
            }
        }

        public string Name
        {
            get
            {
                return relayDevice.Name;
            }
            set
            {
                relayDevice.Name = value;
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

        public string MaintenancePeriod
        {
            get
            {
                return maintenancePeriod;
            }
            set
            {
                maintenancePeriod = value;
                OnProtpertyChange(nameof(MaintenancePeriod));
            }
        }

        public Substation Substation
        {
            get
            {
                return substation;
            }
            set
            {
                substation = value;
                Attachments = new ObservableCollection<Attachment>(serviceUnitOfWork.Attachments.GetAll().
                                                                   Where(x => x.Substation.MaintainedEquipmentId == substation.MaintainedEquipmentId));
                OnProtpertyChange(nameof(Substation));
            }
        }

        public Attachment Attachment
        {
            get
            {
                return relayDevice.Attachment;
            }
            set
            {
                relayDevice.Attachment = value;
                OnProtpertyChange(nameof(Attachment));
            }
        }

        public DeviceType DeviceType
        {
            get
            {
                return relayDevice.DeviceType;
            }
            set
            {
                relayDevice.DeviceType = value;
                OnProtpertyChange(nameof(DeviceType));
            }
        }

        public ElementBase ElementBase
        {
            get
            {
                return relayDevice.ElementBase;
            }
            set
            {
                relayDevice.ElementBase = value;
                OnProtpertyChange(nameof(ElementBase));
            }
        }

        public Manufacturer Manufacturer
        {
            get
            {
                return relayDevice.Manufacturer;
            }
            set
            {
                relayDevice.Manufacturer = value;
                OnProtpertyChange(nameof(Manufacturer));
            }
        }

        public Act Act
        {
            get
            {
                return relayDevice.Act;
            }
            set
            {
                relayDevice.Act = value;
                OnProtpertyChange(nameof(Act));
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

        public MaintenanceCycleModel ReducedMaintenanceCycleModel
        {
            get
            {
                return reducedMaitenenanceCycleModel;
            }
            set
            {
                reducedMaitenenanceCycleModel = value;
                OnProtpertyChange(nameof(ReducedMaintenanceCycleModel));
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

        public ObservableCollection<Attachment> Attachments
        {
            get
            {
                return attachments;
            }
            set
            {
                attachments = value;
                OnProtpertyChange(nameof(Attachments));
            }
        }

        public ObservableCollection<DeviceType> DeviceTypes
        {
            get
            {
                return deviceTypes;
            }
            set
            {
                deviceTypes = value;
                OnProtpertyChange(nameof(DeviceTypes));
            }
        }

        public ObservableCollection<ElementBase> ElementBases
        {
            get
            {
                return elementBases;
            }
            set
            {
                elementBases = value;
                OnProtpertyChange(nameof(ElementBases));
            }
        }

        public ObservableCollection<Manufacturer> Manufacturers
        {
            get
            {
                return manufacturers;
            }
            set
            {
                manufacturers = value;
                OnProtpertyChange(nameof(Manufacturers));
            }
        }

        public ObservableCollection<Act> Acts
        {
            get
            {
                return acts;
            }
            set
            {
                acts = value;
                OnProtpertyChange(nameof(Acts));
            }
        }

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

        public RelayCommand AddAttachment
        {
            get
            {
                return addAttachment ?? (addAttachment = new RelayCommand(o =>
                {
                    Attachment attachment = new Attachment();
                    AttachmentView view = new AttachmentView();
                    view.DataContext = new AttachmentViewModel(serviceUnitOfWork, attachment);
                    if (view.ShowDialog() == true)
                    {
                        serviceUnitOfWork.Attachments.Create(attachment);
                        Attachments = serviceUnitOfWork.Attachments.GetAll();
                    }
                }));
            }
        }
        public RelayCommand AddDeviceType
        {
            get
            {
                return addDeviceType ?? (addDeviceType = new RelayCommand(o =>
                {
                    DeviceType deviceType = new DeviceType();
                    DeviceTypeView view = new DeviceTypeView();
                    view.DataContext = new DeviceTypeViewModel(serviceUnitOfWork, deviceType);
                    if (view.ShowDialog() == true)
                    {
                        serviceUnitOfWork.DeviceTypes.Create(deviceType);
                        DeviceTypes = serviceUnitOfWork.DeviceTypes.GetAll();
                    }
                }));
            }
        }

        public RelayCommand AddElementBase
        {
            get
            {
                return addElementBase ?? (addElementBase = new RelayCommand(o =>
                {
                    ElementBase elementBase = new ElementBase();
                    ElementBaseView view = new ElementBaseView();
                    view.DataContext = new ElementBaseViewModel(serviceUnitOfWork, elementBase);
                    if (view.ShowDialog() == true)
                    {
                        serviceUnitOfWork.ElementBases.Create(elementBase);
                        ElementBases = serviceUnitOfWork.ElementBases.GetAll();
                    }
                }));
            }
        }

        public RelayCommand AddManufacturer
        {
            get
            {
                return addManufacturer ?? (addManufacturer = new RelayCommand(o =>
                {
                    Manufacturer manufacturer = new Manufacturer();
                    ManufacturerView view = new ManufacturerView();
                    view.DataContext = new ManufacturerViewModel(serviceUnitOfWork, manufacturer);
                    if (view.ShowDialog() == true)
                    {
                        serviceUnitOfWork.Manufacturers.Create(manufacturer);
                        Manufacturers = serviceUnitOfWork.Manufacturers.GetAll();
                    }
                }));
            }
        }

        public RelayCommand AddAct
        {
            get
            {
                return addAct ?? (addAct = new RelayCommand(o =>
                {
                    Act act = new Act();
                    ActView view = new ActView();
                    view.DataContext = new ActViewModel(serviceUnitOfWork, act);
                    if (view.ShowDialog() == true)
                    {
                        serviceUnitOfWork.Acts.Create(act);
                        Acts = serviceUnitOfWork.Acts.GetAll();
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
                                                                                                                                                          return y.Contains("В") || y.Contains("Н");
                                                                                                                                                      }
                                                                                                                                                      return false;
                                                                                                                                                  }).Count() != 0));
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
                    relayDevice.Name = Name;
                    relayDevice.InputYear = int.Parse(InputYear);                    
                    relayDevice.MaintenancePeriod = int.Parse(MaintenancePeriod);
                    relayDevice.ExpiryYear = relayDevice.InputYear + relayDevice.MaintenancePeriod;
                    relayDevice.NormalMaintenanceCycle = serviceUnitOfWork.MaintenanceCycles.Get(normalMaintenanceCycleModel.MaintenanceCycleId);
                    relayDevice.ReducedMaintenanceCycle = serviceUnitOfWork.MaintenanceCycles.Get(reducedMaitenenanceCycleModel.MaintenanceCycleId);
                    if (actionType == ActionType.Update)
                    {
                        relayDevice.MaintainedEquipmentId = oldRelayDevice.MaintainedEquipmentId;
                        serviceUnitOfWork.RelayDevices.Update(relayDevice);
                    } 
                    if (actionType == ActionType.Create)
                    {
                        serviceUnitOfWork.RelayDevices.Create(relayDevice);
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
                    case nameof(MaintenancePeriod):
                        {
                            if (MaintenancePeriod == null) return error;
                            error = "Поле не должно быть пустым";
                            if (MaintenancePeriod == string.Empty) break;
                            errors.Remove(error + columnName);
                            int number;
                            error = "Значение должно быть целочисленным числом";
                            try { number = Convert.ToInt32(MaintenancePeriod); }
                            catch { break; }
                            errors.Remove(error + columnName);
                            error = "Значение должно быть больше или равно 8";
                            if (number < 8) break;
                            errors.Remove(error + columnName);                                                     
                            error = string.Empty;
                            break;                                        
                        }
                }
                string errorRecord = error + columnName;                              
                if (error != string.Empty && !errors.Contains(errorRecord)) errors.Add(errorRecord);
                if (Name == null ||
                    MaintenancePeriod == null ||
                    InputYear == null ||
                    relayDevice.Attachment == null ||
                    relayDevice.DeviceType == null ||
                    relayDevice.ElementBase == null ||
                    relayDevice.Manufacturer == null ||                    
                    relayDevice.NormalMaintenanceCycle == null ||
                    relayDevice.ReducedMaintenanceCycle == null ||
                    errors.Count != 0) check = false;
                else check = true;
                return error;
            }
        }

        public RelayDeviceViewModel(IServiceUnitOfWork serviceUnitOfWork, RelayDevice relayDevice, ActionType actionType)
        {
            this.actionType = actionType;

            this.serviceUnitOfWork = serviceUnitOfWork;
            oldRelayDevice = relayDevice;

            RelayDevice newRelayDevice = new RelayDevice();
            newRelayDevice.Name = relayDevice.Name;
            newRelayDevice.Attachment = relayDevice.Attachment;
            newRelayDevice.InputYear = relayDevice.InputYear;
            newRelayDevice.MaintenancePeriod = relayDevice.MaintenancePeriod;
            newRelayDevice.DeviceType = relayDevice.DeviceType;
            newRelayDevice.ElementBase = relayDevice.ElementBase;
            newRelayDevice.Manufacturer = relayDevice.Manufacturer;
            newRelayDevice.Act = relayDevice.Act;
            newRelayDevice.NormalMaintenanceCycle = relayDevice.NormalMaintenanceCycle;
            newRelayDevice.ReducedMaintenanceCycle = relayDevice.ReducedMaintenanceCycle;            
            RelayDevice = newRelayDevice;

            if (relayDevice.Name != null)
            {
                InputYear = relayDevice.InputYear.ToString();
                MaintenancePeriod = relayDevice.MaintenancePeriod.ToString();
                NormalMaintenanceCycleModel = serviceUnitOfWork.MaintenanceCycleModels.Get(relayDevice.NormalMaintenanceCycle);
                ReducedMaintenanceCycleModel = serviceUnitOfWork.MaintenanceCycleModels.Get(relayDevice.ReducedMaintenanceCycle);
            }

            Substations = serviceUnitOfWork.Substations.GetAll();       
            DeviceTypes = serviceUnitOfWork.DeviceTypes.GetAll();
            ElementBases = serviceUnitOfWork.ElementBases.GetAll();
            Manufacturers = serviceUnitOfWork.Manufacturers.GetAll();
            Acts = serviceUnitOfWork.Acts.GetAll();
            MaintenanceCycleModels = new ObservableCollection<MaintenanceCycleModel>(serviceUnitOfWork.MaintenanceCycleModels.GetAll().Where(x => x.MaintenanceTypes.
                                                                                                                                                  Where(y => {
                                                                                                                                                      if (y != null)
                                                                                                                                                      {
                                                                                                                                                          return y.Contains("В") || y.Contains("Н");                                                                                                                                                          
                                                                                                                                                      }
                                                                                                                                                      return false;
                                                                                                                                                      }).Count() != 0));
        }
    }
}