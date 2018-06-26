using MaintenanceScheduleDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaintenanceSchedule.Interfaces;
using MaintenanceSchedule.Commands;
using MaintenanceSchedule.Model;
using MaintenanceSchedule.View.ChangeObjectViews;
using MaintenanceSchedule.ViewModel.ChangeObjectViewModels;

namespace MaintenanceSchedule.ViewModel
{
    class AttachmentsViewModel : BaseViewModel
    {
        public ObservableCollection<Attachment> Attachments { get; set; }

        private ObservableCollection<RelayDeviceModel> relayDeviceModels;
        private ObservableCollection<AdditionalDeviceModel> additionalDeviceModels;
        private RelayCommand add;
        private RelayCommand change;
        private RelayCommand delete;
        private Attachment selectedAttachment;        

        public ObservableCollection<RelayDeviceModel> RelayDeviceModels
        {
            get
            {
                return relayDeviceModels;
            }
            set
            {
                relayDeviceModels = value;
                OnProtpertyChange(nameof(RelayDeviceModels));
            }
        }

        public ObservableCollection<AdditionalDeviceModel> AdditionalDeviceModels
        {
            get
            {
                return additionalDeviceModels;
            }
            set
            {
                additionalDeviceModels = value;
                OnProtpertyChange(nameof(AdditionalDeviceModels));
            }
        }

        public Attachment SelectedAttachment
        {
            get
            {
                return selectedAttachment;
            }
            set
            {
                if (value == null) return;
                selectedAttachment = value;
                Attachment attachment = serviceUnitOfWork.Attachments.Get(value.AttachmentId);
                RelayDeviceModels = serviceUnitOfWork.RelayDeviceModels.GetAll(attachment);
                AdditionalDeviceModels = serviceUnitOfWork.AdditionalDeviceModels.GetAll(attachment);
                OnProtpertyChange(nameof(SelectedAttachment));
            }
        }

        public RelayCommand Add
        {
            get
            {
                return add ?? (add = new RelayCommand(o =>
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

        public RelayCommand Change
        {
            get
            {
                return change ?? (change = new RelayCommand(o =>
                {
                    if (selectedAttachment == null) return;
                    AttachmentView view = new AttachmentView();
                    view.DataContext = new AttachmentViewModel(serviceUnitOfWork, selectedAttachment);
                    if (view.ShowDialog() == true)
                    {
                        serviceUnitOfWork.Attachments.Update(selectedAttachment);
                        Attachments = serviceUnitOfWork.Attachments.GetAll();
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

                }));
            }
        }

        public AttachmentsViewModel(IServiceUnitOfWork serviceUnitOfWork)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
            Attachments = serviceUnitOfWork.Attachments.GetAll(); 
        }
    }
}
