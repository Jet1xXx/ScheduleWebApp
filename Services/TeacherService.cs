using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScheduleWebApp.Models.Entities;

namespace ScheduleWebApp.Services
{
    public class TeacherService
    {
        private readonly AppDbContext _context;

        public TeacherService(AppDbContext context)
        {
            _context = context;
        }

        public List<Teacher.TeacherListItemDto> GetAllTeachers()
        {
            return _context.Teachers
                .AsNoTracking()
                .Include(teacher => teacher.City)
                .OrderBy(teacher => teacher.LastName)
                .ThenBy(teacher => teacher.FirstName)
                .Select(teacher => new Teacher.TeacherListItemDto
                {
                    TeacherId = teacher.TeacherId,
                    FullName = $"{teacher.LastName} {teacher.FirstName} {teacher.MiddleName}".Trim(),
                    Email = teacher.Email,
                    Phone = teacher.Phone,
                    CityName = teacher.City != null ? teacher.City.CityName : null
                })
                .ToList();
        }

        public Teacher.TeacherDetailsDto GetTeacherDetails(int teacherId)
        {
            return _context.Teachers
                .AsNoTracking()
                .Include(teacher => teacher.City)
                .Where(teacher => teacher.TeacherId == teacherId)
                .Select(teacher => new Teacher.TeacherDetailsDto
                {
                    TeacherId = teacher.TeacherId,
                    FirstName = teacher.FirstName,
                    MiddleName = teacher.MiddleName,
                    LastName = teacher.LastName,
                    BirthDate = teacher.BirthDate,
                    Address = teacher.Address,
                    Email = teacher.Email,
                    Phone = teacher.Phone,
                    CityName = teacher.City != null ? teacher.City.CityName : null,
                    CityId = teacher.CityId
                })
                .FirstOrDefault();
        }

        public Teacher.TeacherEditDto GetTeacherForEdit(int teacherId)
        {
            return _context.Teachers
                .AsNoTracking()
                .Where(teacher => teacher.TeacherId == teacherId)
                .Select(teacher => new Teacher.TeacherEditDto
                {
                    TeacherId = teacher.TeacherId,
                    FirstName = teacher.FirstName,
                    MiddleName = teacher.MiddleName,
                    LastName = teacher.LastName,
                    BirthDate = teacher.BirthDate,
                    CityId = teacher.CityId,
                    Address = teacher.Address,
                    Email = teacher.Email,
                    Phone = teacher.Phone
                })
                .FirstOrDefault();
        }

        public List<SelectListItem> GetCitiesDropdown()
        {
            return _context.Cities
                .AsNoTracking()
                .OrderBy(city => city.CityName)
                .Select(city => new SelectListItem
                {
                    Value = city.CityId.ToString(),
                    Text = city.CityName
                })
                .ToList();
        }

        public void CreateTeacher(Teacher.TeacherEditDto teacherDto)
        {
            Teacher newTeacher = new Teacher
            {
                FirstName = teacherDto.FirstName,
                MiddleName = teacherDto.MiddleName,
                LastName = teacherDto.LastName,
                BirthDate = teacherDto.BirthDate.HasValue
                    ? DateTime.SpecifyKind(teacherDto.BirthDate.Value, DateTimeKind.Utc)
                    : null,
                CityId = teacherDto.CityId,
                Address = teacherDto.Address,
                Email = teacherDto.Email?.Trim().ToLower(),
                Phone = teacherDto.Phone
            };

            _context.Teachers.Add(newTeacher);
            _context.SaveChanges();

            teacherDto.TeacherId = newTeacher.TeacherId;
        }

        public void UpdateTeacher(Teacher.TeacherEditDto teacherDto)
        {
            Teacher existingTeacher = _context.Teachers.Find(teacherDto.TeacherId);
            if (existingTeacher == null) return;

            existingTeacher.FirstName = teacherDto.FirstName;
            existingTeacher.MiddleName = teacherDto.MiddleName;
            existingTeacher.LastName = teacherDto.LastName;
            existingTeacher.BirthDate = teacherDto.BirthDate;
            existingTeacher.CityId = teacherDto.CityId;
            existingTeacher.Address = teacherDto.Address;
            existingTeacher.Email = teacherDto.Email?.Trim().ToLower();
            existingTeacher.Phone = teacherDto.Phone;

            _context.SaveChanges();
        }

        public bool DeleteTeacher(int teacherId)
        {
            Teacher teacherToDelete = _context.Teachers.Find(teacherId);
            if (teacherToDelete == null) return false;

            _context.Teachers.Remove(teacherToDelete);
            _context.SaveChanges();
            return true;
        }

        public bool TeacherExists(int teacherId)
        {
            return _context.Teachers.Any(teacher => teacher.TeacherId == teacherId);
        }
    }
}
