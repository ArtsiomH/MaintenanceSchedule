using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceScheduleDataLayer.Entities
{
    public class Attachment
    {
        public int AttachmentId { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        public int? SortingOrder { get; set; }

        public int MaintainedEquipmentId { get; set; }
        public Substation Substation { get; set; }

        public int? VoltageClassId { get; set; }
        public VoltageClass VoltageClass { get; set; }

        public int? ManagementOrganizationId { get; set; }
        public ManagementOrganization ManagementOrganization { get; set; }

        public List<RelayDevice> RelayDevices { get; set; }

        public List<AdditionalDevice> AdditionalDevices { get; set; }
    }
}
