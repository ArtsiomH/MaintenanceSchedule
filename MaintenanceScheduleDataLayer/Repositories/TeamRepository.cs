using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Entities;
using MaintenanceScheduleDataLayer.EFContext;
using System;
using System.Threading.Tasks;

namespace MaintenanceScheduleDataLayer.Repositories
{
    class TeamRepository : IRepository<Team>
    {
        private MaintenanceScheduleContext context;

        public TeamRepository(MaintenanceScheduleContext context)
        {
            this.context = context;
        }

        public void Create(Team t)
        {
            context.Teams.Add(t);
        }
                
        public Team Read(int id)
        {
            return context.Teams.First(x => x.TeamId == id);
        }

        public void Update(Team t)
        {
            Team oldTeam = Read(t.TeamId);
            UpdateEntityReflection.Update(oldTeam, t);
            context.Entry(oldTeam).State = EntityState.Modified;
        }

        public void Delete(Team t)
        {
            Team team = Read(t.TeamId);
            context.Teams.Remove(team);
        }

        public IEnumerable<Team> GetAll()
        {
            return context.Teams.ToList();
        }

        public Task<Team> ReadAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
