using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceScheduleDataLayer.Entities
{
    public class Act
    {
        public int ActId { get; set; }
       
        [MaxLength(50)]
        [Index(IsUnique = true), Required]
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }

        public List<RelayDevice> Devices { get; set; }

        public List<AdditionalDevice> AdditionalDevices { get; set; }
    }
}
