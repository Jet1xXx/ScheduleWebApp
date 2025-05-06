using Microsoft.EntityFrameworkCore;
using ScheduleWebApp.Models;

namespace ScheduleWebApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<AcademicYear> AcademicYears { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<CourseTeacher> CourseTeachers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentGroup> StudentGroups { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ScheduleReplacement> ScheduleReplacements { get; set; }
        public DbSet<TimeTable> TimeTables { get; set; }
        public DbSet<ScheduleType> ScheduleTypes { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AcademicYear>(entity =>
            {
                entity.ToTable("AcademicYear");
                entity.Property(e => e.AcademicYearId).HasColumnName("academic_year_id");
                entity.Property(e => e.StartStudyDate).HasColumnName("start_study_date");
                entity.Property(e => e.EndStudyDate).HasColumnName("end_study_date");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("City");
                entity.Property(e => e.CityId).HasColumnName("city_id");
                entity.Property(e => e.CityName).HasColumnName("city_name");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");
                entity.Property(e => e.CourseId).HasColumnName("course_id");
                entity.Property(e => e.CourseName).HasColumnName("course_name");
                entity.Property(e => e.Description).HasColumnName("description");
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.ToTable("Teacher");
                entity.Property(e => e.TeacherId).HasColumnName("teacher_id");
                entity.Property(e => e.FirstName).HasColumnName("first_name");
                entity.Property(e => e.MiddleName).HasColumnName("middle_name");
                entity.Property(e => e.LastName).HasColumnName("last_name");
                entity.Property(e => e.BirthDate).HasColumnName("birth_date");
                entity.Property(e => e.Address).HasColumnName("address");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.Phone).HasColumnName("phone");
                entity.Property(e => e.CityId).HasColumnName("city_id");
            });

            modelBuilder.Entity<CourseTeacher>(entity =>
            {
                entity.ToTable("CourseTeacher");
                entity.Property(e => e.CourseTeacherId).HasColumnName("course_teacher_id");
                entity.Property(e => e.TeacherId).HasColumnName("teacher_id");
                entity.Property(e => e.CourseId).HasColumnName("course_id");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("Group");
                entity.Property(e => e.GroupId).HasColumnName("group_id");
                entity.Property(e => e.GroupName).HasColumnName("group_name");
                entity.Property(e => e.WeekType).HasColumnName("week_type");
                entity.Property(e => e.AcademicYearId).HasColumnName("academic_year_id");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");
                entity.Property(e => e.StudentId).HasColumnName("student_id");
                entity.Property(e => e.FirstName).HasColumnName("first_name");
                entity.Property(e => e.MiddleName).HasColumnName("middle_name");
                entity.Property(e => e.LastName).HasColumnName("last_name");
                entity.Property(e => e.BirthDate).HasColumnName("birth_date");
                entity.Property(e => e.Address).HasColumnName("address");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.Phone).HasColumnName("phone");
                entity.Property(e => e.ParentPhone).HasColumnName("parent_phone");
                entity.Property(e => e.CityId).HasColumnName("city_id");
            });

            modelBuilder.Entity<StudentGroup>(entity =>
            {
                entity.ToTable("StudentGroup");
                entity.Property(e => e.StudentGroupId).HasColumnName("student_group_id");
                entity.Property(e => e.StudentId).HasColumnName("student_id");
                entity.Property(e => e.GroupId).HasColumnName("group_id");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Room");
                entity.Property(e => e.RoomId).HasColumnName("room_id");
                entity.Property(e => e.RoomNumber).HasColumnName("room_number");
                entity.Property(e => e.Capacity).HasColumnName("capacity");
                entity.Property(e => e.RoomType).HasColumnName("room_type");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("Schedule");
                entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");
                entity.Property(e => e.SchedulеDate).HasColumnName("schedule_date");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.RoomId).HasColumnName("room_id");
                entity.Property(e => e.TimeTableId).HasColumnName("time_table_id");
                entity.Property(e => e.EnrollmentId).HasColumnName("enrollment_id");
                entity.Property(e => e.HolidayId).HasColumnName("holiday_id");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            modelBuilder.Entity<ScheduleReplacement>(entity =>
            {
                entity.ToTable("ScheduleReplacement");
                entity.Property(e => e.ScheduleReplacementId).HasColumnName("schedule_replacement_id");
                entity.Property(e => e.ReplacementDate).HasColumnName("replacement_date");
                entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");
                entity.Property(e => e.NewRoomId).HasColumnName("new_room_id");
                entity.Property(e => e.NewTimeTableId).HasColumnName("new_time_table_id");
                entity.Property(e => e.NewEnrollmentId).HasColumnName("new_enrollment_id");
            });

            modelBuilder.Entity<TimeTable>(entity =>
            {
                entity.ToTable("TimeTable");
                entity.Property(e => e.TimeTableId).HasColumnName("time_table_id");
                entity.Property(e => e.StartTime).HasColumnName("start_time");
                entity.Property(e => e.EndTime).HasColumnName("end_time");
                entity.Property(e => e.LessonNumber).HasColumnName("lesson_number");
                entity.Property(e => e.ScheduleTypeId).HasColumnName("schedule_type_id");
            });

            modelBuilder.Entity<ScheduleType>(entity =>
            {
                entity.ToTable("ScheduleType");
                entity.Property(e => e.ScheduleTypeId).HasColumnName("schedule_type_id");
                entity.Property(e => e.ScheduleTypeName).HasColumnName("schedule_type_name");
                entity.Property(e => e.Description).HasColumnName("description");
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.ToTable("Enrollment");
                entity.Property(e => e.EnrollmentId).HasColumnName("enrollment_id");
                entity.Property(e => e.CourseTeacherId).HasColumnName("course_teacher_id");
                entity.Property(e => e.GroupId).HasColumnName("group_id");
            });

            modelBuilder.Entity<Holiday>(entity =>
            {
                entity.ToTable("Holiday");
                entity.Property(e => e.HolidayId).HasColumnName("holiday_id");
                entity.Property(e => e.HolidayDate).HasColumnName("holiday_date");
                entity.Property(e => e.HolidayName).HasColumnName("holiday_name");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Username).HasColumnName("username");
                entity.Property(e => e.FullName).HasColumnName("full_name");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.HasMany(u => u.CreatedSchedules)
                      .WithOne(s => s.CreatedUser)
                      .HasForeignKey(s => s.CreatedBy)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.UpdatedSchedules)
                      .WithOne(s => s.UpdatedUser)
                      .HasForeignKey(s => s.UpdatedBy)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
