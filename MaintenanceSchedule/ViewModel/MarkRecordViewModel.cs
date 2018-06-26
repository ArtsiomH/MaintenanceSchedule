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

namespace MaintenanceSchedule.ViewModel
{
    class MarkRecordViewModel : BaseViewModel, IDataErrorInfo
    {
        private MaintainedEquipmentByCycle maintainedEquipmentByCycle;
        private ObservableCollection<MaintenanceType> maintenanceTypes;
        private MaintenanceType maintenanceType;
        private string date;
        private RelayCommand save;
        private RelayCommand cancel;
        private bool check = false;
        private List<string> errors = new List<string>();

        public MaintenanceType MaintenanceType
        {
            get
            {
                return maintenanceType;
            }
            set
            {
                maintenanceType = value;
                OnProtpertyChange(nameof(MaintenanceType));
            }
        }

        public ObservableCollection<MaintenanceType> MaintenanceTypes
        {
            get
            {
                return maintenanceTypes;
            }
            set
            {
                maintenanceTypes = value;
                OnProtpertyChange(nameof(MaintenanceTypes));
            }
        }

        public string Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
                OnProtpertyChange(nameof(Date));
            }
        }

        public RelayCommand Save
        {
            get
            {
                return save ?? (save = new RelayCommand(o =>
                {
                    serviceUnitOfWork.MaintainedEquipmentsByCycleService.MarkActualRecord(maintainedEquipmentByCycle, DateTime.Parse(date), MaintenanceType);
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
                    case nameof(Date):
                        {
                            if (Date == null) return string.Empty;
                            error = "Неверный формат даты";
                            DateTime newDate;
                            try
                            {
                                newDate = Convert.ToDateTime(Date);
                            }
                            catch (FormatException)
                            {
                                break;
                            }
                            error = "Необходимо указать дату";
                            if (Date == string.Empty) break;
                            errors.Remove(error + columnName);
                            error = string.Empty;
                            break;
                        }
                }
                string errorRecord = error + columnName;
                if (error != string.Empty && !errors.Contains(errorRecord)) errors.Add(errorRecord);
                if (errors.Count != 0 ||
                    MaintenanceType == null ||
                    Date == string.Empty) check = false;
                else check = true;
                return error;
            }
        }

        public MarkRecordViewModel(IServiceUnitOfWork serviceUnitOfWork, MaintainedEquipmentByCycle maintainedEquipmentByCycle)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
            this.maintainedEquipmentByCycle = maintainedEquipmentByCycle;
            int cycleId = serviceUnitOfWork.MaintainedEquipmentsByCycleService.GetCurrentCycle(maintainedEquipmentByCycle).MaintenanceCycleId;
            MaintenanceCycle maintenanceCycle = serviceUnitOfWork.MaintenanceCycles.Get(cycleId);
            MaintenanceTypes = new ObservableCollection<MaintenanceType>(maintenanceCycle.MaintenanceYears.GroupBy(x => x.MaintenanceType).Select(x => x.Key).ToList());
        }
    }
}
