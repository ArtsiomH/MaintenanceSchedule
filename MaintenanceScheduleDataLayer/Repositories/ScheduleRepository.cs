using System.Collections.Generic;
using System.Data.Entity;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Entities;
using MaintenanceScheduleDataLayer.EFContext;
using System;
using System.Threading.Tasks;

namespace MaintenanceScheduleDataLayer.Repositories
{
    class ScheduleRepository : IRepository<Schedule>
    {
        private MaintenanceScheduleContext context;

        public ScheduleRepository(MaintenanceScheduleContext context)
        {
            this.context = context;
        }

        public void Create(Schedule t)
        {
            context.Schedules.Add(t);
        }

        public void Delete(Schedule t)
        {
            Schedule schedule = Read(t.ScheduleId);
            context.Schedules.Remove(schedule);
        }

        public IEnumerable<Schedule> GetAll()
        {
            return context.Schedules;
        }

        public Schedule Read(int id)
        {
            return context.Schedules.Find(id);
        }

        public Task<Schedule> ReadAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Schedule t)
        {
            Schedule oldSchedule = Read(t.ScheduleId);
            UpdateEntityReflection.Update(oldSchedule, t);
            context.Entry(oldSchedule).State = EntityState.Modified;
        }
    }
}
