using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceScheduleDataLayer.Entities
{
    public class Schedule
    {
        public int ScheduleId { get; set; }

        [Required]
        public int Year { get; set; }

        [MaxLength(50), Required]
        public string Condition { get; set; }
    }
}
