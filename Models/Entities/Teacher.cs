using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleWebApp.Models.Entities
{
    [Table("Teacher")]
    public class Teacher
    {
        [Column("teacher_id")]
        public int TeacherId { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("middle_name")]
        public string MiddleName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [Column("birth_date")]
        public DateTime? BirthDate { get; set; }

        [Column("city_id")]
        public int? CityId { get; set; }

        [ForeignKey("CityId")]
        public City? City { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        public class TeacherListItemDto
        {
            public int TeacherId { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string CityName { get; set; }
        }

        //Новый универсальный DTO
        public class TeacherDto
        {
            public int TeacherId { get; set; }

            [Required]
            [StringLength(50)]
            public string FirstName { get; set; }

            [StringLength(50)]
            public string MiddleName { get; set; }

            [Required]
            [StringLength(50)]
            public string LastName { get; set; }

            public DateTime? BirthDate { get; set; }

            public int? CityId { get; set; }

            [StringLength(200)]
            public string Address { get; set; }

            [EmailAddress]
            [StringLength(100)]
            public string Email { get; set; }

            [Phone]
            [StringLength(20)]
            public string Phone { get; set; }

            // Добавлено для отображения (например в Details или List)
            [BindNever]
            [ScaffoldColumn(false)]
            public string? CityName { get; set; }

        }
    }
}
