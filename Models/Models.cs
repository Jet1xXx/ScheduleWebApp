using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleWebApp.Models
{
    public class AcademicYear
    {
        public int AcademicYearId { get; set; }
        public DateTime StartStudyDate { get; set; }
        public DateTime EndStudyDate { get; set; }
        public ICollection<Group> Groups { get; set; }
    }

    public class City
    {
        public int CityId { get; set; } 

        public string CityName { get; set; }

        public ICollection<Teacher> Teachers { get; set; }
    }


    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public ICollection<CourseTeacher> CourseTeachers { get; set; }
    }

    public class Teacher
    {
        public int TeacherId { get; set; } 

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        public int? CityId { get; set; }  

        public City City { get; set; }

        public string Address { get; set; }
        public string Email { get; set; }

        [Phone(ErrorMessage = "Неверный формат номера телефона")]
        public string Phone { get; set; }
    }


    public class CourseTeacher
    {
        public int CourseTeacherId { get; set; }
        public int TeacherId { get; set; } 
        public Teacher Teacher { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }

    public class Group
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string WeekType { get; set; }
        public int AcademicYearId { get; set; }
        public AcademicYear AcademicYear { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<StudentGroup> StudentGroups { get; set; }
    }

    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? CityId { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

        [Phone(ErrorMessage = "Неверный формат телефона")]
        public string Phone { get; set; }
        public string ParentPhone { get; set; }
    }

    public class StudentGroup
    {
        public int StudentGroupId { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }

    public class Room
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public int Capacity { get; set; }
        public string RoomType { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
        public ICollection<ScheduleReplacement> Replacements { get; set; }
    }

    public class Schedule
    {
        public int ScheduleId { get; set; }
        public DateTime SchedulеDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public int TimeTableId { get; set; }
        public TimeTable TimeTable { get; set; }
        public int EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; }
        public int? HolidayId { get; set; }
        public Holiday Holiday { get; set; }
        public int CreatedBy { get; set; }
        public User CreatedUser { get; set; }
        public int UpdatedBy { get; set; }
        public User UpdatedUser { get; set; }
        public ICollection<ScheduleReplacement> Replacements { get; set; }
    }

    public class ScheduleReplacement
    {
        public int ScheduleReplacementId { get; set; }
        public DateTime ReplacementDate { get; set; }
        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }
        public int NewRoomId { get; set; }
        public Room NewRoom { get; set; }
        public int NewTimeTableId { get; set; }
        public TimeTable NewTimeTable { get; set; }
        public int NewEnrollmentId { get; set; }
        public Enrollment NewEnrollment { get; set; }
    }

    public class TimeTable
    {
        public int TimeTableId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int LessonNumber { get; set; }
        public int? ScheduleTypeId { get; set; }
        public ScheduleType ScheduleType { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
    }

    public class ScheduleType
    {
        public int ScheduleTypeId { get; set; }
        public string ScheduleTypeName { get; set; }
        public string Description { get; set; }
        public ICollection<TimeTable> TimeTables { get; set; }
    }

    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int CourseTeacherId { get; set; }
        public CourseTeacher CourseTeacher { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
    }

    public class Holiday
    {
        public int HolidayId { get; set; }
        public DateTime HolidayDate { get; set; }
        public string HolidayName { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
    }

    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public ICollection<Schedule> CreatedSchedules { get; set; }
        public ICollection<Schedule> UpdatedSchedules { get; set; }
    }
}