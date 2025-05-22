using ScheduleWebApp.Models.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleWebApp.Models.Entities
{
    [Table("group")]
    public class Group
    {
        [Column("group_id")]
        public int GroupId { get; set; }

        [Column("group_name")]
        public string GroupName { get; set; }

        [Column("academic_year_id")]
        public int AcademicYearId { get; set; }

        [ForeignKey("AcademicYearId")]
        public AcademicYear AcademicYear { get; set; }

        [Column("week_type")]
        public string WeekType { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<StudentGroup> StudentGroups { get; set; }
    }
}