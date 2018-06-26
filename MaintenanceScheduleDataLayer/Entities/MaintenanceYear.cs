namespace MaintenanceScheduleDataLayer.Entities
{
    public class MaintenanceYear
    {
        public int MaintenanceYearId { get; set; }

        public int Year { get; set; }

        public int MaintenanceCycleId { get; set; }
        public MaintenanceCycle MaintenanceCycle { get; set; }

        public int? MaintenanceTypeId { get; set; }
        public MaintenanceType MaintenanceType { get; set; }
    }
}