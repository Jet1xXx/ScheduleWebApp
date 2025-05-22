using ScheduleWebApp.Models.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleWebApp.Models.Entities
{
    [Table("course")]
    public class Course
    {
        [Column("course_id")]
        public int CourseId { get; set; }

        [Column("course_name")]
        public string CourseName { get; set; }

        [Column("description")]
        public string Description { get; set; }

        public ICollection<CourseTeacher> CourseTeachers { get; set; }
    }
}