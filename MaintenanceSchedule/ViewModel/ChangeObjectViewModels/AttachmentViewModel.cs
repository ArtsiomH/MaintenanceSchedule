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
    class AttachmentViewModel : BaseViewModel, IDataErrorInfo
    {
        private Attachment attachment;
        private Attachment oldAttachment;
        private ObservableCollection<Substation> substations;
        private ObservableCollection<VoltageClass> voltageClasses;
        private ObservableCollection<ManagementOrganization> managementOrganizations;
        private RelayCommand addSubstation;
        private RelayCommand addVoltageClass;
        private RelayCommand addManagementOrganization;
        private RelayCommand save;
        private RelayCommand cancel;
        private bool check = false;
        private List<string> errors = new List<string>();

        public Attachment Attachment
        {
            get
            {
                return attachment;
            }
            set
            {
                attachment = value;
                OnProtpertyChange(nameof(Attachment));
            }
        }

        public string Name
        {
            get
            {
                return attachment.Name;
            }
            set
            {
                attachment.Name = value;
                OnProtpertyChange(nameof(Name));
            }
        }

        public Substation Substation
        {
            get
            {
                return attachment.Substation;
            }
            set
            {
                attachment.Substation = value;
                OnProtpertyChange(nameof(Substation));
            }
        }

        public VoltageClass VoltageClass
        {
            get
            {
                return attachment.VoltageClass;
            }
            set
            {
                attachment.VoltageClass = value;
                OnProtpertyChange(nameof(VoltageClass));
            }
        }

        public ManagementOrganization ManagementOrganization
        {
            get
            {
                return attachment.ManagementOrganization;
            }
            set
            {
                attachment.ManagementOrganization = value;
                OnProtpertyChange(nameof(ManagementOrganization));
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

        public ObservableCollection<VoltageClass> VoltageClasses
        {
            get
            {
                return voltageClasses;
            }
            set
            {
                voltageClasses = value;
                OnProtpertyChange(nameof(VoltageClasses));
            }
        }

        public ObservableCollection<ManagementOrganization> ManagementOrganizations
        {
            get
            {
                return managementOrganizations;
            }
            set
            {
                managementOrganizations = value;
                OnProtpertyChange(nameof(ManagementOrganizations));
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

        public RelayCommand AddVoltageClass
        {
            get
            {
                return addVoltageClass ?? (addVoltageClass = new RelayCommand(o =>
                {
                    VoltageClass voltageClass = new VoltageClass();
                    VoltageClassView view = new VoltageClassView();
                    view.DataContext = new VoltageClassViewModel(serviceUnitOfWork, voltageClass);
                    if (view.ShowDialog() == true)
                    {
                        serviceUnitOfWork.VoltageClasses.Create(voltageClass);
                        VoltageClasses = serviceUnitOfWork.VoltageClasses.GetAll();
                    }
                }));
            }
        }

        public RelayCommand AddManagementOrganization
        {
            get
            {
                return addManagementOrganization ?? (addManagementOrganization = new RelayCommand(o =>
                {
                    ManagementOrganization managementOrganization = new ManagementOrganization();
                    ManagementOrganizationView view = new ManagementOrganizationView();
                    view.DataContext = new ManagementOrganizationViewModel(serviceUnitOfWork, managementOrganization);
                    if (view.ShowDialog() == true)
                    {
                        serviceUnitOfWork.ManagementOrganizations.Create(managementOrganization);
                        ManagementOrganizations = serviceUnitOfWork.ManagementOrganizations.GetAll();
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
                    oldAttachment.Name = Name;
                    oldAttachment.SortingOrder = attachment.SortingOrder;
                    oldAttachment.Substation = attachment.Substation;
                    oldAttachment.VoltageClass = attachment.VoltageClass;
                    oldAttachment.ManagementOrganization = attachment.ManagementOrganization;
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
                            error = "Необходимо уникальное название";
                            if (serviceUnitOfWork.Attachments.GetAll().FirstOrDefault(x => x.Name.Equals(Name, StringComparison.CurrentCultureIgnoreCase)
                                                                                           && !x.Name.Equals(oldAttachment.Name, StringComparison.CurrentCultureIgnoreCase)) != null)
                                                                                           break;
                            errors.Remove(error);
                            error = "Поле не должно быть пустым";
                            if (Name == string.Empty) break;
                            errors.Remove(error);
                            error = string.Empty;
                            break;
                        }
                }
                if (error != string.Empty && !errors.Contains(error)) errors.Add(error);
                if (Name == null ||
                    attachment.Substation == null ||
                    attachment.VoltageClass == null ||
                    attachment.ManagementOrganization == null ||
                    errors.Count != 0) check = false;
                else check = true;
                return error;
            }
        }

        public AttachmentViewModel(IServiceUnitOfWork serviceUnitOfWork, Attachment attachment)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
            oldAttachment = attachment;

            Attachment newAttachment = new Attachment();
            
            newAttachment.Name = attachment.Name;
            //newSubstation.SortingOrder = substation.SortingOrder;
            newAttachment.Substation = attachment.Substation;            
            newAttachment.VoltageClass = attachment.VoltageClass;
            newAttachment.ManagementOrganization = attachment.ManagementOrganization;
            Attachment = newAttachment;

            Substations = serviceUnitOfWork.Substations.GetAll();
            VoltageClasses = serviceUnitOfWork.VoltageClasses.GetAll();
            ManagementOrganizations = serviceUnitOfWork.ManagementOrganizations.GetAll();
        }
    }
}