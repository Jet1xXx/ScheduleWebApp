using ScheduleWebApp.Models.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleWebApp.Models.Entities
{
    [Table("time_table")]
    public class TimeTable
    {
        [Column("time_table_id")]
        public int TimeTableId { get; set; }

        [Column("start_time")]
        public TimeSpan StartTime { get; set; }

        [Column("end_time")]
        public TimeSpan EndTime { get; set; }

        [Column("lesson_number")]
        public int LessonNumber { get; set; }

        [Column("schedule_type_id")]
        public int? ScheduleTypeId { get; set; }

        [ForeignKey("ScheduleTypeId")]
        public ScheduleType ScheduleType { get; set; }
    }
}