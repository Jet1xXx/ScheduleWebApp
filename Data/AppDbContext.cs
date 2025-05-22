using Microsoft.EntityFrameworkCore;
using ScheduleWebApp.Models;

namespace ScheduleWebApp.Models.Entities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

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
                entity.HasKey(e => e.AcademicYearId);
                entity.Property(e => e.AcademicYearId).HasColumnName("academic_year_id");
                entity.Property(e => e.StartStudyDate).HasColumnName("start_study_date");
                entity.Property(e => e.EndStudyDate).HasColumnName("end_study_date");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("City");
                entity.HasKey(e => e.CityId);
                entity.Property(e => e.CityId).HasColumnName("city_id");
                entity.Property(e => e.CityName).HasColumnName("city_name").IsRequired();
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
                
                entity.HasOne(t => t.City)
                      .WithMany(c => c.Teachers)
                      .HasForeignKey(t => t.CityId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<CourseTeacher>(entity =>
            {
                entity.ToTable("CourseTeacher");
                entity.HasKey(e => e.CourseTeacherId);
                
                entity.HasOne(ct => ct.Teacher)
                      .WithMany()
                      .HasForeignKey(ct => ct.TeacherId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(ct => ct.Course)
                      .WithMany(c => c.CourseTeachers)
                      .HasForeignKey(ct => ct.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);
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
                entity.HasKey(e => e.StudentGroupId);
                
                entity.HasOne(sg => sg.Student)
                      .WithMany()
                      .HasForeignKey(sg => sg.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(sg => sg.Group)
                      .WithMany(g => g.StudentGroups)
                      .HasForeignKey(sg => sg.GroupId)
                      .OnDelete(DeleteBehavior.Cascade);
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
                entity.HasKey(e => e.ScheduleId);
                
                entity.HasOne(s => s.Room)
                      .WithMany(r => r.Schedules)
                      .HasForeignKey(s => s.RoomId)
                      .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(s => s.TimeTable)
                      .WithMany()
                      .HasForeignKey(s => s.TimeTableId)
                      .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(s => s.Enrollment)
                      .WithMany(e => e.Schedules)
                      .HasForeignKey(s => s.EnrollmentId)
                      .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(s => s.CreatedUser)
                      .WithMany(u => u.CreatedSchedules)
                      .HasForeignKey(s => s.CreatedBy)
                      .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(s => s.UpdatedUser)
                      .WithMany(u => u.UpdatedSchedules)
                      .HasForeignKey(s => s.UpdatedBy)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ScheduleReplacement>(entity =>
            {
                entity.ToTable("ScheduleReplacement");
                entity.HasKey(e => e.ScheduleReplacementId);
                
                entity.HasOne(sr => sr.Schedule)
                      .WithMany(s => s.Replacements)
                      .HasForeignKey(sr => sr.ScheduleId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TimeTable>(entity =>
            {
                entity.ToTable("TimeTable");
                entity.HasKey(e => e.TimeTableId);
                
                entity.HasOne(tt => tt.ScheduleType)
                      .WithMany(st => st.TimeTables)
                      .HasForeignKey(tt => tt.ScheduleTypeId)
                      .OnDelete(DeleteBehavior.SetNull);
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

            modelBuilder.Entity<Teacher>()
                .HasIndex(t => t.Email)
                .IsUnique()
                .HasFilter("[email] IS NOT NULL");
                
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.Email)
                .IsUnique()
                .HasFilter("[email] IS NOT NULL");
                
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();


        }
    }
}