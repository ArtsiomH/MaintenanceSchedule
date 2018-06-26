using MaintenanceSchedule.Commands;
using MaintenanceSchedule.Interfaces;
using MaintenanceScheduleDataLayer.Entities;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MaintenanceSchedule.ViewModel.ChangeObjectViewModels
{
    class TransformerTypeViewModel : BaseViewModel, IDataErrorInfo
    {
        private TransformerType transformerType;
        private TransformerType oldTransformerType;
        private string name;
        private bool check = false;
        private List<string> errors = new List<string>();

        private RelayCommand save;
        private RelayCommand cancel;

        public TransformerType TransformerType
        {
            get
            {
                return transformerType;
            }
            set
            {
                transformerType = value;
                OnProtpertyChange(nameof(TransformerType));
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnProtpertyChange(nameof(Name));
            }
        }

        public RelayCommand Save
        {
            get
            {
                return save ?? (save = new RelayCommand(o =>
                {
                    oldTransformerType.Name = Name;
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
                switch(columnName)
                {
                    case nameof(Name):
                        {
                            if (Name == null) break;
                            error = "Поле не должно быть пустым";
                            if (Name == string.Empty) break;
                            errors.Remove(error + columnName);
                            error = "Необходимо уникальное название";
                            if (serviceUnitOfWork.TransformerTypes.GetAll().FirstOrDefault(x => x.Name == Name) != null) break;
                            errors.Remove(error + columnName);
                            error = string.Empty;
                            break;
                        }
                }
                string errorRecord = error + columnName;
                if (error != string.Empty && !errors.Contains(errorRecord)) errors.Add(errorRecord);
                if (Name == null ||
                    errors.Count != 0) check = false;
                else check = true;
                return error;
            }
        }

        public TransformerTypeViewModel(IServiceUnitOfWork serviceUnitOfWork, TransformerType transformerType)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
            oldTransformerType = transformerType;
            TransformerType newTransformerType = new TransformerType();
            newTransformerType.Name = transformerType.Name;
            TransformerType = newTransformerType;
        }
    }
}
