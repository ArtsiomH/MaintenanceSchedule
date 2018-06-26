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
    class MaintenanceCycleViewModel : BaseViewModel, IDataErrorInfo
    {
        private ObservableCollection<MaintenanceType> maintenanceTypes;
        private MaintenanceCycleModel maintenanceCycleModel;
        private MaintenanceCycleModel oldMaitenanceCycleModel;
        private ActionType actionType;
        private bool check = false;
        private List<string> errors = new List<string>();

        private RelayCommand save;
        private RelayCommand cancel;
        private RelayCommand addType;

        public ObservableCollection<MaintenanceType> MaintenanceTypes
        {
            get
            {
                return maintenanceTypes;
            }
            set
            {
                maintenanceTypes = value;
                OnProtpertyChange(nameof(MaintenanceTypes));
            }
        }

        public MaintenanceCycleModel MaintenanceCycleModel
        {
            get
            {
                return maintenanceCycleModel;
            }
            set
            {
                maintenanceCycleModel = value;
                OnProtpertyChange(nameof(MaintenanceCycleModel));
            }
        }

        public string ShowName
        {
            get
            {
                return MaintenanceCycleModel.ShowName;
            }
            set
            {
                MaintenanceCycleModel.ShowName = value;
                OnProtpertyChange(nameof(ShowName));
            }
        }

        public string Name
        {
            get
            {
                return MaintenanceCycleModel.Name;
            }
            set
            {
                MaintenanceCycleModel.Name = value;
                OnProtpertyChange(nameof(Name));
            }
        }

        public RelayCommand Save
        {
            get
            {
                return save ?? (save = new RelayCommand(o =>
                {                    
                    oldMaitenanceCycleModel.Name = MaintenanceCycleModel.Name;
                    oldMaitenanceCycleModel.ShowName = MaintenanceCycleModel.ShowName;
                    for (int i = 0; i < oldMaitenanceCycleModel.MaintenanceTypes.Length; i++)
                    {
                        oldMaitenanceCycleModel.MaintenanceTypes[i] = MaintenanceCycleModel.MaintenanceTypes[i];
                    }
                    if (actionType == ActionType.Create)
                    {
                        serviceUnitOfWork.MaintenanceCycleModels.Create(oldMaitenanceCycleModel);
                    }
                    else if (actionType == ActionType.Update)
                    {
                        serviceUnitOfWork.MaintenanceCycleModels.Update(oldMaitenanceCycleModel);
                    }
                    ((Window)o).DialogResult = true;
                },
                o => check &&
                     MaintenanceCycleModel.MaintenanceTypes.Count(x => x != null) >= 2));
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

        public RelayCommand AddType
        {
            get
            {
                return addType ?? (addType = new RelayCommand(o =>
                {
                    MaintenanceType maintenanceType = new MaintenanceType();
                    MaintenanceTypeView view = new MaintenanceTypeView();
                    view.DataContext = new MaintenanceTypeViewModel(serviceUnitOfWork, maintenanceType);
                    if(view.ShowDialog() == true)
                    {
                        serviceUnitOfWork.MaintenanceTypes.Create(maintenanceType);
                        MaintenanceTypes = serviceUnitOfWork.MaintenanceTypes.GetAll();                       
                    } 
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
                            if (serviceUnitOfWork.MaintenanceCycles.GetAll().FirstOrDefault(x => x.Name.Equals(Name, StringComparison.CurrentCultureIgnoreCase)
                                                                                           && !x.Name.Equals(oldMaitenanceCycleModel.Name, StringComparison.CurrentCultureIgnoreCase)) != null) break;
                            errors.Remove(error + columnName);
                            error = string.Empty;
                            break;
                        }
                    case nameof(ShowName):
                        {
                            if (ShowName == null) return error;
                            error = "Поле не должно быть пустым";
                            if (ShowName == string.Empty) break;
                            errors.Remove(error + columnName);                            
                            error = string.Empty;
                            break;
                        }
                }
                string errorRecord = error + columnName;
                if (error != string.Empty && !errors.Contains(errorRecord)) errors.Add(errorRecord);
                if (Name == null ||
                    ShowName == null ||                                        
                    errors.Count != 0) check = false;
                else check = true;
                return error;
            }
        }

        public MaintenanceCycleViewModel(IServiceUnitOfWork serviceUnitOfWork, MaintenanceCycleModel maintenanceCycleModel, ActionType actionType)
        {
            this.actionType = actionType;

            this.serviceUnitOfWork = serviceUnitOfWork;
            oldMaitenanceCycleModel = maintenanceCycleModel;
            MaintenanceTypes = serviceUnitOfWork.MaintenanceTypes.GetAll();
            MaintenanceTypes.Insert(0, new MaintenanceType());
            MaintenanceCycleModel cycle = new MaintenanceCycleModel();
            cycle.Name = maintenanceCycleModel.Name;
            cycle.ShowName = maintenanceCycleModel.ShowName;
            for (int i = 0; i < cycle.MaintenanceTypes.Length; i++)
            {
                cycle.MaintenanceTypes[i] = maintenanceCycleModel.MaintenanceTypes[i];
            }
            MaintenanceCycleModel = cycle;
        }
    }
}
