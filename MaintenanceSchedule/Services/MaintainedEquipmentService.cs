using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaintenanceSchedule.Interfaces;
using MaintenanceScheduleDataLayer.Entities;
using MaintenanceScheduleDataLayer.Interfaces;

namespace MaintenanceSchedule.Services
{
    class MaintainedEquipmentService : IMaintainedEquipmentService
    {
        IUnitOfWork dataBase;

        public MaintainedEquipmentService(IUnitOfWork dataBase)
        {
            this.dataBase = dataBase;
        }

        public void Create(MaintainedEquipment t)
        {
            throw new NotImplementedException();
        }

        public void Delete(MaintainedEquipment t)
        {
            throw new NotImplementedException();
        }

        public MaintainedEquipment Get(int id)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<MaintainedEquipment> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(MaintainedEquipment t)
        {
            throw new NotImplementedException();
        }
    }
}
