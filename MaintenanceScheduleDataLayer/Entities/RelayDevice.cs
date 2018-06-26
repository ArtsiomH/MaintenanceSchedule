using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceScheduleDataLayer.Entities
{
    [Table("RelayDevices")]
    public class RelayDevice : Device
    {
        //[Required]
        //[MaxLength(50)]
        public string Name { get; set; }

        public int? ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }

        public int? ElementBaseId { get; set; }
        public ElementBase ElementBase { get; set; }

        public int? DeviceTypeId { get; set; }
        public DeviceType DeviceType { get; set; }

        public int? AttachmentId { get; set; }
        public Attachment Attachment { get; set; }

        public List<AdditionalWork> CombineDevices { get; set; }
    }
}
