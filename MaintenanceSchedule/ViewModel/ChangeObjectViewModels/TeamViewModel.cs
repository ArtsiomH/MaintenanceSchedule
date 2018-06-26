using MaintenanceSchedule.Commands;
using MaintenanceSchedule.Interfaces;
using MaintenanceScheduleDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MaintenanceSchedule.ViewModel.ChangeObjectViewModels
{
    class TeamViewModel : BaseViewModel, IDataErrorInfo
    {
        private Team team;
        private Team oldTeam;

        private RelayCommand save;
        private RelayCommand cancel;
        private bool check = false;
        private List<string> errors = new List<string>();

        public Team Team
        {
            get
            {
                return team;
            }
            set
            {
                team = value;
                OnProtpertyChange(nameof(Team));
            }
        }

        public string Name
        {
            get
            {
                return team.Name;
            }
            set
            {
                team.Name = value;
                OnProtpertyChange(nameof(Name));
            }
        }

        public string Leader
        {
            get
            {
                return team.Leader;
            }
            set
            {
                team.Leader = value;
                OnProtpertyChange(nameof(Leader));
            }
        }

        public RelayCommand Save
        {
            get
            {
                return save ?? (save = new RelayCommand(o =>
                {                    
                    oldTeam.Name = Name;
                    oldTeam.Leader = Leader;
                    ((Window)o).DialogResult = true;
                },
                o => check));
            }
        }

        public RelayCommand Cancel
        {
            get
            {
                return cancel ?? (cancel = new RelayCommand(o =>
                {
                    ((Window)o).DialogResult = false;
                }));
            }
        }

        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case nameof(Name):
                        {
                            if (Name == null) break;
                            error = "Поле не должно быть пустым";
                            if (Name == string.Empty) break;
                            errors.Remove(error + columnName);
                            error = "Необходимо уникальное название";
                            if (serviceUnitOfWork.Teams.GetAll().FirstOrDefault(x => x.Name == Name) != null) break;
                            errors.Remove(error + columnName);
                            error = string.Empty;
                            break;
                        }

                    case nameof(Leader):
                        {
                            if (Leader == null) break;
                            error = "Поле не должно быть пустым";
                            if (Leader == string.Empty) break;
                            errors.Remove(error + columnName);
                            error = "Необходимо уникальное название";
                            if (serviceUnitOfWork.Teams.GetAll().FirstOrDefault(x => x.Leader == Leader) != null) break;
                            errors.Remove(error + columnName);
                            error = string.Empty;
                            break;
                        }
                }
                string errorRecord = error + columnName;
                if (error != string.Empty && !errors.Contains(errorRecord)) errors.Add(errorRecord);
                if (Name == null ||
                    Leader == null ||
                    errors.Count != 0) check = false;
                else check = true;
                return error;
            }
        }

        public TeamViewModel(IServiceUnitOfWork serviceUnitOfWork, Team team)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
            oldTeam = team;
            Team newTeam = new Team();            
            newTeam.Name = team.Name;
            newTeam.Leader = team.Leader;
            Team = newTeam;
        }
    }
}
