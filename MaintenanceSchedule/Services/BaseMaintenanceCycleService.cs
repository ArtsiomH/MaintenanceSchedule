using MaintenanceScheduleDataLayer.Entities;
using MaintenanceScheduleDataLayer.Interfaces;
using System.Collections.Generic;

namespace MaintenanceSchedule.Services
{
    class BaseMaintenanceCycleService
    {
        protected IUnitOfWork dataBase;

        public BaseMaintenanceCycleService(IUnitOfWork dataBase)
        {
            this.dataBase = dataBase;
        }

        public List<MaintainedEquipmentByCycle> GetAllMaintainedEquipments(int cycleId)
        {
            MaintenanceCycle maintenanceCycle = dataBase.MaintenanceCycles.Read(cycleId);
            List<MaintainedEquipmentByCycle> equipments = new List<MaintainedEquipmentByCycle>(maintenanceCycle.NormalEquipments);
            equipments.AddRange(maintenanceCycle.ReducedEquipments);
            return equipments;
        }
    }
}
