using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceScheduleDataLayer.Entities
{
    public class DeviceType
    {
        public int DeviceTypeId { get; set; }

        [MaxLength(50)]
        [Index(IsUnique = true), Required]
        public string Name { get; set; }

        public List<RelayDevice> Devices { get; set; }
    }
}
