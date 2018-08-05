using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaintenanceSchedule.Interfaces;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Repositories;
using MaintenanceScheduleDataLayer.Entities;

namespace MaintenanceSchedule.Services
{
    class ServiceUnitOfWork : IServiceUnitOfWork
    {        
        private IUnitOfWork unitOfWork;

        private ActService actService;
        private AdditionalDeviceModelService additionalDeviceModelService;
        private AdditionalDeviceService additionalDeviceService;
        private AttachmentService attachmentService;
        private AdditionalWorkModelService additionalWorkModelService;
        private AdditionalWorkService additionalWorkService;
        private DeviceTypeService deviceTypeService;
        private DistrictElectricalNetworkService districtElectricalNetworkService;
        private ElementBaseService elementBaseService;
        private InspectionsFrequencyService inspectionFrequencyService;
        private MaintainedEquipmentService maintainedEquipmentService;
        private ScheduleRecordModelService scheduleRecordModelService;
        private MaintainedEquipmentByCycleService maintainedEquipmentByCycleService;
        private MaintenanceCycleModelService maintenanceCycleModelService;
        private MaintenanceCycleService maintenanceCycleService;
        private MaintenanceRecordService maintenanceRecordService;
        private MaintenanceTypeService maintenanceTypeService;
        private MaintenanceYearService maintenanceYearService;
        private ManagementOrganizationService managementOrganizationService;
        private ManufacturerService manufacturerService;
        private RelayDeviceService relayDeviceService;
        private RelayDeviceModelService relayDeviceModelService;
        private SubstationService substationService;
        private TeamService teamService;
        private TransformerTypeService TransformerTypeService;
        private VoltageClassService voltageClassService;
        private ScheduleService scheduleService;


        public ServiceUnitOfWork(string name)
        {
            unitOfWork = new EFUnitOfWork(name);
        }

        public IActService Acts
        {
            get
            {
                if (actService == null)
                {
                    actService = new ActService(unitOfWork);
                }
                return actService;
            }
        }

        public IAdditionalDeviceModelService AdditionalDeviceModels
        {
            get
            {
                if (additionalDeviceModelService == null)
                {
                    additionalDeviceModelService = new AdditionalDeviceModelService(unitOfWork);
                }
                return additionalDeviceModelService;
            }
        }

        public IAdditionalDeviceService AdditionalDevices
        {
            get
            {
                if (additionalDeviceService == null)
                {
                    additionalDeviceService = new AdditionalDeviceService(unitOfWork);
                }
                return additionalDeviceService;
            }
        }

        public IAttachmentService Attachments
        {
            get
            {
                if (attachmentService == null)
                {
                    attachmentService = new AttachmentService(unitOfWork);
                }
                return attachmentService;
            }
        }

        public IAdditionalWorkModelService AdditionalWorkModels
        {
            get
            {
                if (additionalWorkModelService == null)
                {
                    additionalWorkModelService = new AdditionalWorkModelService(unitOfWork);
                }
                return additionalWorkModelService;
            }
        }

        public IAdditionalWorkService AdditionalWorks
        {
            get
            {
                if (additionalWorkService == null)
                {
                    additionalWorkService = new AdditionalWorkService(unitOfWork);
                }
                return additionalWorkService;
            }
        }

        public IBaseService<DeviceType> DeviceTypes
        {
            get
            {
                if (deviceTypeService == null)
                {
                    deviceTypeService = new DeviceTypeService(unitOfWork);
                }
                return deviceTypeService;
            }
        }

        public IBaseService<DistrictElectricalNetwork> DistrictElectricalNetworks
        {
            get
            {
                if (districtElectricalNetworkService == null)
                {
                    districtElectricalNetworkService = new DistrictElectricalNetworkService(unitOfWork);
                }
                return districtElectricalNetworkService;
            }
        }

        public IBaseService<ElementBase> ElementBases
        {
            get
            {
                if (elementBaseService == null)
                {
                    elementBaseService = new ElementBaseService(unitOfWork);
                }
                return elementBaseService;
            }
        }

        public IBaseService<InspectionsFrequency> InspectionsFrequencies
        {
            get
            {
                if (inspectionFrequencyService == null)
                {
                    inspectionFrequencyService = new InspectionsFrequencyService(unitOfWork);
                }
                return inspectionFrequencyService;
            }
        }

        public IMaintainedEquipmentService MaintainedEquipments
        {
            get
            {
                if (maintainedEquipmentService == null)
                {
                    maintainedEquipmentService = new MaintainedEquipmentService(unitOfWork);
                }
                return maintainedEquipmentService;
            }
        }

        public IScheduleRecordModelService ScheduleRecordModels
        {
            get
            {
                if (scheduleRecordModelService == null)
                {
                    scheduleRecordModelService = new ScheduleRecordModelService(unitOfWork);
                }
                return scheduleRecordModelService;
            }
        }

        public IMaintainedEquipmentByCycleService MaintainedEquipmentsByCycleService
        {
            get
            {
                if (maintainedEquipmentByCycleService == null)
                {
                    maintainedEquipmentByCycleService = new MaintainedEquipmentByCycleService(unitOfWork);
                }
                return maintainedEquipmentByCycleService;
            }
        }

        public IMaintenanceCycleModelService MaintenanceCycleModels
        {
            get
            {
                if (maintenanceCycleModelService == null)
                {
                    maintenanceCycleModelService = new MaintenanceCycleModelService(unitOfWork);
                }
                return maintenanceCycleModelService;
            }
        }

        public IMaintenanceCycleService MaintenanceCycles
        {
            get
            {
                if (maintenanceCycleService == null)
                {
                    maintenanceCycleService = new MaintenanceCycleService(unitOfWork);
                }
                return maintenanceCycleService;
            }
        }

        public IMaintenanceRecordService MaintenanceRecords
        {
            get
            {
                if (maintenanceRecordService == null)
                {
                    maintenanceRecordService = new MaintenanceRecordService(unitOfWork);
                }
                return maintenanceRecordService;
            }
        }

        public IBaseService<MaintenanceType> MaintenanceTypes
        {
            get
            {
                if (maintenanceTypeService == null)
                {
                    maintenanceTypeService = new MaintenanceTypeService(unitOfWork);
                }
                return maintenanceTypeService;
            }
        }

        public IBaseService<MaintenanceYear> MaintenanceYears
        {
            get
            {
                if (maintenanceYearService == null)
                {
                    maintenanceYearService = new MaintenanceYearService(unitOfWork);
                }
                return maintenanceYearService;
            }
        }

        public IBaseService<ManagementOrganization> ManagementOrganizations
        {
            get
            {
                if (managementOrganizationService == null)
                {
                    managementOrganizationService = new ManagementOrganizationService(unitOfWork);
                }
                return managementOrganizationService;
            }
        }

        public IBaseService<Manufacturer> Manufacturers
        {
            get
            {
                if (manufacturerService == null)
                {
                    manufacturerService = new ManufacturerService(unitOfWork);
                }
                return manufacturerService;
            }
        }

        public IRelayDeviceService RelayDevices
        {
            get
            {
                if (relayDeviceService == null)
                {
                    relayDeviceService = new RelayDeviceService(unitOfWork);
                }
                return relayDeviceService;
            }
        }

        public IRelayDeviceModelService RelayDeviceModels
        {
            get
            {
                if (relayDeviceModelService == null)
                {
                    relayDeviceModelService = new RelayDeviceModelService(unitOfWork);
                }
                return relayDeviceModelService;
            }
        }

        public ISubstationService Substations
        {
            get
            {
                if (substationService == null)
                {
                    substationService = new SubstationService(unitOfWork);
                }
                return substationService;
            }
        }

        public IBaseService<Team> Teams
        {
            get
            {
                if (teamService == null)
                {
                    teamService = new TeamService(unitOfWork);
                }
                return teamService;
            }
        }

        public IBaseService<TransformerType> TransformerTypes
        {
            get
            {
                if (TransformerTypeService == null)
                {
                    TransformerTypeService = new TransformerTypeService(unitOfWork);
                }
                return TransformerTypeService;
            }
        }

        public IBaseService<VoltageClass> VoltageClasses
        {
            get
            {
                if (voltageClassService == null)
                {
                    voltageClassService = new VoltageClassService(unitOfWork);
                }
                return voltageClassService;
            }
        }

        public IScheduleService Schedules
        {
            get
            {
                if (scheduleService == null)
                {
                    scheduleService = new ScheduleService(unitOfWork);
                }
                return scheduleService;
            }
        }
    }
}