using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaintenanceSchedule.Commands;
using MaintenanceSchedule.Interfaces;
using MaintenanceSchedule.Model;
using MaintenanceSchedule.Services;
using System.Windows;
using System.Windows.Controls;
using System.Collections;
using System.Collections.ObjectModel;
using System.Configuration;
using MaintenanceSchedule.View;
using MaintenanceScheduleDataLayer.Entities;
using System.Data.Entity;
using System.IO;

namespace MaintenanceSchedule.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        private ContentControl contentControl = new ContentControl();
        private string connectionString = ConfigurationManager.ConnectionStrings["ProbLoc"].ConnectionString;
        public ObservableCollection<RelayDevice> Devices { get; set; }
        
        
        public ContentControl ContentControl
        {
            get { return contentControl; }
            set
            {
                contentControl = value;
                OnProtpertyChange(nameof(ContentControl));
            }
        }

        private RelayCommand showSubstations;
        private RelayCommand showAttachments;
        private RelayCommand showDevices;
        private RelayCommand showAdditionalDevices;
        private RelayCommand showCombineDevices;
        private RelayCommand showMaintenanceCycles;

        public RelayCommand ShowSubstations
        {
            get
            {
                return showSubstations ?? (showSubstations = new RelayCommand(o =>
                {
                    //ContentControl.Content = ((Window)o).FindResource("Substations");
                    SubstationsView view = new SubstationsView();
                    view.DataContext = new SubstationsViewModel(serviceUnitOfWork);
                    ContentControl.Content = view;                                                                           
                }));
            }
        }

        public RelayCommand ShowAttachments
        {
            get
            {
                return showAttachments ?? (showAttachments = new RelayCommand(o =>
                {
                    AttachmentsView view = new AttachmentsView();
                    view.DataContext = new AttachmentsViewModel(serviceUnitOfWork);
                    ContentControl.Content = view;
                }));
            }
        }

        public RelayCommand ShowDevices
        {
            get
            {
                return showDevices ?? (showDevices = new RelayCommand(o =>
                {
                    RelayDevicesView view = new RelayDevicesView();
                    view.DataContext = new RelayDevicesViewModel(serviceUnitOfWork);
                    ContentControl.Content = view;
                }));
            }
        }

        public RelayCommand ShowAdditionalDevices
        {
            get
            {
                return showAdditionalDevices ?? (showAdditionalDevices = new RelayCommand(o =>
                {
                    AdditionalDevicesView view = new AdditionalDevicesView();
                    view.DataContext = new AdditionalDevicesViewModel(serviceUnitOfWork);
                    ContentControl.Content = view;
                }));
            }
        }

        public RelayCommand ShowCombineDevices
        {
            get
            {
                return showCombineDevices ?? (showCombineDevices = new RelayCommand(o =>
                {
                    AdditionalWorksView view = new AdditionalWorksView();
                    view.DataContext = new AdditionalWorksViewModel(serviceUnitOfWork);
                    ContentControl.Content = view;
                }));
            }
        }

        public RelayCommand ShowMaintenanceCycles
        {
            get
            {
                return showMaintenanceCycles ?? (showMaintenanceCycles = new RelayCommand(o =>
                {
                    MaintenanceCyclesView view = new MaintenanceCyclesView();
                    view.DataContext = new MaintenanceCyclesViewModel(serviceUnitOfWork);
                    ContentControl.Content = view;
                }));
            }
        }

        public MainViewModel()
        {
            string s = Directory.GetCurrentDirectory();
            AppDomain.CurrentDomain.SetData("DataDirectory", Directory.GetCurrentDirectory());
            serviceUnitOfWork = new ServiceUnitOfWork(connectionString);
            serviceUnitOfWork.Substations.GetAll();

        }
    }
}
