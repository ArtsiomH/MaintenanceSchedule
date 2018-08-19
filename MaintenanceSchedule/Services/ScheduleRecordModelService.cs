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
using System.Globalization;

namespace MaintenanceSchedule.Services
{
    class ScheduleRecordModelService : IScheduleRecordModelService
    {
        private IUnitOfWork dataBase;
		private static readonly object lockObject = new object();


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
				m_semaphore.WaitOne();
				if (equipment is Substation)
				{						
					AddScheduleRecordModelForSubstationAsync(equipment, year, serviceUnitOfWork);
					
				}
				else
				{
					AddScheduleRecordModelAsync(equipment, year, serviceUnitOfWork);
				}
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
                MaintenanceRecord lastMaintenanceRecord = maintainedEquipment.MaintenanceRecords
                    .LastOrDefault(x => x.ActualMaintenanceDate != null && x.ActualMaintenanceDate.Value.Year != year);

				if (lastMaintenanceRecord != null)
				{
					scheduleRecord.LastMaintenanceDate = lastMaintenanceRecord.PlannedMaintenanceDate;
					scheduleRecord.LastMaintenanceType = lastMaintenanceRecord.PlannedMaintenanceType;
				}                

                MaintenanceRecord plannedMaitenanceRecored = maintainedEquipment.MaintenanceRecords
                    .LastOrDefault(x => x.PlannedMaintenanceDate.Year == year);

                scheduleRecord.PlannedMaintenanceDate = plannedMaitenanceRecored.PlannedMaintenanceDate;
                scheduleRecord.PlannedMaintenanceType = plannedMaitenanceRecored.PlannedMaintenanceType;
				if (plannedMaitenanceRecored.IsPlanned == true)
				{
					scheduleRecord.PlannedMonth = DateTimeFormatInfo.CurrentInfo.GetMonthName(plannedMaitenanceRecored.PlannedMaintenanceDate.Month);
					scheduleRecord.PlannedDay = plannedMaitenanceRecored.PlannedMaintenanceDate.Day;
				}

                scheduleRecord.ActualMaintenanceDate = plannedMaitenanceRecored.ActualMaintenanceDate;
                scheduleRecord.ActualMaintenanceType = plannedMaitenanceRecored.ActualMaintenanceType;
				if (plannedMaitenanceRecored.ActualMaintenanceDate != null && plannedMaitenanceRecored.IsPlanned == true)
				{
					scheduleRecord.ActualMonth = DateTimeFormatInfo.CurrentInfo.GetMonthName(plannedMaitenanceRecored.ActualMaintenanceDate.Value.Month);
					scheduleRecord.ActualDay = plannedMaitenanceRecored.ActualMaintenanceDate.Value.Day;
				}

				scheduleRecord.MaintainedEquipmentId = plannedMaitenanceRecored.MaintainedEquipmentId;
				scheduleRecord.MaintenanceRecordId = plannedMaitenanceRecored.MaintenanceRecordId;

				scheduleRecord.IsPlanned = plannedMaitenanceRecored.IsPlanned;			

				lock (lockObject)
				{
					m_scheduleRecordModel.Add(scheduleRecord);
				}				

                m_semaphore.Release();

            });            
        }

		private async void AddScheduleRecordModelForSubstationAsync(MaintainedEquipment maintainedEquipment, int year, IServiceUnitOfWork serviceUnitOfWork)
		{
			await Task.Run(() =>
			{
				if (maintainedEquipment is Substation)
				{					
						ScheduleRecordModel scheduleRecord = new ScheduleRecordModel();
					scheduleRecord.MaintenanceTypes = new List<string>();

					Substation substation = dataBase.Substations
						.ReadAsync(maintainedEquipment.MaintainedEquipmentId).Result;

					scheduleRecord.Substation = substation.Name;
					scheduleRecord.Attachment = s_allAttachments;
					scheduleRecord.Name = s_allEquipments;
					scheduleRecord.MaintenanceTypes.Add("осмотр");					

					List<MaintenanceRecord> previousYearRecords = maintainedEquipment.MaintenanceRecords.Where(x => x.PlannedMaintenanceDate.Year == year - 1).ToList();

					List<MaintenanceRecord> records = maintainedEquipment.MaintenanceRecords.Where(x => x.PlannedMaintenanceDate.Year == year).ToList();
					
					for (int i = 0; i < records.Count; i++)
					{
						MaintenanceRecord lastMaintenanceRecord = new MaintenanceRecord();
						if (previousYearRecords.Count == 0)
						{
							scheduleRecord.LastMaintenanceDate = null;
							scheduleRecord.LastMaintenanceType = null;
						}
						else
						{
							scheduleRecord.LastMaintenanceDate = previousYearRecords[i].ActualMaintenanceDate;
							scheduleRecord.LastMaintenanceType = previousYearRecords[i].ActualMaintenanceType;
						}						

						scheduleRecord.PlannedMaintenanceDate = records[i].PlannedMaintenanceDate;
						scheduleRecord.PlannedMaintenanceType = records[i].PlannedMaintenanceType;
						if (records[i].IsPlanned == true)
						{
							scheduleRecord.PlannedMonth = DateTimeFormatInfo.CurrentInfo.GetMonthName(records[i].PlannedMaintenanceDate.Month);
							scheduleRecord.PlannedDay = records[i].PlannedMaintenanceDate.Day;
						}

						scheduleRecord.ActualMaintenanceDate = records[i].ActualMaintenanceDate;
						scheduleRecord.ActualMaintenanceType = records[i].ActualMaintenanceType;
						if (records[i].ActualMaintenanceDate.HasValue && records[i].IsPlanned == true)
						{
							scheduleRecord.ActualMonth = DateTimeFormatInfo.CurrentInfo.GetMonthName(records[i].ActualMaintenanceDate.Value.Month);
							if (records[i].ActualMaintenanceDate.Value.Day != 1)
							{
								scheduleRecord.ActualDay = records[i].ActualMaintenanceDate.Value.Day;
							}
						}

						scheduleRecord.MaintainedEquipmentId = records[i].MaintainedEquipmentId;
						scheduleRecord.MaintenanceRecordId = records[i].MaintenanceRecordId;

						scheduleRecord.IsPlanned = records[i].IsPlanned;

						lock (lockObject)
						{
							m_scheduleRecordModel.Add(scheduleRecord);
						}						
					}

					m_semaphore.Release();
				}			
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

			scheduleRecord.IsPlanned = plannedMaitenanceRecored.IsPlanned;

            m_scheduleRecordModel.Add(scheduleRecord);
        }
		
		public void SetPlannedMonth(ScheduleRecordModel scheduleRecordModel, string month)
		{
			if (scheduleRecordModel == null) return;
			MaintenanceRecord record = GetCurrentRecord(scheduleRecordModel);
			if (string.IsNullOrEmpty(month))
			{
				record.IsPlanned = false;
				record.ActualMaintenanceDate = null;
				record.ActualMaintenanceType = null;
				record.PlannedMaintenanceDate = GetNewDate(record, null);
				scheduleRecordModel.IsPlanned = false;
				scheduleRecordModel.PlannedMonth = string.Empty;
				scheduleRecordModel.ActualMonth = string.Empty;
				scheduleRecordModel.ActualMaintenanceType = null;
			}
			else
			{
				record.IsPlanned = true;
				scheduleRecordModel.IsPlanned = true;
				scheduleRecordModel.PlannedMonth = month;
				record.PlannedMaintenanceDate = GetNewDate(record, month);
			}
			dataBase.MaintenanceRecords.Update(record);
			dataBase.Save();
		}
		
		public void SetActualMonth(ScheduleRecordModel scheduleRecordModel, string month)
		{
			if (scheduleRecordModel == null || string.IsNullOrEmpty(month)) return;
			MaintenanceRecord record = GetCurrentRecord(scheduleRecordModel);
			record.ActualMaintenanceDate = GetNewDate(record, month);
			dataBase.MaintenanceRecords.Update(record);
			dataBase.Save();
		}

		public void SetActualType(ScheduleRecordModel scheduleRecordModel, string type)
		{
			if (scheduleRecordModel == null || string.IsNullOrEmpty(type)) return;
			MaintenanceRecord record = GetCurrentRecord(scheduleRecordModel);
			MaintenanceType maintenanceType = dataBase.MaintenanceTypes.GetAll().FirstOrDefault(x => x.Name == type);
			if (maintenanceType != null)
			{
				record.ActualMaintenanceType = maintenanceType;
				dataBase.MaintenanceRecords.Update(record);
				dataBase.Save();
			}			
		}

		private MaintenanceRecord GetCurrentRecord(ScheduleRecordModel scheduleRecordModel)
		{
			MaintainedEquipment equipment = dataBase.MaintainedEquipments.Read(scheduleRecordModel.MaintainedEquipmentId);
			return equipment.MaintenanceRecords.Find(x => x.MaintenanceRecordId == scheduleRecordModel.MaintenanceRecordId);
		}

		private DateTime GetNewDate(MaintenanceRecord record, string month)
		{
			if (string.IsNullOrEmpty(month)) return new DateTime(record.PlannedMaintenanceDate.Year, 1, 1);
			string[] array = DateTimeFormatInfo.CurrentInfo.MonthNames;
			DateTime dateTime = record.PlannedMaintenanceDate;
			if (!array.Contains(month)) return DateTime.Now;
			return new DateTime(dateTime.Year, Array.IndexOf(array, month) + 1, dateTime.Day);
		}
	}
}
