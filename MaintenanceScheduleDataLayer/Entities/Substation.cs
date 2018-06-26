using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceScheduleDataLayer.Entities
{
    [Table("Substations")]
    public class Substation : MaintainedEquipment
    {
        [MaxLength(50)]
        [Index(IsUnique = true), Required]
        public string Name { get; set; }
        public int? SortingOrder { get; set; }
        public int TransformerCount { get; set; }

        public int? TeamId { get; set; }
        public Team Team { get; set; }

        public int? TransformerTypeId { get; set; }
        public TransformerType TransformerType { get; set; }

        public int? DistrictElectricalNetworkId { get; set; }
        public DistrictElectricalNetwork DistrictElectricalNetwork { get; set; }

        public int InspectionsFrequencyId { get; set; }       
        public InspectionsFrequency InspectionsFrequency { get; set; }

        public List<AdditionalWork> AdditionalWorks { get; set; }

        public List<Attachment> Attachments { get; set; }
    }
}
