using MaintenanceSchedule.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaintenanceScheduleDataLayer.Entities;
using System.Collections.ObjectModel;
using MaintenanceScheduleDataLayer.Interfaces;

namespace MaintenanceSchedule.Services
{
    class ScheduleService : IScheduleService
    {
        IUnitOfWork dataBase;

        private readonly string s_inDeveloping = "В разработке";
        private readonly string s_signed = "Подписан";

        public ScheduleService(IUnitOfWork dataBase)
        {
            this.dataBase = dataBase;
        }

        public void Create(Schedule t)
        {
            IEnumerable<Schedule> schedules = dataBase.Schedules.GetAll();
            if (schedules.Count() == 0)
            {
                t.Year = DateTime.Now.Year + 1;
                t.Condition = s_inDeveloping;
                dataBase.Schedules.Create(t);
                dataBase.Save();
            }
            else
            {                
                Schedule lastInDevelopingSchedule = schedules.LastOrDefault(x => x.Condition != s_inDeveloping);
                if (lastInDevelopingSchedule == null)
                {
                    Schedule lastSignedSchedule = schedules.LastOrDefault(x => x.Condition == s_inDeveloping);
                    t.Year = lastSignedSchedule.Year + 1;
                    t.Condition = s_inDeveloping;
                    dataBase.Schedules.Create(t);
                    dataBase.Save();
                }
            }                 
        }

        public void Delete(Schedule t)
        {
            if (dataBase.Schedules.GetAll().LastOrDefault(x => x.Condition == s_inDeveloping) != null)
            {
                dataBase.Schedules.Delete(t);
            }
        }

        public Schedule Get(int id)
        {
            return dataBase.Schedules.Read(id);
        }

        public ObservableCollection<Schedule> GetAll()
        {
            return new ObservableCollection<Schedule>(dataBase.Schedules.GetAll());
        }

        public void Update(Schedule t)
        {
            dataBase.Schedules.Update(t);
        }
    }
}
