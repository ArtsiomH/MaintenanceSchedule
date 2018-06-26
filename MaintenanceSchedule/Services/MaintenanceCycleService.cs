using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaintenanceSchedule.Interfaces;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Repositories;
using MaintenanceScheduleDataLayer.Entities;

namespace MaintenanceSchedule.Services
{
    class MaintenanceCycleService : BaseMaintenanceCycleService, IMaintenanceCycleService
    {
        public MaintenanceCycleService(IUnitOfWork dataBase) : base(dataBase)
        {   }

        public void Create(MaintenanceCycle t)
        {
            dataBase.MaintenanceCycles.Create(t);
        }

        public void Delete(MaintenanceCycle t)
        {
            if(GetAllMaintainedEquipments(t.MaintenanceCycleId).Count != 0) return;
            dataBase.MaintenanceCycles.Delete(t);
        }

        public void Delete(MaintenanceCycle deletedMaintenanceCycle, MaintenanceCycle newMaintenanceCycle)
        {
            List<MaintainedEquipmentByCycle> equipments = GetAllMaintainedEquipments(deletedMaintenanceCycle.MaintenanceCycleId);
            if (equipments.Count != 0)
            {
                foreach (MaintainedEquipmentByCycle equipment in equipments)
                {
                    if (equipment is Device)
                    {
                        BaseDeviceService deviceService = new BaseDeviceService(dataBase);
                        deviceService.UpdateRecords(deletedMaintenanceCycle, newMaintenanceCycle, (Device)equipment);
                        
                    }
                    else if (equipment is AdditionalWork)
                    {
                        AdditionalWorkService combineDeviceService = new AdditionalWorkService(dataBase);
                        combineDeviceService.UpdateRecords(deletedMaintenanceCycle, newMaintenanceCycle, (AdditionalWork)equipment);
                    }                    
                }
                dataBase.Save();
            }

        }

        public MaintenanceCycle Get(int id)
        {
            return dataBase.MaintenanceCycles.Read(id);
        }

        public ObservableCollection<MaintenanceCycle> GetAll()
        {
            return new ObservableCollection<MaintenanceCycle>(dataBase.MaintenanceCycles.GetAll());
        }

        public void Update(MaintenanceCycle t)
        {
            dataBase.MaintenanceCycles.Update(t);
        }
    }
}
