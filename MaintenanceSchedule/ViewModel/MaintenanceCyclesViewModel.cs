using MaintenanceSchedule.Commands;
using MaintenanceSchedule.Interfaces;
using MaintenanceSchedule.Model;
using MaintenanceSchedule.ViewModel.ChangeObjectViewModels;
using MaintenanceSchedule.View.ChangeObjectViews;
using MaintenanceScheduleDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaintenanceSchedule.Enums;

namespace MaintenanceSchedule.ViewModel
{
    class MaintenanceCyclesViewModel : BaseViewModel
    {
        private ObservableCollection<MaintenanceCycleModel> maintenanceCycleModels;        
        private RelayCommand add;
        private RelayCommand change;
        private RelayCommand delete;
        private RelayCommand getItem;

        public MaintenanceCycleModel SelectedMaintenanceCycle { get; set; }

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

        public RelayCommand Add
        {
            get
            {
                return add ?? (add = new RelayCommand(o =>
                {
                    MaintenanceCycleView view = new MaintenanceCycleView();
                    MaintenanceCycleModel maintenanceCycleModel = new MaintenanceCycleModel();
                    view.DataContext = new MaintenanceCycleViewModel(serviceUnitOfWork, maintenanceCycleModel, ActionType.Create);
                    if (view.ShowDialog() == true)
                    {
                        //serviceUnitOfWork.MaintenanceCycleModels.Create(maintenanceCycleModel);
                        MaintenanceCycleModels = serviceUnitOfWork.MaintenanceCycleModels.GetAll();
                    }
                }));
            }
        }

        public RelayCommand Change
        {
            get
            {
                return change ?? (change = new RelayCommand(o =>
                {
                    if (SelectedMaintenanceCycle == null) return;
                    MaintenanceCycleView view = new MaintenanceCycleView();
                    view.DataContext = new MaintenanceCycleViewModel(serviceUnitOfWork, SelectedMaintenanceCycle, ActionType.Update);
                    if (view.ShowDialog() == true)
                    {
                        //serviceUnitOfWork.MaintenanceCycleModels.Update(SelectedMaintenanceCycle);
                        MaintenanceCycleModels = serviceUnitOfWork.MaintenanceCycleModels.GetAll();
                    }
                }));
            }
        }

        public RelayCommand Delete
        {
            get
            {
                return delete ?? (delete = new RelayCommand(o =>
                {
                    if (SelectedMaintenanceCycle == null) return;
                    bool deleteCycle = false;
                    if (serviceUnitOfWork.MaintenanceCycles.GetAllMaintainedEquipments(SelectedMaintenanceCycle.MaintenanceCycleId).Count == 0) deleteCycle = true;
                    else
                    {
                        SelectingNewMaintenanceCycleView view = new SelectingNewMaintenanceCycleView();
                        view.DataContext = new SelectingNewMaintenanceCycleViewModel(serviceUnitOfWork, serviceUnitOfWork.MaintenanceCycles.Get(SelectedMaintenanceCycle.MaintenanceCycleId));
                        if (view.ShowDialog() == true) deleteCycle = true;
                    }
                    if (deleteCycle)
                    {
                        serviceUnitOfWork.MaintenanceCycles.Delete(serviceUnitOfWork.MaintenanceCycles.Get(SelectedMaintenanceCycle.MaintenanceCycleId));
                        MaintenanceCycleModels = serviceUnitOfWork.MaintenanceCycleModels.GetAll();
                    }                    
                }));
            }
        }

        public RelayCommand GetItem
        {
            get
            {
                return getItem ?? (getItem = new RelayCommand(o =>
                {
                    if (SelectedMaintenanceCycle != (MaintenanceCycleModel)o)
                    {
                        SelectedMaintenanceCycle = (MaintenanceCycleModel)o;
                    }
                }));
            }
        }

        public MaintenanceCyclesViewModel(IServiceUnitOfWork serviceUnitOfWork)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
            MaintenanceCycleModels = serviceUnitOfWork.MaintenanceCycleModels.GetAll();                      
        }


    }
}
