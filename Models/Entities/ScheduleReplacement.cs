using ScheduleWebApp.Models.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleWebApp.Models.Entities
{
    [Table("schedule_replacement")]
    public class ScheduleReplacement
    {
        [Column("schedule_replacement_id")]
        public int ScheduleReplacementId { get; set; }

        [Column("schedule_id")]
        public int ScheduleId { get; set; }

        [Column("new_room_id")]
        public int NewRoomId { get; set; }

        [Column("replacement_date")]
        public DateTime ReplacementDate { get; set; }

        [Column("new_time_table_id")]
        public int NewTimeTableId { get; set; }

        [Column("new_enrollment_id")]
        public int NewEnrollmentId { get; set; }

        [ForeignKey("ScheduleId")]
        public Schedule Schedule { get; set; }

        [ForeignKey("NewRoomId")]
        public Room NewRoom { get; set; }

        [ForeignKey("NewTimeTableId")]
        public TimeTable NewTimeTable { get; set; }

        [ForeignKey("NewEnrollmentId")]
        public Enrollment NewEnrollment { get; set; }
    }
}