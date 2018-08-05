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
    class AdditionalWorkModelService : IAdditionalWorkModelService
    {
        IUnitOfWork dataBase;

        public AdditionalWorkModelService(IUnitOfWork dataBase)
        {
            this.dataBase = dataBase;
        }

        public ObservableCollection<AdditionalWorkModel> GetAll(Substation substation)
        {
            ObservableCollection<AdditionalWorkModel> additionalWorkModels = new ObservableCollection<AdditionalWorkModel>();
            foreach (AdditionalWork combineDevice in substation.AdditionalWorks)
            {
                AdditionalWorkModel additionalWorkModel = new AdditionalWorkModel();
                additionalWorkModel.MaintainedEquipmentId = combineDevice.MaintainedEquipmentId;
                additionalWorkModel.InputYear = combineDevice.InputYear;
                additionalWorkModel.Name = combineDevice.Name;
                MaintenanceRecord record = combineDevice.MaintenanceRecords.Where(x => x.ActualMaintenanceDate != null).Last();
                additionalWorkModel.LastMaintenanceDate = record.ActualMaintenanceDate.Value;
                additionalWorkModel.LastMaintenanceType = record.ActualMaintenanceType;
                record = combineDevice.MaintenanceRecords.Where(x => x.ActualMaintenanceDate == null).First();
                additionalWorkModel.PlannedMaintenanceDate = record.PlannedMaintenanceDate;
                additionalWorkModel.PlannedMaintenanceType = record.PlannedMaintenanceType;
                additionalWorkModels.Add(additionalWorkModel);
            }
            return additionalWorkModels;
        }

        public ObservableCollection<AdditionalWorkModel> GetAll(RelayDevice device)
        {
            ObservableCollection<AdditionalWorkModel> additionalWorkModels = new ObservableCollection<AdditionalWorkModel>();
            foreach (AdditionalWork combineDevice in device.CombineDevices)
            {
                AdditionalWorkModel additionalWorkModel = new AdditionalWorkModel();
                additionalWorkModel.MaintainedEquipmentId = combineDevice.MaintainedEquipmentId;
                additionalWorkModel.Name = combineDevice.Name;
                additionalWorkModel.InputYear = combineDevice.InputYear;
                MaintenanceRecord record = combineDevice.MaintenanceRecords.Where(x => x.ActualMaintenanceDate != null).Last();
                additionalWorkModel.LastMaintenanceDate = record.ActualMaintenanceDate.Value;
                additionalWorkModel.LastMaintenanceType = record.ActualMaintenanceType;
                record = combineDevice.MaintenanceRecords.Where(x => x.ActualMaintenanceDate == null).First();
                additionalWorkModel.PlannedMaintenanceDate = record.PlannedMaintenanceDate;
                additionalWorkModel.PlannedMaintenanceType = record.PlannedMaintenanceType;
                additionalWorkModels.Add(additionalWorkModel);
            }
            return additionalWorkModels;
        }
    }
}
