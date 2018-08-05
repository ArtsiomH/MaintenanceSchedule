using System;
using System.Data.Entity;
using System.IO;
using MaintenanceScheduleDataLayer.Entities;


namespace MaintenanceScheduleDataLayer.EFContext
{
    class MaintenanceScheduleContext : DbContext
    {
        public DbSet<Act> Acts { get; set; }
        public DbSet<AdditionalDevice> AdditionalDevices { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<AdditionalWork> AdditionalWorks { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceType> DeviceTypes { get; set; }
        public DbSet<DistrictElectricalNetwork> DistrictElectricalNetworks { get; set; }
        public DbSet<ElementBase> ElementBases { get; set; }
        public DbSet<InspectionsFrequency> InspectionsFrequencies { get; set; }
        public DbSet<MaintainedEquipment> MaintainedEquipments { get; set; }
        public DbSet<MaintainedEquipmentByCycle> MaintainedEquipmentsByCycle { get; set; }
        public DbSet<MaintenanceCycle> MaintenanceCycles { get; set; }
        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
        public DbSet<MaintenanceType> MaintenanceTypes { get; set; }
        public DbSet<MaintenanceYear> MaintenanceYears { get; set; }
        public DbSet<ManagementOrganization> ManagementOrganizations { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<RelayDevice> RelayDevices { get; set; }
        public DbSet<Substation> Substations { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TransformerType> TransformerTypes { get; set; }
        public DbSet<VoltageClass> VoltageClasses { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        public MaintenanceScheduleContext(string name) : base(name)
        {
            Database.SetInitializer(new MaintenanceScheduleInitializer());
            Database.Initialize(false);
        }

        public MaintenanceScheduleContext() :base("name=ProbLoc")
        {
            string path = Directory.GetCurrentDirectory();
            path += @"\MaintenanceSchedule\bin\Debug\";
            AppDomain.CurrentDomain.SetData("DataDirectory", path);
            Database.SetInitializer(new MaintenanceScheduleInitializer());
        }
    }
}
