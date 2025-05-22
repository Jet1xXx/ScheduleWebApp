using ScheduleWebApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleWebApp.Models.Entities
{
    [Table("schedule")]
    public class Schedule
    {
        [Column("schedule_id")]
        public int ScheduleId { get; set; }

        [Column("room_id")]
        public int RoomId { get; set; }

        [Column("schedule_date")]
        public DateTime SchedulеDate { get; set; }

        [Column("time_table_id")]
        public int TimeTableId { get; set; }

        [Column("enrollment_id")]
        public int EnrollmentId { get; set; }

        [Column("holiday_id")]
        public int? HolidayId { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_by")]
        public int UpdatedBy { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("RoomId")]
        public Room Room { get; set; }

        [ForeignKey("TimeTableId")]
        public TimeTable TimeTable { get; set; }

        [ForeignKey("EnrollmentId")]
        public Enrollment Enrollment { get; set; }

        [ForeignKey("HolidayId")]
        public Holiday Holiday { get; set; }

        [ForeignKey("CreatedBy")]
        public User CreatedUser { get; set; }

        [ForeignKey("UpdatedBy")]
        public User UpdatedUser { get; set; }

        public ICollection<ScheduleReplacement> Replacements { get; set; }
    }
}