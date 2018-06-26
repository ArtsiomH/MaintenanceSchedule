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

namespace MaintenanceSchedule.Services
{
    class MaintenanceCycleModelService : BaseMaintenanceCycleService, IMaintenanceCycleModelService
    {
        public MaintenanceCycleModelService(IUnitOfWork dataBase) : base(dataBase)
        {    }

        public void Create(MaintenanceCycleModel maintenanceCycleModel)
        {
            MaintenanceCycle maintenanceCycle = new MaintenanceCycle();
            MaintenanceYear maintenanceYear;
            List<MaintenanceType> maintenanceTypes = new List<MaintenanceType>(dataBase.MaintenanceTypes.GetAll());
            maintenanceCycle.Name = maintenanceCycleModel.Name;
            maintenanceCycle.ShowName = maintenanceCycleModel.ShowName;
            maintenanceCycle.MaintenanceYears = new List<MaintenanceYear>();
            dataBase.MaintenanceCycles.Create(maintenanceCycle);            
            for (int i = 0; i < maintenanceCycleModel.MaintenanceTypes.Length; i++)
            {                
                if ( maintenanceCycleModel.MaintenanceTypes[i] != null)
                {
                    maintenanceYear = new MaintenanceYear();
                    maintenanceYear.Year = i;
                    maintenanceYear.MaintenanceType = maintenanceTypes.First(x => x.Name == maintenanceCycleModel.MaintenanceTypes[i]);
                    maintenanceYear.MaintenanceCycle = maintenanceCycle;
                    dataBase.MaintenanceYears.Create(maintenanceYear);                                     
                }                
            }
            dataBase.MaintenanceCycles.Create(maintenanceCycle);
            dataBase.Save();
        }

        public MaintenanceCycleModel Get(MaintenanceCycle maintenanceCycle)
        {
            MaintenanceCycleModel maintenanceCycleModel = new MaintenanceCycleModel();
            maintenanceCycleModel.MaintenanceTypes = new string[9];
            maintenanceCycleModel.MaintenanceCycleId = maintenanceCycle.MaintenanceCycleId;
            maintenanceCycleModel.Name = maintenanceCycle.Name;
            maintenanceCycleModel.ShowName = maintenanceCycle.ShowName;
            MaintenanceYear maintenanceYear = new MaintenanceYear();
            for (int i = 0; i <= 8; i++)
            {
                maintenanceYear = maintenanceCycle.MaintenanceYears.FirstOrDefault(x => x.Year == i);
                if (maintenanceYear != null)
                {
                    maintenanceCycleModel.MaintenanceTypes[i] = maintenanceYear.MaintenanceType.Name;
                }
            }
            return maintenanceCycleModel;
        }

        public ObservableCollection<MaintenanceCycleModel> GetAll()
        {
            ObservableCollection<MaintenanceCycleModel> maintenanceCycleModels = new ObservableCollection<MaintenanceCycleModel>();
            foreach (MaintenanceCycle cycle in dataBase.MaintenanceCycles.GetAll())
            {
                MaintenanceCycleModel maintenanceCycleModel = Get(cycle);
                maintenanceCycleModels.Add(maintenanceCycleModel);              
            }
            return maintenanceCycleModels;
        }

        public void Update(MaintenanceCycleModel maintenanceCycleModel)
        {
            MaintenanceCycle maintenanceCycle = new MaintenanceCycle();
            MaintenanceCycle oldMaintenanceCycle = dataBase.MaintenanceCycles.Read(maintenanceCycleModel.MaintenanceCycleId);
            MaintenanceYear maintenanceYear;
            List<MaintenanceType> maintenanceTypes = new List<MaintenanceType>(dataBase.MaintenanceTypes.GetAll());
            while (oldMaintenanceCycle.MaintenanceYears.Count != 0)
            {
                dataBase.MaintenanceYears.Delete(oldMaintenanceCycle.MaintenanceYears.First());
            }
            dataBase.Save();           
            oldMaintenanceCycle.Name = maintenanceCycleModel.Name;
            oldMaintenanceCycle.ShowName = maintenanceCycleModel.ShowName;                                  
            for (int i = 0; i < maintenanceCycleModel.MaintenanceTypes.Length; i++)
            {
                if (maintenanceCycleModel.MaintenanceTypes[i] != null)
                {
                    maintenanceYear = new MaintenanceYear();
                    maintenanceYear.Year = i;
                    maintenanceYear.MaintenanceType = maintenanceTypes.First(x => x.Name == maintenanceCycleModel.MaintenanceTypes[i]);
                    maintenanceYear.MaintenanceCycle = oldMaintenanceCycle;
                    dataBase.MaintenanceYears.Create(maintenanceYear);
                }
            }
            //dataBase.MaintenanceCycles.Update(maintenanceCycle);
            dataBase.Save();            
            maintenanceCycle = dataBase.MaintenanceCycles.Read(oldMaintenanceCycle.MaintenanceCycleId);
            List<MaintainedEquipmentByCycle> maintainedEquipmentsByCycle = GetAllMaintainedEquipments(maintenanceCycle.MaintenanceCycleId);

            if (maintainedEquipmentsByCycle.Count != 0)
            {            
                foreach (MaintainedEquipmentByCycle equipment in maintainedEquipmentsByCycle)
                {
                    if (equipment is Device)
                    {                     
                        BaseDeviceService deviceService = new BaseDeviceService(dataBase);
                        deviceService.UpdateRecords(maintenanceCycle, (Device)equipment);
                    }
                    else if (equipment is AdditionalWork)
                    {   
                        AdditionalWorkService combineDeviceService = new AdditionalWorkService(dataBase);
                        combineDeviceService.UpdateRecords(maintenanceCycle, (AdditionalWork)equipment);                        
                    }               
                }
                dataBase.Save();
            }
        }
    }
}