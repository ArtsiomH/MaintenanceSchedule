using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceScheduleDataLayer.Entities
{
    public class TransformerType
    {
        public int TransformerTypeId { get; set; }

        [MaxLength(50)]
        [Index(IsUnique = true), Required]
        public string Name { get; set; }        

        public List<Substation> Substations { get; set; }
    }
}
