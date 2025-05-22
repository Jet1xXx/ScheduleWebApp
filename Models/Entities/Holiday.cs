using ScheduleWebApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleWebApp.Models.Entities
{
    [Table("holiday")]
    public class Holiday
    {
        [Column("holiday_id")]
        public int HolidayId { get; set; }

        [Column("holiday_date")]
        public DateTime HolidayDate { get; set; }

        [Column("holiday_name")]
        public string HolidayName { get; set; }

        public ICollection<Schedule> Schedules { get; set; }
    }
}