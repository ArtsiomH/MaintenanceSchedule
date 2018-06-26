using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations;

namespace MaintenanceScheduleDataLayer.Entities
{
    [Table("AdditionalWorks")]
    public class AdditionalWork : MaintainedEquipmentByCycle
    {
        [MaxLength(150)]
        [Required]
        public string Name { get; set; }
        
        public int? SubstationId { get; set; }
        [InverseProperty("AdditionalWorks")]
        [ForeignKey("SubstationId")]
        public Substation Substation { get; set; }

        public List<RelayDevice> Devices { get; set; }
    }
}
