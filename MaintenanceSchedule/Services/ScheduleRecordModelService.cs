using MaintenanceSchedule.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaintenanceScheduleDataLayer.Entities;
using System.Collections.ObjectModel;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceSchedule.Model;
using System.Threading;

namespace MaintenanceSchedule.Services
{
    class ScheduleRecordModelService : IScheduleRecordModelService
    {
        private IUnitOfWork dataBase;

        private readonly string s_allAttachments = "Все присоединения";
        private readonly string s_allEquipments = "Все устройства";
        private List<ScheduleRecordModel> m_scheduleRecordModel = new List<ScheduleRecordModel>();
        private Semaphore m_semaphore;
        

        public ScheduleRecordModelService(IUnitOfWork dataBase)
        {
            this.dataBase = dataBase;           
            m_semaphore = new Semaphore(10, 10);
        }

        public ObservableCollection<ScheduleRecordModel> GetAll(int year, IServiceUnitOfWork serviceUnitOfWork)
        {
            List<MaintainedEquipment> equipments = new List<MaintainedEquipment>(dataBase.MaintainedEquipments.GetAll()
                .Where(x => x.MaintenanceRecords.LastOrDefault(y => y.PlannedMaintenanceDate.Year == year) != null));                     
            
            foreach (MaintainedEquipment equipment in equipments)
            {
                //AddScheduleRecordModel(equipment, year);
                AddScheduleRecordModelAsync(equipment, year, serviceUnitOfWork);
                m_semaphore.WaitOne();
            }
                        
            return new ObservableCollection<ScheduleRecordModel>(m_scheduleRecordModel);
        }

        private async void AddScheduleRecordModelAsync(MaintainedEquipment maintainedEquipment, int year, IServiceUnitOfWork serviceUnitOfWork)
        {
            await Task.Run(() =>
            {
                ScheduleRecordModel scheduleRecord = new ScheduleRecordModel();
                scheduleRecord.MaintenanceTypes = new List<string>();
                if (maintainedEquipment is Substation)
                {
                    Substation substation = dataBase.Substations
                        .ReadAsync(maintainedEquipment.MaintainedEquipmentId).Result;

                    scheduleRecord.Substation = substation.Name;
                    scheduleRecord.Attachment = s_allAttachments;
                    scheduleRecord.Name = s_allEquipments;
                    scheduleRecord.MaintenanceTypes.Add("осмотр");
                }
                else if (maintainedEquipment is AdditionalWork)
                {
                    AdditionalWork work = dataBase.AdditionalWorks
                        .ReadAsync(maintainedEquipment.MaintainedEquipmentId).Result;

                    scheduleRecord.Substation = work.Substation.Name;
                    scheduleRecord.Attachment = null;
                    scheduleRecord.Name = work.Name;
                    scheduleRecord.MaintenanceTypes = GetMaintenanceTypes(work, serviceUnitOfWork);
                }
                else if (maintainedEquipment is AdditionalDevice)
                {
                    AdditionalDevice additionalDevice = dataBase.AdditionalDevices
                        .ReadAsync(maintainedEquipment.MaintainedEquipmentId).Result;

                    scheduleRecord.Substation = additionalDevice.Attachment.Substation.Name;
                    scheduleRecord.Attachment = additionalDevice.Attachment.Name;
                    scheduleRecord.Name = additionalDevice.Name;
                    scheduleRecord.MaintenanceTypes = GetMaintenanceTypes(additionalDevice, serviceUnitOfWork);
                }
                else if (maintainedEquipment is RelayDevice)
                {
                    RelayDevice relayDevice = dataBase.RelayDevices
                        .ReadAsync(maintainedEquipment.MaintainedEquipmentId).Result;

                    scheduleRecord.Substation = relayDevice.Attachment.Substation.Name;
                    scheduleRecord.Attachment = relayDevice.Attachment.Name;
                    scheduleRecord.Name = relayDevice.Name;
                    scheduleRecord.MaintenanceTypes = GetMaintenanceTypes(relayDevice, serviceUnitOfWork);
                }
                if (maintainedEquipment is MaintainedEquipmentByCycle)
                {
                    
                }
                MaintenanceRecord lastMaintenanceRecord = maintainedEquipment.MaintenanceRecords
                    .LastOrDefault(x => x.ActualMaintenanceDate != null);

                scheduleRecord.LastMaintenanceDate = lastMaintenanceRecord.PlannedMaintenanceDate;
                scheduleRecord.LastMaintenanceType = lastMaintenanceRecord.PlannedMaintenanceType;

                MaintenanceRecord plannedMaitenanceRecored = maintainedEquipment.MaintenanceRecords
                    .LastOrDefault(x => x.PlannedMaintenanceDate.Year == year);

                scheduleRecord.PlannedMaintenanceDate = plannedMaitenanceRecored.PlannedMaintenanceDate;
                scheduleRecord.PlannedMaintenanceType = plannedMaitenanceRecored.PlannedMaintenanceType;

                scheduleRecord.ActualMaintenanceDate = plannedMaitenanceRecored.ActualMaintenanceDate;
                scheduleRecord.ActualMaintenanceType = plannedMaitenanceRecored.ActualMaintenanceType;

                m_scheduleRecordModel.Add(scheduleRecord);

                m_semaphore.Release();

            });            
        }

        private List<string> GetMaintenanceTypes(MaintainedEquipmentByCycle equipment, IServiceUnitOfWork serviceUnitOfWork)
        {
            MaintenanceCycle maintenanceCycle = serviceUnitOfWork.MaintainedEquipmentsByCycleService
                       .GetCurrentCycle(equipment as MaintainedEquipmentByCycle);

            return maintenanceCycle.MaintenanceYears.GroupBy(x => x.MaintenanceType.Name).Select(x => x.Key).ToList();
        }

        private void AddScheduleRecordModel(MaintainedEquipment maintainedEquipment, int year)
        {
            ScheduleRecordModel scheduleRecord = new ScheduleRecordModel();
            if (maintainedEquipment is Substation)
            {
                Substation substation = dataBase.Substations.Read(maintainedEquipment.MaintainedEquipmentId);
                scheduleRecord.Substation = substation.Name;
                scheduleRecord.Attachment = s_allAttachments;
                scheduleRecord.Name = s_allEquipments;
            }
            else if (maintainedEquipment is AdditionalWork)
            {
                AdditionalWork work = dataBase.AdditionalWorks.Read(maintainedEquipment.MaintainedEquipmentId);
                scheduleRecord.Substation = work.Substation.Name;
                scheduleRecord.Attachment = null;
                scheduleRecord.Name = work.Name;
            }
            else if (maintainedEquipment is AdditionalDevice)
            {
                AdditionalDevice additionalDevice = dataBase.AdditionalDevices.Read(maintainedEquipment.MaintainedEquipmentId); ;
                scheduleRecord.Substation = additionalDevice.Attachment.Substation.Name;
                scheduleRecord.Attachment = additionalDevice.Attachment.Name;
                scheduleRecord.Name = additionalDevice.Name;
            }
            else if (maintainedEquipment is RelayDevice)
            {
                RelayDevice relayDevice = dataBase.RelayDevices.Read(maintainedEquipment.MaintainedEquipmentId);

                scheduleRecord.Substation = relayDevice.Attachment.Substation.Name;
                scheduleRecord.Attachment = relayDevice.Attachment.Name;
                scheduleRecord.Name = relayDevice.Name;
            }
            MaintenanceRecord lastMaintenanceRecord = maintainedEquipment.MaintenanceRecords
                .LastOrDefault(x => x.ActualMaintenanceDate != null);

            scheduleRecord.LastMaintenanceDate = lastMaintenanceRecord.PlannedMaintenanceDate;
            scheduleRecord.LastMaintenanceType = lastMaintenanceRecord.PlannedMaintenanceType;

            MaintenanceRecord plannedMaitenanceRecored = maintainedEquipment.MaintenanceRecords
                .LastOrDefault(x => x.PlannedMaintenanceDate.Year == year);

            scheduleRecord.PlannedMaintenanceDate = plannedMaitenanceRecored.PlannedMaintenanceDate;
            scheduleRecord.PlannedMaintenanceType = plannedMaitenanceRecored.PlannedMaintenanceType;

            scheduleRecord.ActualMaintenanceDate = plannedMaitenanceRecored.ActualMaintenanceDate;
            scheduleRecord.ActualMaintenanceType = plannedMaitenanceRecored.ActualMaintenanceType;

            m_scheduleRecordModel.Add(scheduleRecord);
        }
    }
}
