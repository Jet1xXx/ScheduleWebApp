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

        //доработать 
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
       
        public Teacher.TeacherDto GetTeacher(int teacherId)
        {
            return _context.Teachers
                .AsNoTracking()
                .Include(t => t.City)
                .Where(t => t.TeacherId == teacherId)
                .Select(t => new Teacher.TeacherDto
                {
                    TeacherId = t.TeacherId,
                    FirstName = t.FirstName,
                    MiddleName = t.MiddleName,
                    LastName = t.LastName,
                    BirthDate = t.BirthDate,
                    CityId = t.CityId,
                    Address = t.Address,
                    Email = t.Email,
                    Phone = t.Phone,
                    CityName = t.City != null ? t.City.CityName : null
                })
                .FirstOrDefault();
        }

       

        public void CreateTeacher(Teacher.TeacherDto teacherDto)
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


        //ОШИБКИ В КОНТРОЛЛЕРЕ
        public void UpdateTeacher(Teacher.TeacherDto teacherDto)
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

      
    }
}
