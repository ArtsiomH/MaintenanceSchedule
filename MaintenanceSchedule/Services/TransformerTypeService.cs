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
    class TransformerTypeService : ITransformerTypeService
    {
        IUnitOfWork dataBase;

        public TransformerTypeService(IUnitOfWork dataBase)
        {
            this.dataBase = dataBase;
        }

        public void Create(TransformerType t)
        {
            dataBase.TransformerTypes.Create(t);
            dataBase.Save();
        }

        public void Delete(TransformerType t)
        {
            dataBase.TransformerTypes.Delete(t);
            dataBase.Save();
        }

        public TransformerType Get(int id)
        {
            return dataBase.TransformerTypes.Read(id);
        }

        public ObservableCollection<TransformerType> GetAll()
        {
            return new ObservableCollection<TransformerType>(dataBase.TransformerTypes.GetAll());
        }

        public void Update(TransformerType t)
        {
            dataBase.TransformerTypes.Update(t);
            dataBase.Save();
        }
    }
}
