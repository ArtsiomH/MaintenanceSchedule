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
    class AdditionalDeviceViewModel : BaseViewModel, IDataErrorInfo
    {
        private AdditionalDevice additionalDevice;
        private AdditionalDevice oldAdditionalDevice;
        private Substation substation;
        private ObservableCollection<Substation> substations;
        private ObservableCollection<Attachment> attachments;
        private ObservableCollection<Act> acts;
        private ObservableCollection<MaintenanceCycle> maintenanceCycles;
        private ObservableCollection<MaintenanceCycleModel> maintenanceCycleModels;
        private MaintenanceCycleModel normalMaintenanceCycleModel;
        private MaintenanceCycleModel reducedMaitnenanceCycleModel;
        private RelayCommand addAttachment;
        private RelayCommand addAct;
        private RelayCommand addMaintenanceCycle;
        private RelayCommand save;
        private RelayCommand cancel;
        private bool check = false;
        private List<string> errors = new List<string>();
        private string inputYear;
        private string maintenancePeriod;
        private ActionType actionType;

        public AdditionalDevice AdditionalDevice
        {
            get
            {
                return additionalDevice;
            }
            set
            {
                additionalDevice = value;
                OnProtpertyChange(nameof(AdditionalDevice));
            }
        }

        public string Name
        {
            get
            {
                return additionalDevice.Name;
            }
            set
            {
                additionalDevice.Name = value;
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
                return additionalDevice.Attachment;
            }
            set
            {
                additionalDevice.Attachment = value;
                OnProtpertyChange(nameof(Attachment));
            }
        }

        public Act Act
        {
            get
            {
                return additionalDevice.Act;
            }
            set
            {
                additionalDevice.Act = value;
                OnProtpertyChange(nameof(Act));
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
                return reducedMaitnenanceCycleModel;
            }
            set
            {
                reducedMaitnenanceCycleModel = value;
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
                        MaintenanceCycles = MaintenanceCycles = MaintenanceCycles = new ObservableCollection<MaintenanceCycle>(serviceUnitOfWork.MaintenanceCycles.GetAll().Where(x => x.MaintenanceYears.Where(y => (y.MaintenanceType.Name.Contains("В") ||
                                                                                                                                                                                                                      y.MaintenanceType.Name.Contains("Н"))).Count() != 0));
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
                    additionalDevice.Name = Name;
                    additionalDevice.InputYear = int.Parse(InputYear);
                    additionalDevice.MaintenancePeriod = int.Parse(MaintenancePeriod);
                    additionalDevice.ExpiryYear = additionalDevice.InputYear + additionalDevice.MaintenancePeriod;
                    additionalDevice.NormalMaintenanceCycle = serviceUnitOfWork.MaintenanceCycles.Get(normalMaintenanceCycleModel.MaintenanceCycleId);
                    additionalDevice.ReducedMaintenanceCycle = serviceUnitOfWork.MaintenanceCycles.Get(reducedMaitnenanceCycleModel.MaintenanceCycleId);
                    if (actionType == ActionType.Update)
                    {
                        additionalDevice.MaintainedEquipmentId = oldAdditionalDevice.MaintainedEquipmentId;
                        serviceUnitOfWork.AdditionalDevices.Update(additionalDevice);
                    }
                    if (actionType == ActionType.Create)
                    {
                        serviceUnitOfWork.AdditionalDevices.Create(additionalDevice);
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
                    additionalDevice.Attachment == null ||
                    additionalDevice.NormalMaintenanceCycle == null ||
                    additionalDevice.ReducedMaintenanceCycle == null ||
                    errors.Count != 0) check = false;
                else check = true;
                return error;
            }
        }

        public AdditionalDeviceViewModel(IServiceUnitOfWork serviceUnitOfWork, AdditionalDevice additionalDevice, ActionType actionType)
        {
            this.actionType = actionType;

            this.serviceUnitOfWork = serviceUnitOfWork;
            oldAdditionalDevice = additionalDevice;

            AdditionalDevice newAdditionalDevice = new AdditionalDevice();
            newAdditionalDevice.Name = additionalDevice.Name;
            newAdditionalDevice.Attachment = additionalDevice.Attachment;
            newAdditionalDevice.InputYear = additionalDevice.InputYear;
            newAdditionalDevice.MaintenancePeriod = additionalDevice.MaintenancePeriod;
            newAdditionalDevice.Act = additionalDevice.Act;
            newAdditionalDevice.NormalMaintenanceCycle =  additionalDevice.NormalMaintenanceCycle;            
            newAdditionalDevice.ReducedMaintenanceCycle = additionalDevice.ReducedMaintenanceCycle;            
            AdditionalDevice = newAdditionalDevice;

            if (additionalDevice.Name != null)
            {
                InputYear = additionalDevice.InputYear.ToString();
                MaintenancePeriod = additionalDevice.MaintenancePeriod.ToString();
                NormalMaintenanceCycleModel = serviceUnitOfWork.MaintenanceCycleModels.Get(additionalDevice.NormalMaintenanceCycle);
                ReducedMaintenanceCycleModel = serviceUnitOfWork.MaintenanceCycleModels.Get(additionalDevice.ReducedMaintenanceCycle);
            }

            Substations = serviceUnitOfWork.Substations.GetAll();
            Acts = serviceUnitOfWork.Acts.GetAll();
            //MaintenanceCycles = MaintenanceCycles = MaintenanceCycles = new ObservableCollection<MaintenanceCycle>(serviceUnitOfWork.MaintenanceCycles.GetAll().Where(x => x.MaintenanceYears.Where(y => (y.MaintenanceType.Name.Contains("В") ||
            //                                                                                                                                                                                             y.MaintenanceType.Name.Contains("Н"))).Count() != 0));
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