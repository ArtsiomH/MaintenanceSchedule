using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.IO;
using MaintenanceScheduleDataLayer.Entities;
using MaintenanceScheduleDataLayer.Enum;
using MaintenanceScheduleDataLayer.EFContext;
using MaintenanceScheduleDataLayer.Interfaces;
using Excel = Microsoft.Office.Interop.Excel;

namespace MaintenanceScheduleDataLayer
{
    public class ReadExcel
    {
        public void Read(IUnitOfWork unitOfWork)
        {
            Excel.Application excel = new Excel.Application();
            string path = Directory.GetCurrentDirectory();
            path += "\\data.xlsx";
            excel.Workbooks.Open(path, Password: 1);

            Excel.Worksheet workSheet = excel.ActiveSheet;
            Excel.Range excelRange;
            int lol = 0;
            int randomNumber = 0;
            Random rand = new Random();
            List<string> liders = new List<string> { "Бушланов", "Васькович", "Солодухин", "Шилько", "Цапкин" };
            List<string> shortName = new List<string> { "П3", "П1", "П2", "Н", "В"};

            MaintainedEquipmentEnum equipmentEnum = MaintainedEquipmentEnum.None;
            List<ManagementOrganization> organizations = new List<ManagementOrganization>();
            List<Team> teams = new List<Team>();
            List<Substation> substations = new List<Substation>();
            List<Attachment> attachments = new List<Attachment>();
            List<InspectionsFrequency> inspections = new List<InspectionsFrequency>();
            List<ElementBase> elementBases = new List<ElementBase>();
            List<MaintenanceYear> maintenanceYears = new List<MaintenanceYear>();
            List<MaintenanceCycle> cycles = new List<MaintenanceCycle>();
            List<Act> acts = new List<Act>();
            
            List<MaintenanceType> maintenanceTypes = new List<MaintenanceType>();

            TransformerType typeTranformer = new TransformerType();
            typeTranformer.Name = "Неуказан";
            unitOfWork.TransformerTypes.Create(typeTranformer);
            unitOfWork.Save();

            DistrictElectricalNetwork network = new DistrictElectricalNetwork();
            network.Name = "Неуказан";
            network.Head = "Неуказан";
            unitOfWork.DistricElectricalNetworks.Create(network);
            unitOfWork.Save();

            VoltageClass voltageClass = new VoltageClass();
            voltageClass.Name = "Не указан";
            unitOfWork.VoltageClasses.Create(voltageClass);
            unitOfWork.Save();

            Manufacturer manufacturer = new Manufacturer();
            manufacturer.Name = "Не указан";
            unitOfWork.Manufacturers.Create(manufacturer);
            unitOfWork.Save();

            DeviceType deviceType = new DeviceType();
            deviceType.Name = "Не указан";
            unitOfWork.DeviceTypes.Create(deviceType);
            unitOfWork.Save();

            Act withoutAct = new Act();
            withoutAct.Name = "Не указан";
            withoutAct.CreationDate = new DateTime(2015, 2, 1);
            unitOfWork.Acts.Create(withoutAct);
            unitOfWork.Save();

            for (int number = 0; number < liders.Count; number++)
            {
                Team newTeam = new Team();
                newTeam.Leader = liders[number];
                newTeam.Name = shortName[number];
                teams.Add(newTeam);
                unitOfWork.Teams.Create(newTeam);
                unitOfWork.Save();
            }

            bool viewedAllSubstation = false;
            int startRecord = 3;
            int lastRecord = 400; //1875;

            for (int i = startRecord; i <= lastRecord; i++)
            {
                excelRange = workSheet.Rows[i];
                
                if (Convert.ToString(excelRange.Cells[1, 6].Value2) == "все устройства")
                {
                    equipmentEnum = MaintainedEquipmentEnum.Substation;
                }
                else if (Convert.ToString(excelRange.Cells[1, 8].Value2) == null && Convert.ToString(excelRange.Cells[1, 10].Value2) != null)
                {
                    equipmentEnum = MaintainedEquipmentEnum.AdditionalDevice;
                }
                else if (Convert.ToString(excelRange.Cells[1, 8].Value2) == null && Convert.ToString(excelRange.Cells[1, 10].Value2) == null)
                {
                    equipmentEnum = MaintainedEquipmentEnum.CombineDevice;
                }
                else if (Convert.ToString(excelRange.Cells[1, 8].Value2) != null && Convert.ToString(excelRange.Cells[1, 10].Value2) != null)
                {
                    equipmentEnum = MaintainedEquipmentEnum.Device;
                }

                if (equipmentEnum != MaintainedEquipmentEnum.Substation && !viewedAllSubstation)
                {
                    if (lastRecord == i)
                    {
                        viewedAllSubstation = true;
                        i = startRecord;
                    }
                    continue;
                }

                if (equipmentEnum == MaintainedEquipmentEnum.Substation && viewedAllSubstation) continue;                

                ManagementOrganization organization = new ManagementOrganization();
                organization.Name = Convert.ToString(excelRange.Cells[1, 1].Value2);
                if( !organizations.Exists(x => x.Name == organization.Name) )
                {                                    
                    organizations.Add(organization);
                    unitOfWork.ManagementOrganizations.Create(organization);
                    unitOfWork.Save();
                }
                else
                {
                    organization = organizations.Find(x => x.Name == organization.Name);
                }

                Team team = new Team();
                string liderShortName = Convert.ToString(excelRange.Cells[1, 2].Value2);
                team = teams.Find(x => x.Name == liderShortName);

                Substation substation = new Substation();
                substation.Name = Convert.ToString(excelRange.Cells[1, 3].Value2);
                substation.Name = substation.Name.TrimEnd();
                if (substations.Exists(x => x.Name == substation.Name))
                {
                    if (!viewedAllSubstation) continue;
                    substation = substations.Find(x => x.Name == substation.Name);
                }
                substation.Team = team;
                substation.DistrictElectricalNetwork = network;
                substation.TransformerType = typeTranformer;

                Attachment attachment = new Attachment();
                attachment.Name = Convert.ToString(excelRange.Cells[1, 5].Value2);
                attachment.ManagementOrganization = organization;
                attachment.VoltageClass = voltageClass;

                switch (equipmentEnum)
                {
                    case MaintainedEquipmentEnum.Substation:
                        {
                            InspectionsFrequency period = new InspectionsFrequency();
                            period.Name = Convert.ToString(excelRange.Cells[1, 10].Value2);
                                                        
                            MaintenanceType type = new MaintenanceType();
                            type.Name = "осмотр";
                            if (!maintenanceTypes.Any(x => x.Name == type.Name))
                            {
                                maintenanceTypes.Add(type);
                                unitOfWork.MaintenanceTypes.Create(type);
                                unitOfWork.Save();
                            }
                            else
                            {
                                type = maintenanceTypes.Find(x => x.Name == type.Name);
                            }

                            MaintenanceRecord record = new MaintenanceRecord();
                            List<MaintenanceRecord> maintenanceRecords = new List<MaintenanceRecord>();
                            substation.InputYear = Convert.ToInt32(workSheet.Cells[1, 18].Value2);

                            if (period.Name.StartsWith("1 раз в месяц"))
                            {
                                period.Count = 12;
                                string inspection = excelRange.Cells[1, 10].Value2;
                                if (inspection != null)
                                {
                                    for (int column = 18; column < 39; column++)
                                    {                                     
                                        for (int workingMonth = 1; workingMonth <= 12; workingMonth++)
                                        {
                                            record = new MaintenanceRecord();                                           
                                            int year = Convert.ToInt32(workSheet.Cells[1, column].Value2);

                                            record.PlannedMaintenanceDate = new DateTime(year, workingMonth, 1);
                                            record.PlannedMaintenanceType = type;

                                            if (year < DateTime.Now.Year || (year == DateTime.Now.Year && workingMonth <= DateTime.Now.Month))
                                            {                                               
                                                record.ActualMaintenanceDate = new DateTime(year, workingMonth, 1);
                                                record.ActualMaintenanceType = type;
                                            }
                                            maintenanceRecords.Add(record);
                                        }
                                    }
                                }
                            }
                            else if (period.Name.StartsWith("1 раз в 6"))
                            {
                                period.Count = 2;
                                for (int column = 18; column < 39; column++)
                                {
                                    string inspection = excelRange.Cells[1, 10].Value2;
                                    if (inspection != null)
                                    {
                                        for (int workingMonth = 1; workingMonth <= 2; workingMonth++)
                                        {
                                            record = new MaintenanceRecord();
                                            int year = Convert.ToInt32(workSheet.Cells[1, column].Value2);

                                            record.PlannedMaintenanceDate = new DateTime(year, 4 * workingMonth, 1);
                                            record.PlannedMaintenanceType = type;

                                            if (year < DateTime.Now.Year)
                                            {
                                                record.ActualMaintenanceDate = new DateTime(year, 4 * workingMonth, 1);
                                                record.ActualMaintenanceType = type;

                                            }
                                            maintenanceRecords.Add(record);
                                        }
                                    }
                                }
                            }
                            else if (period.Name.StartsWith("1 раз в 12"))
                            {
                                period.Count = 1;
                                for (int column = 18; column < 39; column++)
                                {
                                    string inspection = excelRange.Cells[1, 10].Value2;
                                    if (inspection != null)
                                    {
                                        record = new MaintenanceRecord();

                                        int year = Convert.ToInt32(workSheet.Cells[1, column].Value2);

                                        record.PlannedMaintenanceDate = new DateTime(year, 5, 1);
                                        record.PlannedMaintenanceType = type;

                                        if (year < DateTime.Now.Year)
                                        {
                                            record.ActualMaintenanceDate = new DateTime(year, 5, 1);
                                            record.ActualMaintenanceType = type;
                                        }                                        
                                        maintenanceRecords.Add(record);                                    
                                    }
                                }
                            }

                            if (!inspections.Exists(x => x.Name == period.Name))
                            {
                                inspections.Add(period);
                                unitOfWork.InspectionsFrequencies.Create(period);
                                unitOfWork.Save();
                            }
                            else
                            {
                                period = inspections.Find(x => x.Name == period.Name);
                            }

                            if (!substations.Exists(x => x.Name == substation.Name))
                            {
                                substation.InspectionsFrequency = period;
                                substations.Add(substation);
                                unitOfWork.Substations.Create(substation);
                                unitOfWork.Save();
                            }
                            else
                            {
                                substation = substations.Find(x => x.Name == substation.Name);
                            }                                                       

                            foreach (MaintenanceRecord rec in maintenanceRecords)
                            {
                                rec.MaintainedEquipment = substation;
                                unitOfWork.MaintenanceRecords.Create(rec);
                                unitOfWork.Save();
                            }                            
                            break;
                        }

                    case MaintainedEquipmentEnum.Device:
                        {                   
                            RelayDevice device = new RelayDevice();
                            device.Name = Convert.ToString(excelRange.Cells[1, 6].Value2);
                            device.MaintenanceRecords = new List<MaintenanceRecord>();
                            
                            ElementBase elementBase = new ElementBase();
                            elementBase.Name = Convert.ToString(excelRange.Cells[1, 8].Value2);
                            if(!elementBases.Exists(x => x.Name == elementBase.Name))
                            {
                                elementBases.Add(elementBase);
                                unitOfWork.ElementBases.Create(elementBase);
                                unitOfWork.Save();
                            }
                            else
                            {
                                elementBase = elementBases.Find(x => x.Name == elementBase.Name);
                            }

                            device.ElementBase = elementBase;

                            device.Manufacturer = manufacturer;

                            device.MaintenancePeriod = Convert.ToInt32(excelRange.Cells[1, 9].Value2);
                            device.ExpiryYear = Convert.ToInt32(excelRange.Cells[1, 12].Value2);
                            device.InputYear = Convert.ToInt32(excelRange.Cells[1, 13].Value2);

                            string lastRecovery = Convert.ToString(excelRange.Cells[1, 14].Value2);
                            if (lastRecovery != null)
                            {
                                string[] array = lastRecovery.Split('-', '/');
                                if (array.Count() < 3)
                                {
                                    array = new string[] { array[0], array[1], "16"};
                                }
                                
                                try
                                {
                                    device.LastRecovery = new DateTime(2000 + Convert.ToInt32(array[2]), Convert.ToInt32(array[1]), 1);
                                }
                                catch (ArgumentOutOfRangeException)
                                {
                                    device.LastRecovery = new DateTime(2000 + Convert.ToInt32(array[1]), Convert.ToInt32(array[2]), 1);
                                }
                                catch (FormatException)
                                {
                                    try
                                    {
                                        device.LastRecovery = new DateTime(2000 + Convert.ToInt32(array[2]), Convert.ToInt32(array[3]), 1);
                                    }
                                    catch (Exception)
                                    {
                                        device.LastRecovery = new DateTime(2000 + Convert.ToInt32(array[3]), Convert.ToInt32(array[2]), 1);
                                    }
                                }
                            }
                            

                            MaintenanceCycle cycle = new MaintenanceCycle();
                            randomNumber++;
                            cycle.Name = randomNumber.ToString();
                            cycle.MaintenanceYears = new List<MaintenanceYear>();                           
                            cycle.ShowName = Convert.ToString(excelRange.Cells[1, 10].Value2);
                            

                            device.NormalMaintenanceCycle = cycle;
                            device.ReducedMaintenanceCycle = cycle;
                                                       

                            bool recoveryFound = false;
                            int year = 8;

                            List<MaintenanceRecord> maintenanceRecords = new List<MaintenanceRecord>();
                            MaintenanceRecord record = new MaintenanceRecord();
                            
                            for (int column = 38; column >= 18; column--)
                            {
                                MaintenanceType maintenanceType = new MaintenanceType();
                                maintenanceType.Name = Convert.ToString(excelRange.Cells[1, column].Value2);
                                if (maintenanceType.Name == " ") maintenanceType.Name = null;
                                if (maintenanceType.Name != null) maintenanceType.Name = maintenanceType.Name.Trim();
                                if (maintenanceType.Name!= null && !maintenanceTypes.Exists(x => x.Name.Equals(maintenanceType.Name, StringComparison.InvariantCultureIgnoreCase)))
                                {
                                    maintenanceTypes.Add(maintenanceType);
                                    unitOfWork.MaintenanceTypes.Create(maintenanceType);
                                    unitOfWork.Save();
                                }
                                else if (maintenanceTypes.Exists(x => x.Name.Equals(maintenanceType.Name, StringComparison.InvariantCultureIgnoreCase)))
                                {
                                    maintenanceType = maintenanceTypes.First(x => x.Name.Equals(maintenanceType.Name, StringComparison.InvariantCultureIgnoreCase));
                                }

                                if (maintenanceType.Name == "В" && year == 8) recoveryFound = true;

                                if (maintenanceType.Name != null)
                                {
                                    int workYear = Convert.ToInt32(workSheet.Cells[1, column].Value2);
                                    record = new MaintenanceRecord();
                                    record.PlannedMaintenanceDate = new DateTime(workYear, 5, 1);
                                    record.PlannedMaintenanceType = maintenanceType;

                                    if (workYear < DateTime.Now.Year)
                                    {
                                        record.ActualMaintenanceDate = new DateTime(workYear, 5, 1);
                                        record.ActualMaintenanceType = maintenanceType;
                                        record.IsPlanned = true;
                                    }                                    
                                    maintenanceRecords.Add(record);
                                }

                                if (recoveryFound && year >= 0 && maintenanceType.Name != null)
                                {
                                    MaintenanceYear maintenanceYear = new MaintenanceYear();

                                    maintenanceYear.MaintenanceType = maintenanceType;
                                    maintenanceYear.Year = year;
                                    maintenanceYear.MaintenanceCycle = cycle;
                                    cycle.MaintenanceYears.Add(maintenanceYear);
                                    if (year == 0) recoveryFound = false;                                    
                                }
                                
                                if (recoveryFound) year--;
                            }
                                                        
                            List<MaintenanceCycle> searchCycle = cycles.FindAll(x => x.MaintenanceYears.Count == cycle.MaintenanceYears.Count);
                            bool exist = false;

                            foreach (MaintenanceCycle maintananceCycle in searchCycle)
                            {
                                for (int j = 0; j < maintananceCycle.MaintenanceYears.Count; j++)
                                {
                                    if (maintananceCycle.MaintenanceYears[j].MaintenanceType == cycle.MaintenanceYears[j].MaintenanceType &&
                                        maintananceCycle.MaintenanceYears[j].Year == cycle.MaintenanceYears[j].Year)
                                    {
                                        exist = true;
                                    }
                                    else
                                    {
                                        exist = false;
                                        break;
                                    }
                                }
                                if (exist)
                                {                                                                  
                                    device.NormalMaintenanceCycle = maintananceCycle;
                                    device.ReducedMaintenanceCycle = maintananceCycle;
                                    break;
                                }                                
                            }

                            if (!exist)
                            {
                                cycles.Add(cycle);
                                List<MaintenanceYear> years = new List<MaintenanceYear>();
                                years.AddRange(cycle.MaintenanceYears);
                                cycle.MaintenanceYears.Clear();
                                unitOfWork.MaintenanceCycles.Create(cycle);
                                unitOfWork.Save();
                                foreach (MaintenanceYear obj in years)
                                {                                    
                                    unitOfWork.MaintenanceYears.Create(obj);
                                    unitOfWork.Save();                                                                         
                                }                                
                            }

                            attachment.Name = attachment.Name.Trim();
                            if (!attachments.Exists(x => x.Name == attachment.Name))
                            {
                                attachment.Substation = substations.FirstOrDefault(x => x.Name == substation.Name);
                                attachments.Add(attachment);
                                unitOfWork.Attachments.Create(attachment);
                                unitOfWork.Save();
                                lol++;
                            }
                            else
                            {
                                attachment = attachments.First(x => x.Name == attachment.Name);
                            }

                            device.Act = withoutAct;

                            string act = Convert.ToString(excelRange.Cells[1, 11].Value2);

                            if (act != null && !act.StartsWith("вкл"))
                            {
                                Act actOfService = new Act();
                                actOfService.Name = act;
                                if (!acts.Exists(x => x.Name == actOfService.Name))
                                {
                                    acts.Add(actOfService);
                                    string[] strings = actOfService.Name.Split(' ');
                                    DateTime date;
                                    DateTime.TryParse(strings[2], out date);
                                    actOfService.CreationDate = date;
                                    unitOfWork.Acts.Create(actOfService);
                                    unitOfWork.Save();
                                }
                                else
                                {
                                    actOfService = acts.Find(x => x.Name == actOfService.Name);
                                }
                                device.Act = actOfService;
                            }

                            device.DeviceType = deviceType;
                            

                            
                            device.Attachment = attachment;
                            unitOfWork.Devices.Create(device);
                            unitOfWork.Save();
                            maintenanceRecords.Reverse();
                            foreach (MaintenanceRecord rec in maintenanceRecords)
                            {
                                rec.MaintainedEquipment = device;
                                unitOfWork.MaintenanceRecords.Create(rec);
                                unitOfWork.Save();
                            }

                            break;
                        }

                    case MaintainedEquipmentEnum.CombineDevice:
                        {                                                
                            AdditionalWork additionalWork = new AdditionalWork();
                            additionalWork.Name = Convert.ToString(excelRange.Cells[1, 6].Value2);
                            additionalWork.MaintenanceRecords = new List<MaintenanceRecord>();                            
                                                     
                            
                            if (additionalWork.Name.StartsWith("прохождение"))
                            {
                                int k = 1;
                            }
                            MaintenanceCycle cycle = new MaintenanceCycle();
                            cycle.MaintenanceYears = new List<MaintenanceYear>();
                            randomNumber++;
                            cycle.Name = randomNumber.ToString();
                            cycle.ShowName = Convert.ToString(excelRange.Cells[1, 10].Value2);
                            if (cycle.ShowName == null)
                            {
                                cycle.ShowName = "Не назначено имя";
                            }
                            additionalWork.NormalMaintenanceCycle = cycle;
                            additionalWork.ReducedMaintenanceCycle = cycle;
                            bool recoveryFound = false;
                            int year = 0;
                            List<MaintenanceRecord> maintenanceRecords = new List<MaintenanceRecord>();
                            MaintenanceRecord record = new MaintenanceRecord();

                            for (int column = 18; column <= 38; column++)
                            {                                
                                MaintenanceType maintenanceType = new MaintenanceType();
                                maintenanceType.Name = Convert.ToString(excelRange.Cells[1, column].Value2);
                                if (maintenanceType.Name == " ") maintenanceType.Name = null;
                                if (maintenanceType.Name != null && !maintenanceTypes.Exists(x => x.Name.Equals(maintenanceType.Name, StringComparison.InvariantCultureIgnoreCase)))
                                {
                                    maintenanceTypes.Add(maintenanceType);
                                    unitOfWork.MaintenanceTypes.Create(maintenanceType);
                                    unitOfWork.Save();
                                }
                                else if (maintenanceTypes.Exists(x => x.Name.Equals(maintenanceType.Name, StringComparison.InvariantCultureIgnoreCase)))
                                {
                                    maintenanceType = maintenanceTypes.First(x => x.Name.Equals(maintenanceType.Name, StringComparison.InvariantCultureIgnoreCase));
                                }

                                if (maintenanceType.Name != null && year == 0) recoveryFound = true;

                                if (maintenanceType.Name != null)
                                {
                                    int workYear = Convert.ToInt32(workSheet.Cells[1, column].Value2);
                                    record = new MaintenanceRecord();
                                    record.PlannedMaintenanceDate = new DateTime(workYear, 5, 1);
                                    record.PlannedMaintenanceType = maintenanceType;

                                    if (workYear < DateTime.Now.Year)
                                    {
                                        record.ActualMaintenanceDate = new DateTime(workYear, 5, 1);
                                        record.ActualMaintenanceType = maintenanceType;
                                        record.IsPlanned = true;
                                    }
                                    maintenanceRecords.Add(record);
                                    if (maintenanceRecords.Count == 1) additionalWork.InputYear = Convert.ToInt32(workSheet.Cells[1, column].Value2);
                                }

                                if (recoveryFound && year <= 8 && maintenanceType.Name != null)
                                {
                                    MaintenanceYear maintenanceYear = new MaintenanceYear();
                                    maintenanceYear.MaintenanceType = maintenanceType;
                                    maintenanceYear.Year = year;
                                    maintenanceYear.MaintenanceCycle = cycle;
                                    cycle.MaintenanceYears.Add(maintenanceYear);
                                    if (year == 8) recoveryFound = false;
                                }
                                
                                if (recoveryFound) year++;                                                                
                            }

                            List<MaintenanceCycle> searchCycle = cycles.FindAll(x => x.MaintenanceYears.Count == cycle.MaintenanceYears.Count);
                            bool exist = false;

                            foreach (MaintenanceCycle maintananceCycle in searchCycle)
                            {
                                for (int j = 0; j < maintananceCycle.MaintenanceYears.Count; j++)
                                {
                                    if (maintananceCycle.MaintenanceYears[j].MaintenanceType == cycle.MaintenanceYears[j].MaintenanceType &&
                                        maintananceCycle.MaintenanceYears[j].Year == cycle.MaintenanceYears[j].Year)
                                    {
                                        exist = true;
                                    }
                                    else
                                    {
                                        exist = false;
                                        break;
                                    }
                                }
                                if (exist)
                                {
                                    additionalWork.NormalMaintenanceCycle = maintananceCycle;
                                    additionalWork.ReducedMaintenanceCycle = maintananceCycle;
                                    break;
                                }
                            }

                            if (!exist)
                            {
                                cycles.Add(cycle);
                                List<MaintenanceYear> years = new List<MaintenanceYear>();
                                cycle.MaintenanceYears.Reverse();
                                years.AddRange(cycle.MaintenanceYears);
                                cycle.MaintenanceYears.Clear();
                                unitOfWork.MaintenanceCycles.Create(cycle);
                                unitOfWork.Save();
                                foreach (MaintenanceYear obj in years)
                                {
                                    unitOfWork.MaintenanceYears.Create(obj);
                                    unitOfWork.Save();
                                }
                            }

                            additionalWork.Substation = substations.FirstOrDefault(x => x.Name == substation.Name);
                            unitOfWork.AdditionalWorks.Create(additionalWork);
                            unitOfWork.Save();

                            foreach (MaintenanceRecord rec in maintenanceRecords)
                            {
                                rec.MaintainedEquipment = additionalWork;
                                unitOfWork.MaintenanceRecords.Create(rec);
                                unitOfWork.Save();
                            }
                            break;
                        }

                    case MaintainedEquipmentEnum.AdditionalDevice:
                        {                                                    
                            AdditionalDevice additionalDevice = new AdditionalDevice();
                            additionalDevice.Name = Convert.ToString(excelRange.Cells[1, 6].Value2);
                            additionalDevice.MaintenanceRecords = new List<MaintenanceRecord>();
                            additionalDevice.MaintenancePeriod = 25;

                            string lastRecovery = Convert.ToString(excelRange.Cells[1, 14].Value2);
                            if (lastRecovery != null)
                            {
                                string[] array = lastRecovery.Split('-', '/');
                                if (array.Count() < 3)
                                {
                                    array = new string[] { array[0], array[1], "16" };
                                }
                                try
                                {
                                    additionalDevice.LastRecovery = new DateTime(2000 + Convert.ToInt32(array[2]), Convert.ToInt32(array[1]), 1);
                                }
                                catch (ArgumentOutOfRangeException)
                                {
                                    additionalDevice.LastRecovery = new DateTime(2000 + Convert.ToInt32(array[1]), Convert.ToInt32(array[2]), 1);
                                }
                                catch (FormatException)
                                {
                                    try
                                    {
                                        additionalDevice.LastRecovery = new DateTime(2000 + Convert.ToInt32(array[2]), Convert.ToInt32(array[3]), 1);
                                    }
                                    catch (Exception)
                                    {
                                        additionalDevice.LastRecovery = new DateTime(2000 + Convert.ToInt32(array[3]), Convert.ToInt32(array[2]), 1);
                                    }
                                }
                            }
                            MaintenanceCycle cycle = new MaintenanceCycle();
                            cycle.MaintenanceYears = new List<MaintenanceYear>();
                            randomNumber++;
                            cycle.Name = randomNumber.ToString();
                            cycle.ShowName = Convert.ToString(excelRange.Cells[1, 10].Value2);
                            additionalDevice.NormalMaintenanceCycle = cycle;
                            additionalDevice.ReducedMaintenanceCycle = cycle;
                            bool recoveryFound = false;
                            int year = 0;
                            List<MaintenanceRecord> maintenanceRecords = new List<MaintenanceRecord>();
                            MaintenanceRecord record = new MaintenanceRecord();

                            for (int column = 18; column <= 38; column++)
                            {
                                MaintenanceType maintenanceType = new MaintenanceType();
                                maintenanceType.Name = Convert.ToString(excelRange.Cells[1, column].Value2);
                                if (maintenanceType.Name == " ") maintenanceType.Name = null;
                                if (maintenanceType.Name != null && !maintenanceTypes.Exists(x => x.Name.Equals(maintenanceType.Name, StringComparison.InvariantCultureIgnoreCase)))
                                {
                                    maintenanceTypes.Add(maintenanceType);
                                    unitOfWork.MaintenanceTypes.Create(maintenanceType);
                                    unitOfWork.Save();
                                }
                                else if (maintenanceTypes.Exists(x => x.Name.Equals(maintenanceType.Name, StringComparison.InvariantCultureIgnoreCase)))
                                {
                                    maintenanceType = maintenanceTypes.First(x => x.Name.Equals(maintenanceType.Name, StringComparison.InvariantCultureIgnoreCase));
                                }

                                if (maintenanceType.Name != "В" && year == 0) recoveryFound = true;

                                if (maintenanceType.Name != null)
                                {
                                    int workYear = Convert.ToInt32(workSheet.Cells[1, column].Value2);
                                    record = new MaintenanceRecord();
                                    record.PlannedMaintenanceDate = new DateTime(workYear, 5, 1);
                                    record.PlannedMaintenanceType = maintenanceType;

                                    if (workYear < DateTime.Now.Year)
                                    {
                                        record.ActualMaintenanceDate = new DateTime(workYear, 5, 1);
                                        record.ActualMaintenanceType = maintenanceType;
                                        record.IsPlanned = true;
                                    }
                                    maintenanceRecords.Add(record);                                    
                                }

                                if (recoveryFound && year >= 0 && maintenanceType.Name != null)
                                {
                                    MaintenanceYear maintenanceYear = new MaintenanceYear();

                                    maintenanceYear.MaintenanceType = maintenanceType;
                                    maintenanceYear.Year = year;
                                    maintenanceYear.MaintenanceCycle = cycle;
                                    cycle.MaintenanceYears.Add(maintenanceYear);
                                    if (year == 8) recoveryFound = false;
                                }

                                if (recoveryFound) year++;                                
                            }

                            additionalDevice.Act = withoutAct;

                            string act = Convert.ToString(excelRange.Cells[1, 11].Value2);

                            if (act != null)
                            {
                                Act actOfService = new Act();
                                actOfService.Name = act;                                
                                if (!acts.Any(x => x.Name == actOfService.Name))
                                {
                                    string[] strings = actOfService.Name.Split(' ');
                                    DateTime date;
                                    DateTime.TryParse(strings[2], out date);
                                    actOfService.CreationDate = date;
                                    acts.Add(actOfService);
                                    unitOfWork.Acts.Create(actOfService);
                                    unitOfWork.Save();
                                }
                                else
                                {
                                    actOfService = acts.Find(x => x.Name == actOfService.Name);
                                }
                                additionalDevice.Act = actOfService;
                            }

                            List<MaintenanceCycle> searchCycle = cycles.FindAll(x => x.MaintenanceYears.Count == cycle.MaintenanceYears.Count);
                            bool exist = false;

                            foreach (MaintenanceCycle maintananceCycle in searchCycle)
                            {
                                for (int j = 0; j < maintananceCycle.MaintenanceYears.Count; j++)
                                {
                                    if (maintananceCycle.MaintenanceYears[j].MaintenanceType == cycle.MaintenanceYears[j].MaintenanceType &&
                                        maintananceCycle.MaintenanceYears[j].Year == cycle.MaintenanceYears[j].Year)
                                    {
                                        exist = true;
                                    }
                                    else
                                    {
                                        exist = false;
                                        break;
                                    }
                                }
                                if (exist)
                                {
                                    additionalDevice.NormalMaintenanceCycle = maintananceCycle;
                                    additionalDevice.ReducedMaintenanceCycle = maintananceCycle;
                                    break;
                                }
                            }

                            if (!exist)
                            {
                                cycles.Add(cycle);
                                List<MaintenanceYear> years = new List<MaintenanceYear>();
                                cycle.MaintenanceYears.Reverse();
                                years.AddRange(cycle.MaintenanceYears);
                                cycle.MaintenanceYears.Clear();
                                unitOfWork.MaintenanceCycles.Create(cycle);
                                unitOfWork.Save();
                                foreach (MaintenanceYear obj in years)
                                {
                                    unitOfWork.MaintenanceYears.Create(obj);
                                    unitOfWork.Save();
                                }
                            }

                            if (!attachments.Exists(x => x.Name == attachment.Name))
                            {
                                attachment.Substation = substations.FirstOrDefault(x => x.Name == substation.Name);
                                attachments.Add(attachment);
                                unitOfWork.Attachments.Create(attachment);
                                unitOfWork.Save();
                            }
                            else
                            {
                                attachment = attachments.First(x => x.Name == attachment.Name);
                            }
                            additionalDevice.Attachment = attachment;
                            unitOfWork.AdditionalDevices.Create(additionalDevice);
                            unitOfWork.Save();
                            foreach (MaintenanceRecord rec in maintenanceRecords)
                            {
                                rec.MaintainedEquipment = additionalDevice;
                                unitOfWork.MaintenanceRecords.Create(rec);
                                unitOfWork.Save();
                            }
                            break;
                        }

                    default:
                        {
                            break;
                        }

                }
                //unitOfWork.Save();
            }
            excel.Workbooks.Close();
        }

        private MaintenanceCycle AddEightNormalCycleHightVoltage()
        {
            MaintenanceCycle maintenanceCycle = new MaintenanceCycle();

            maintenanceCycle.Name = "8 лет ПТО 110кВ";

            maintenanceCycle.MaintenanceYears = new List<MaintenanceYear>();

            maintenanceCycle.MaintenanceYears.Add(AddMaintenanceYear(0, "Н"));

            maintenanceCycle.MaintenanceYears.Add(AddMaintenanceYear(1, "К-1"));

            maintenanceCycle.MaintenanceYears.Add(AddMaintenanceYear(4, "К"));

            maintenanceCycle.MaintenanceYears.Add(AddMaintenanceYear(8, "В"));

            return maintenanceCycle;
        }

        private MaintenanceYear AddMaintenanceYear(int year, string type)
        {
            MaintenanceYear maintenanceYear = new MaintenanceYear();            
            maintenanceYear.Year = year;
            MaintenanceType maintenanceType = new MaintenanceType();
            maintenanceType.Name = type;
            maintenanceYear.MaintenanceType = maintenanceType;
            return maintenanceYear;
        }
    }
}