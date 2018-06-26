using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaintenanceSchedule.Interfaces;
using MaintenanceScheduleDataLayer.Repositories;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Entities;

namespace MaintenanceSchedule.Services
{
    class TeamService : ITeamService
    {
        IUnitOfWork dataBase;

        public TeamService(IUnitOfWork dataBase)
        {
            this.dataBase = dataBase;
        }

        public void Create(Team t)
        {
            dataBase.Teams.Create(t);
            dataBase.Save();
        }

        public void Delete(Team t)
        {
            dataBase.Teams.Delete(t);
            dataBase.Save();
        }

        public Team Get(int id)
        {
            return dataBase.Teams.Read(id);            
        }

        public ObservableCollection<Team> GetAll()
        {
            return new ObservableCollection<Team>(dataBase.Teams.GetAll());
        }

        public void Update(Team t)
        {
            dataBase.Teams.Update(t);
            dataBase.Save();
        }
    }
}
