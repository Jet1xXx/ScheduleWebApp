using ScheduleWebApp.Models.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleWebApp.Models.Entities
{
    [Table("city")]
    public class City
    {
        [Column("city_id")]
        public int CityId { get; set; }

        [Column("city_name")]
        public string CityName { get; set; }

        public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
    }
}