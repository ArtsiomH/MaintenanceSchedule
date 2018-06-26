using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceScheduleDataLayer.Entities
{
    public class InspectionsFrequency
    {
        public int InspectionsFrequencyId { get; set; }

        [MaxLength(50)]
        [Index(IsUnique = true), Required]
        public string Name { get; set; }
        public int Count { get; set; }
                
        public List<Substation> Substations { get; set; }
    }
}
