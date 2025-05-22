using ScheduleWebApp.Models.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleWebApp.Models.Entities
{
    [Table("schedule_type")]
    public class ScheduleType
    {
        [Column("schedule_type_id")]
        public int ScheduleTypeId { get; set; }

        [Column("schedule_type_name")]
        public string ScheduleTypeName { get; set; }

        [Column("description")]
        public string Description { get; set; }

        public ICollection<TimeTable> TimeTables { get; set; }
    }
}