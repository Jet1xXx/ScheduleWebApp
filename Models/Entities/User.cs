using ScheduleWebApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleWebApp.Models.Entities
{
    [Table("user")]
    public class User
    {
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("full_name")]
        public string FullName { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        public ICollection<Schedule> CreatedSchedules { get; set; }
        public ICollection<Schedule> UpdatedSchedules { get; set; }
    }
}