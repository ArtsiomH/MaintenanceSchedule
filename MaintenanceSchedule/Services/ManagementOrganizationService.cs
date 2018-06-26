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
    class ManagementOrganizationService : IManagementOrganizationService
    {
        IUnitOfWork dataBase;

        public ManagementOrganizationService(IUnitOfWork dataBase)
        {
            this.dataBase = dataBase;
        }

        public void Create(ManagementOrganization t)
        {
            throw new NotImplementedException();
        }

        public void Delete(ManagementOrganization t)
        {
            throw new NotImplementedException();
        }

        public ManagementOrganization Get(int id)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<ManagementOrganization> GetAll()
        {
            return new ObservableCollection<ManagementOrganization>(dataBase.ManagementOrganizations.GetAll());
        }

        public void Update(ManagementOrganization t)
        {
            throw new NotImplementedException();
        }
    }
}
