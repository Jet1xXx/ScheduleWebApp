using ScheduleWebApp.Models.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleWebApp.Models.Entities
{
    [Table("course_teacher")]
    public class CourseTeacher
    {
        [Column("course_teacher_id")]
        public int CourseTeacherId { get; set; }

        [Column("teacher_id")]
        public int TeacherId { get; set; }

        [ForeignKey("TeacherId")]
        public Teacher Teacher { get; set; }

        [Column("course_id")]
        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}