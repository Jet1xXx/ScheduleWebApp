using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleWebApp.Models.Entities
{
    [Table("academic_year")]
    public class AcademicYear
    {
        [Column("academic_year_id")]
        public int AcademicYearId { get; set; }

        [Column("start_study_date")]
        public DateTime StartStudyDate { get; set; }

        [Column("end_study_date")]
        public DateTime EndStudyDate { get; set; }

        public ICollection<Group> Groups { get; set; }
    }
}