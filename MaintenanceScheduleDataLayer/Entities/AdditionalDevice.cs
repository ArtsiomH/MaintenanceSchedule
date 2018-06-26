using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceScheduleDataLayer.Entities
{
    [Table("AdditionalDevices")]
    public class AdditionalDevice : Device
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int? AttachmentId { get; set; }
        public Attachment Attachment { get; set; }
    }
}
