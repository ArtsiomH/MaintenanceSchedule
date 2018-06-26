using MaintenanceSchedule.Model;
using MaintenanceScheduleDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceSchedule.Interfaces
{
    interface IMaintenanceCycleModelService
    {
        void Create(MaintenanceCycleModel maintenanceCycleModel);
        MaintenanceCycleModel Get(MaintenanceCycle maintenanceCycle);
        ObservableCollection<MaintenanceCycleModel> GetAll();
        void Update(MaintenanceCycleModel maintenanceCycleModel);
        List<MaintainedEquipmentByCycle> GetAllMaintainedEquipments(int id);
    }
}
