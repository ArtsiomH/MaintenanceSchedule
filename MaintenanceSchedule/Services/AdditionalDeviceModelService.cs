using MaintenanceSchedule.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaintenanceSchedule.Model;
using MaintenanceScheduleDataLayer.Entities;
using System.Collections.ObjectModel;
using MaintenanceScheduleDataLayer.Interfaces;

namespace MaintenanceSchedule.Services
{
    class AdditionalDeviceModelService : IAdditionalDeviceModelService
    {
        IUnitOfWork dataBase;

        public AdditionalDeviceModelService(IUnitOfWork dataBase)
        {
            this.dataBase = dataBase;
        }

        public ObservableCollection<AdditionalDeviceModel> GetAll(Attachment attachment)
        {
            ObservableCollection<AdditionalDeviceModel> additionalDeviceModels = new ObservableCollection<AdditionalDeviceModel>();
            foreach (AdditionalDevice additionalDevice in attachment.AdditionalDevices)
            {
                AdditionalDeviceModel additionalDeviceModel = new AdditionalDeviceModel();
                additionalDeviceModel.MaintainedEquipmentId = additionalDevice.MaintainedEquipmentId;
                additionalDeviceModel.Name = additionalDevice.Name;
                additionalDeviceModel.InputYear = additionalDevice.InputYear;
                additionalDeviceModel.MaintenancePeriod = additionalDevice.MaintenancePeriod;
                if (additionalDevice.LastRecovery != null)
                {
                    additionalDeviceModel.LastRecovery = additionalDevice.LastRecovery.Value;
                }
                MaintenanceRecord record = additionalDevice.MaintenanceRecords.Where(x => x.ActualMaintenanceDate != null).Last();
                additionalDeviceModel.LastMaintenanceDate = record.ActualMaintenanceDate.Value;
                additionalDeviceModel.LastMaintenanceType = record.ActualMaintenanceType;
                record = additionalDevice.MaintenanceRecords.Where(x => x.ActualMaintenanceDate == null).FirstOrDefault();
                if (record != null)
                {
                    additionalDeviceModel.PlannedMaintenanceDate = record.PlannedMaintenanceDate;
                    additionalDeviceModel.PlannedMaintenanceType = record.PlannedMaintenanceType;
                }                
                additionalDeviceModels.Add(additionalDeviceModel);
            }
            return additionalDeviceModels;
        }
    }
}
