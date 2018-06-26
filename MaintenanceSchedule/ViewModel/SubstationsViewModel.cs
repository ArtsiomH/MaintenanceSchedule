using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaintenanceSchedule.Commands;
using MaintenanceSchedule.Interfaces;
using MaintenanceSchedule.Services;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Entities;
using MaintenanceSchedule.Model;
using MaintenanceSchedule.View.ChangeObjectViews;
using MaintenanceSchedule.ViewModel.ChangeObjectViewModels;
using MaintenanceSchedule.View;

namespace MaintenanceSchedule.ViewModel
{
    class SubstationsViewModel : BaseViewModel
    {
        private ObservableCollection<Substation> substations { get; set; }       

        private Substation selectedSubstation;                            
        private RelayCommand add;
        private RelayCommand change;
        private RelayCommand delete;
        private RelayCommand mark;
        private bool canMark = false;
        private ObservableCollection<Attachment> attachments = new ObservableCollection<Attachment>();
        private ObservableCollection<AdditionalWorkModel> additionalWorkModels = new ObservableCollection<AdditionalWorkModel>();
        private ObservableCollection<MaintenanceRecord> maintenanceRecords = new ObservableCollection<MaintenanceRecord>();

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

        public ObservableCollection<AdditionalWorkModel> AdditionalWorkModels
        {
            get
            {
                return additionalWorkModels;
            }
            set
            {
                additionalWorkModels = value;
                OnProtpertyChange(nameof(AdditionalWorkModels));
            }
        }

        public ObservableCollection<MaintenanceRecord> MaintenanceRecords
        {
            get
            {                
                return maintenanceRecords;
            }
            set
            {                
                maintenanceRecords = value;
                OnProtpertyChange(nameof(MaintenanceRecords));
            }
        }

        public Substation SelectedSubstation
        {
            get
            {
                return selectedSubstation;
            }
            set
            {
                if (value == null) return;
                selectedSubstation = value;              
                Substation substation = serviceUnitOfWork.Substations.Get(selectedSubstation.MaintainedEquipmentId);
                Attachments = new ObservableCollection<Attachment>(substation.Attachments);
                AdditionalWorkModels = serviceUnitOfWork.AdditionalWorkModels.GetAll(substation);
                MaintenanceRecords = new ObservableCollection<MaintenanceRecord>(substation.MaintenanceRecords);
                MaintenanceRecord record = substation.MaintenanceRecords.FirstOrDefault(x => x.ActualMaintenanceDate == null && x.IsPlanned == true && x.IsRescheduled == false);
                if (record == null) canMark = false;
                else canMark = true;
                OnProtpertyChange(nameof(SelectedSubstation));                
            }
        }       

        public RelayCommand Add
        {
            get
            {
                return add ?? (add = new RelayCommand(o =>
                {
                    Substation newSubstation = new Substation();
                    SubstationView view = new SubstationView();
                    view.DataContext = new SubstationViewModel(serviceUnitOfWork, newSubstation);
                    if (view.ShowDialog() == true)
                    {
                        serviceUnitOfWork.Substations.Create(newSubstation);
                        Substations = serviceUnitOfWork.Substations.GetAll();
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
                    if (selectedSubstation == null) return;
                    Substation newSubstation = new Substation();
                    SubstationView view = new SubstationView();
                    view.DataContext = new SubstationViewModel(serviceUnitOfWork, selectedSubstation);
                    if (view.ShowDialog() == true)
                    {
                        serviceUnitOfWork.Substations.Update(selectedSubstation);
                        Substations = serviceUnitOfWork.Substations.GetAll();
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
                    if (SelectedSubstation == null) return;
                    serviceUnitOfWork.Substations.Delete(SelectedSubstation);
                    Substations = serviceUnitOfWork.Substations.GetAll();
                    Attachments = null;
                    AdditionalWorkModels = null;
                    MaintenanceRecords = null;
                }));
            }
        }

        public RelayCommand Mark
        {
            get
            {
                return mark ?? (mark = new RelayCommand(o =>
                {
                    if (SelectedSubstation == null) return;
                    MarkInspectionView view = new MarkInspectionView();
                    view.DataContext = new MarkInspectionViewModel(serviceUnitOfWork, SelectedSubstation);
                    if (view.ShowDialog() == true)
                    {
                        SelectedSubstation = SelectedSubstation;
                    }
                },
                o => canMark));
            }
        }

        public SubstationsViewModel(IServiceUnitOfWork serviceUnitOfWork)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
            Substations = serviceUnitOfWork.Substations.GetAll();
        }
    }
}