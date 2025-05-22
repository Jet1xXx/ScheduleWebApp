using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleWebApp.Models.Entities
{
    [Table("Teacher")]
    public class Teacher
    {
        [Key]
        [Column("teacher_id")]
        public int TeacherId { get; set; }

        [Required]
        [StringLength(50)]
        [Column("first_name")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Column("middle_name")]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(50)]
        [Column("last_name")]
        public string LastName { get; set; }

        [Column("birth_date")]
        public DateTime? BirthDate { get; set; }

        [Column("city_id")]
        public int? CityId { get; set; }

        [ForeignKey("CityId")]
        public City? City { get; set; }

        [StringLength(200)]
        [Column("address")]
        public string Address { get; set; }

        [EmailAddress]
        [Column("email")]
        public string Email { get; set; }

        [Phone]
        [Column("phone")]
        public string Phone { get; set; }

        // -------------------- DTOs --------------------

        public class TeacherListItemDto
        {
            public int TeacherId { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string CityName { get; set; }
        }

        public class TeacherDetailsDto
        {
            public int TeacherId { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public DateTime? BirthDate { get; set; }
            public string Address { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string CityName { get; set; }
            public int? CityId { get; set; }
        }

        public class TeacherEditDto
        {
            public int TeacherId { get; set; }

            [Required, StringLength(50)]
            public string FirstName { get; set; }

            [StringLength(50)]
            public string MiddleName { get; set; }

            [Required, StringLength(50)]
            public string LastName { get; set; }

            public DateTime? BirthDate { get; set; }

            public int? CityId { get; set; }

            [StringLength(200)]
            public string Address { get; set; }

            [EmailAddress]
            public string Email { get; set; }

            [Phone]
            public string Phone { get; set; }
        }

        // -------------------- Static Methods --------------------

        public static List<TeacherListItemDto> GetAllTeachers(AppDbContext context)
        {
            return context.Teachers
                .AsNoTracking()
                .Include(t => t.City)
                .OrderBy(t => t.LastName)
                .ThenBy(t => t.FirstName)
                .Select(t => new TeacherListItemDto
                {
                    TeacherId = t.TeacherId,
                    FullName = $"{t.LastName} {t.FirstName} {t.MiddleName}".Trim(),
                    Email = t.Email,
                    Phone = t.Phone,
                    CityName = t.City != null ? t.City.CityName : null

                })
                .ToList();
        }

        public static TeacherDetailsDto GetTeacherDetails(AppDbContext context, int id)
        {
            return context.Teachers
                .AsNoTracking()
                .Include(t => t.City)
                .Where(t => t.TeacherId == id)
                .Select(t => new TeacherDetailsDto
                {
                    TeacherId = t.TeacherId,
                    FirstName = t.FirstName,
                    MiddleName = t.MiddleName,
                    LastName = t.LastName,
                    BirthDate = t.BirthDate,
                    Address = t.Address,
                    Email = t.Email,
                    Phone = t.Phone,
                    CityName = t.City != null ? t.City.CityName : null,
                    CityId = t.CityId
                })
                .FirstOrDefault();
        }

        public static TeacherEditDto GetTeacherForEdit(AppDbContext context, int id)
        {
            return context.Teachers
                .AsNoTracking()
                .Where(t => t.TeacherId == id)
                .Select(t => new TeacherEditDto
                {
                    TeacherId = t.TeacherId,
                    FirstName = t.FirstName,
                    MiddleName = t.MiddleName,
                    LastName = t.LastName,
                    BirthDate = t.BirthDate,
                    CityId = t.CityId,
                    Address = t.Address,
                    Email = t.Email,
                    Phone = t.Phone
                })
                .FirstOrDefault();
        }

        public static List<SelectListItem> GetCitiesDropdown(AppDbContext context)
        {
            return context.Cities
                .AsNoTracking()
                .OrderBy(c => c.CityName)
                .Select(c => new SelectListItem
                {
                    Value = c.CityId.ToString(),
                    Text = c.CityName
                })
                .ToList();
        }

        public static void CreateTeacher(AppDbContext context, TeacherEditDto dto)
        {
            var teacher = new Teacher
            {
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                BirthDate = dto.BirthDate.HasValue
                    ? DateTime.SpecifyKind(dto.BirthDate.Value, DateTimeKind.Utc)
                    : null,
                CityId = dto.CityId,
                Address = dto.Address,
                Email = dto.Email,
                Phone = dto.Phone
            };

            context.Teachers.Add(teacher);
            context.SaveChanges();

            dto.TeacherId = teacher.TeacherId;
        }


        public static void UpdateTeacher(AppDbContext context, TeacherEditDto dto)
        {
            var teacher = context.Teachers.Find(dto.TeacherId);
            if (teacher == null) return;

            teacher.FirstName = dto.FirstName;
            teacher.MiddleName = dto.MiddleName;
            teacher.LastName = dto.LastName;
            teacher.BirthDate = dto.BirthDate;
            teacher.CityId = dto.CityId;
            teacher.Address = dto.Address;
            teacher.Email = dto.Email;
            teacher.Phone = dto.Phone;

            context.SaveChanges();
        }

        public static bool DeleteTeacher(AppDbContext context, int id)
        {
            var teacher = context.Teachers.Find(id);
            if (teacher == null) return false;

            context.Teachers.Remove(teacher);
            context.SaveChanges();
            return true;
        }

        public static bool TeacherExists(AppDbContext context, int id)
        {
            return context.Teachers.Any(t => t.TeacherId == id);
        }
    }
}
