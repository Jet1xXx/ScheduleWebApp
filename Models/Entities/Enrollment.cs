using ScheduleWebApp.Models.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleWebApp.Models.Entities
{
    [Table("enrollment")]
    public class Enrollment
    {
        [Column("enrollment_id")]
        public int EnrollmentId { get; set; }

        [Column("course_teacher_id")]
        public int CourseTeacherId { get; set; }

        [ForeignKey("CourseTeacherId")]
        public CourseTeacher CourseTeacher { get; set; }

        [Column("group_id")]
        public int GroupId { get; set; }

        [ForeignKey("GroupId")]
        public Group Group { get; set; }

        public ICollection<Schedule> Schedules { get; set; }
    }
}