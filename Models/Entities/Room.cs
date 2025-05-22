using ScheduleWebApp.Models.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleWebApp.Models.Entities
{
    [Table("room")]
    public class Room
    {
        [Column("room_id")]
        public int RoomId { get; set; }

        [Column("room_number")]
        public string RoomNumber { get; set; }

        [Column("capacity")]
        public int Capacity { get; set; }

        [Column("room_type")]
        public string RoomType { get; set; }

        public ICollection<Schedule> Schedules { get; set; }
        public ICollection<ScheduleReplacement> Replacements { get; set; }
    }
}