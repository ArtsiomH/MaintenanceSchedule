using MaintenanceSchedule.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaintenanceSchedule.Model;
using System.Collections.ObjectModel;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Entities;
using System.Collections;

namespace MaintenanceSchedule.Services
{
    class RelayDeviceModelService : IRelayDeviceModelService
    {
        IUnitOfWork dataBase;

        public RelayDeviceModelService(IUnitOfWork dataBase)
        {
            this.dataBase = dataBase;
        }

        public RelayDeviceModel Get(int deviceId)
        {
            throw new NotImplementedException();
        }        

        public ObservableCollection<RelayDeviceModel> GetAll(Attachment attachment)
        {
            return getCollectionDeviceModel(attachment.RelayDevices);
        }

        public ObservableCollection<RelayDeviceModel> GetAll(AdditionalWork combineDevice)
        {
            return getCollectionDeviceModel(combineDevice.Devices);
        }

        private ObservableCollection<RelayDeviceModel> getCollectionDeviceModel(IEnumerable<RelayDevice> relayDevices)
        {
            ObservableCollection<RelayDeviceModel> deviceModels = new ObservableCollection<RelayDeviceModel>();
            List<RelayDevice> devices = dataBase.RelayDevices.GetAll().Intersect(relayDevices).ToList();
            foreach (RelayDevice device in devices)
            {
                RelayDevice relayDevice = dataBase.RelayDevices.Read(device.MaintainedEquipmentId);
                RelayDeviceModel relayDeviceModel = new RelayDeviceModel();
                relayDeviceModel.MaintainedEquipmentId = relayDevice.MaintainedEquipmentId;                
                relayDeviceModel.Name = relayDevice.Name;
                relayDeviceModel.InputYear = relayDevice.InputYear.Value;
                relayDeviceModel.MaintenancePeriod = relayDevice.MaintenancePeriod;
                if (relayDevice.LastRecovery != null)
                {
                    relayDeviceModel.LastRecovery = relayDevice.LastRecovery.Value;
                }
                MaintenanceRecord record = relayDevice.MaintenanceRecords.Where(x => x.ActualMaintenanceDate != null).Last();
                relayDeviceModel.LastMaintenanceDate = record.ActualMaintenanceDate.Value;
                relayDeviceModel.LastMaintenanceType = record.ActualMaintenanceType;
                record = relayDevice.MaintenanceRecords.Where(x => x.ActualMaintenanceDate == null).First();
                relayDeviceModel.ActualMaintenanceDate = record.PlannedMaintenanceDate;
                relayDeviceModel.ActualMaintenanceType = record.PlannedMaintenanceType;
                relayDeviceModel.ElementBase = relayDevice.ElementBase;
                deviceModels.Add(relayDeviceModel);
            }
            return deviceModels;
        }
    }
}
