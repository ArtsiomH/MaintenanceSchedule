using System;
using MaintenanceScheduleDataLayer.Entities;

namespace MaintenanceScheduleDataLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Act> Acts { get; }
        IRepository<AdditionalDevice> AdditionalDevices { get; }
        IRepository<Attachment> Attachments { get;}
        IRepository<AdditionalWork> AdditionalWorks { get; }
        IRepository<Device> Devices { get; }
        IRepository<DeviceType> DeviceTypes { get; }
        IRepository<DistrictElectricalNetwork> DistricElectricalNetworks { get; }
        IRepository<ElementBase> ElementBases { get; }
        IRepository<InspectionsFrequency> InspectionsFrequencies { get; }
        IRepository<MaintainedEquipmentByCycle> MaintainedEquipmentsByCycle { get; }
        IRepository<MaintainedEquipment> MaintainedEquipments { get; }
        IRepository<MaintenanceCycle> MaintenanceCycles { get; }
        IRepository<MaintenanceRecord> MaintenanceRecords { get; }
        IRepository<MaintenanceType> MaintenanceTypes { get; }
        IRepository<MaintenanceYear> MaintenanceYears { get; }
        IRepository<ManagementOrganization> ManagementOrganizations { get; }
        IRepository<Manufacturer> Manufacturers { get; }
        IRepository<RelayDevice> RelayDevices { get; }
        IRepository<Substation> Substations { get; }
        IRepository<Team> Teams { get; }
        IRepository<TransformerType> TransformerTypes { get; }
        IRepository<VoltageClass> VoltageClasses { get; }
        IRepository<Schedule> Schedules { get; }
        void Save();
    }
}
