using System;
using MaintenanceScheduleDataLayer.Entities;
using MaintenanceScheduleDataLayer.EFContext;
using MaintenanceScheduleDataLayer.Interfaces;
using System.Diagnostics;

namespace MaintenanceScheduleDataLayer.Repositories
{
    public class EFUnitOfWork : IDisposable, IUnitOfWork
    {
        private MaintenanceScheduleContext context;

        private ActRepository actRepository;
        private AdditionalDeviceRepository additionDeviceRepository;
        private AttachmentRepository attachmentRepository;
        private AdditionalWorkRepository additionalWorkRepository;
        private DeviceRepository deviceRepository;
        private DeviceTypeRepository deviceTypeRepository;
        private DistrictElectricalNetworkRepository districElectricalNetworkRepository;
        private ElementBaseRepository elementBaseRepository;
        private InspectionsFrequencyRepository inspectionFrequencyRepository;
        private MaintainedEquipmentRepository maintainedEquipmentRepository;
        private MaintainedEquipmentByCycleRepository maintainedEquipmentByCycleRepository;
        private MaintenanceCycleRepository maintenanceCycleRepository;
        private MaintenanceRecordRepository maintenanceRecordRepository;
        private MaintenanceTypeRepository maintenanceTypeRepository;
        private MaintenanceYearRepository maintenanceYearRepository;
        private ManagementOrganizationRepository managementOrganizationRepository;
        private ManufacturerRepository manufacturerRepository;
        private RelayDeviceRepository relayDeviceRepository;
        private SubstationRepository substationRepository;
        private TeamRepository teamRepository;
        private TransformerTypeRepository TransformerTypeRepository;
        private VoltageClassRepository voltageClassRepository;
        private ScheduleRepository scheduleRepository;


        public EFUnitOfWork(string name)
        {
            context = new MaintenanceScheduleContext(name);
        }

        public IRepository<Act> Acts
        {
            get
            {
                if (actRepository == null)
                {
                    actRepository = new ActRepository(context);
                }
                return actRepository;
            }
        }

        public IRepository<AdditionalDevice> AdditionalDevices
        {
            get
            {
                if (additionDeviceRepository == null)
                {
                    additionDeviceRepository = new AdditionalDeviceRepository(context);
                }
                return additionDeviceRepository;
            }
        }

        public IRepository<Attachment> Attachments
        {
            get
            {
                if (attachmentRepository == null)
                {
                    attachmentRepository = new AttachmentRepository(context);
                }
                return attachmentRepository;
            }
        }

        public IRepository<AdditionalWork> AdditionalWorks
        {
            get
            {
                if (additionalWorkRepository == null)
                {
                    additionalWorkRepository = new AdditionalWorkRepository(context);
                }
                return additionalWorkRepository;
            }
        }

        public IRepository<Device> Devices
        {
            get
            {
                if (deviceRepository == null)
                {
                    deviceRepository = new DeviceRepository(context);
                }
                return deviceRepository;
            }
        }

        public IRepository<DeviceType> DeviceTypes
        {
            get
            {
                if (deviceTypeRepository == null)
                {
                    deviceTypeRepository = new DeviceTypeRepository(context);
                }
                return deviceTypeRepository;
            }
        }

        public IRepository<DistrictElectricalNetwork> DistricElectricalNetworks
        {
            get
            {
                if (districElectricalNetworkRepository == null)
                {
                    districElectricalNetworkRepository = new DistrictElectricalNetworkRepository(context);
                }
                return districElectricalNetworkRepository;
            }
        }

        public IRepository<ElementBase> ElementBases
        {
            get
            {
                if (elementBaseRepository == null)
                {
                    elementBaseRepository = new ElementBaseRepository(context);
                }
                return elementBaseRepository;
            }
        }

        public IRepository<InspectionsFrequency> InspectionsFrequencies
        {
            get
            {
                if (inspectionFrequencyRepository == null)
                {
                    inspectionFrequencyRepository = new InspectionsFrequencyRepository(context);
                }
                return inspectionFrequencyRepository;
            }
        }

        public IRepository<MaintainedEquipment> MaintainedEquipments
        {
            get
            {
                if (maintainedEquipmentRepository == null)
                {
                    maintainedEquipmentRepository = new MaintainedEquipmentRepository(context);
                }
                return maintainedEquipmentRepository;
            }
        }

        public IRepository<MaintainedEquipmentByCycle> MaintainedEquipmentsByCycle
        {
            get
            {
                if (maintainedEquipmentByCycleRepository == null)
                {
                    maintainedEquipmentByCycleRepository = new MaintainedEquipmentByCycleRepository(context);
                }
                return maintainedEquipmentByCycleRepository;
            }
        }

        public IRepository<MaintenanceCycle> MaintenanceCycles
        {
            get
            {
                if (maintenanceCycleRepository == null)
                {
                    maintenanceCycleRepository = new MaintenanceCycleRepository(context);
                }
                return maintenanceCycleRepository;
            }
        }

        public IRepository<MaintenanceRecord> MaintenanceRecords
        {
            get
            {
                if (maintenanceRecordRepository == null)
                {
                    maintenanceRecordRepository = new MaintenanceRecordRepository(context);
                }
                return maintenanceRecordRepository;
            }
        }

        public IRepository<MaintenanceType> MaintenanceTypes
        {
            get
            {
                if (maintenanceTypeRepository == null)
                {
                    maintenanceTypeRepository = new MaintenanceTypeRepository(context);
                }
                return maintenanceTypeRepository;
            }
        }

        public IRepository<MaintenanceYear> MaintenanceYears
        {
            get
            {
                if (maintenanceYearRepository == null)
                {
                    maintenanceYearRepository = new MaintenanceYearRepository(context);
                }
                return maintenanceYearRepository;
            }
        }

        public IRepository<ManagementOrganization> ManagementOrganizations
        {
            get
            {
                if (managementOrganizationRepository == null)
                {
                    managementOrganizationRepository = new ManagementOrganizationRepository(context);
                }
                return managementOrganizationRepository;
            }
        }

        public IRepository<Manufacturer> Manufacturers
        {
            get
            {
                if (manufacturerRepository == null)
                {
                    manufacturerRepository = new ManufacturerRepository(context);
                }
                return manufacturerRepository;
            }
        }

        public IRepository<RelayDevice> RelayDevices
        {
            get
            {
                if (relayDeviceRepository == null)
                {
                    relayDeviceRepository = new RelayDeviceRepository(context);
                }
                return relayDeviceRepository;
            }
        }

        public IRepository<Substation> Substations
        {
            get
            {
                if (substationRepository == null)
                {
                    substationRepository = new SubstationRepository(context);
                }
                return substationRepository;
            }
        }

        public IRepository<Team> Teams
        {
            get
            {
                if (teamRepository == null)
                {
                    teamRepository = new TeamRepository(context);
                }
                return teamRepository;
            }
        }

        public IRepository<TransformerType> TransformerTypes
        {
            get
            {
                if (TransformerTypeRepository == null)
                {
                    TransformerTypeRepository = new TransformerTypeRepository(context);
                }
                return TransformerTypeRepository;
            }
        }

        public IRepository<VoltageClass> VoltageClasses
        {
            get
            {
                if (voltageClassRepository == null)
                {
                    voltageClassRepository = new VoltageClassRepository(context);
                }
                return voltageClassRepository;
            }
        }

        public IRepository<Schedule> Schedules
        {
            get
            {
                if (scheduleRepository == null)
                {
                    scheduleRepository = new ScheduleRepository(context);
                }
                return scheduleRepository;
            }
        }

        public void Save()
        {
            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                var s = context.GetValidationErrors();                
                Debug.WriteLine(e.ToString());
            }
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}