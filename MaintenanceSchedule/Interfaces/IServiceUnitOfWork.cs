using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaintenanceScheduleDataLayer.Entities;

namespace MaintenanceSchedule.Interfaces
{
    interface IServiceUnitOfWork
    {
        IActService Acts { get; }
        IAdditionalDeviceService AdditionalDevices { get; }
        IAdditionalDeviceModelService AdditionalDeviceModels { get; }
        IAttachmentService Attachments { get; }
        IAdditionalWorkModelService AdditionalWorkModels { get; }
        IAdditionalWorkService AdditionalWorks { get; }
        IBaseService<DeviceType> DeviceTypes { get; }
        IBaseService<DistrictElectricalNetwork> DistrictElectricalNetworks { get; }
        IBaseService<ElementBase> ElementBases { get; }
        IBaseService<InspectionsFrequency> InspectionsFrequencies { get; }
        IMaintainedEquipmentService MaintainedEquipments { get; }
        IScheduleRecordModelService ScheduleRecordModels { get; }
        IMaintainedEquipmentByCycleService MaintainedEquipmentsByCycleService { get; }
        IMaintenanceCycleModelService MaintenanceCycleModels { get; }
        IMaintenanceCycleService MaintenanceCycles { get; }
        IMaintenanceRecordService MaintenanceRecords { get; }
        IBaseService<MaintenanceType> MaintenanceTypes { get; }
        IBaseService<MaintenanceYear> MaintenanceYears { get; }
        IBaseService<ManagementOrganization> ManagementOrganizations { get; }
        IBaseService<Manufacturer> Manufacturers { get; }
        IRelayDeviceService RelayDevices { get; }
        IRelayDeviceModelService RelayDeviceModels { get; }
        ISubstationService Substations { get; }
        IBaseService<Team> Teams { get; }
        IBaseService<TransformerType> TransformerTypes { get; }
        IBaseService<VoltageClass> VoltageClasses { get; }
        IScheduleService Schedules { get; }
    }
}
