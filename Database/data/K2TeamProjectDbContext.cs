using K2TeamProject.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace K2TeamProject.data
{
    public class K2TeamProjectDbContext : DbContext
    {
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Scheduling> Schedulings { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Scheduling>()
                .HasOne(s => s.Course)
                .WithMany()
                .HasForeignKey(s => s.Fk_CourseID);

            modelBuilder.Entity<Course>()
                .HasOne<Teacher>()
                .WithMany()
                .HasForeignKey(c => c.Fk_TeacherID);

            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Student)
                .WithMany()
                .HasForeignKey(g => g.Fk_StudentID);

            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Teacher)
                .WithMany()
                .HasForeignKey(g => g.Fk_TeacherID);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Grade)
                .WithMany()
                .HasForeignKey(e => e.Fk_GradeID);

            modelBuilder.Entity<Teacher>().HasData(
                new Teacher { TeacherID = 1, TeacherName = "Ms. Eleanor Vance" },
                new Teacher { TeacherID = 2, TeacherName = "Mr. Thomas Harding" },
                new Teacher { TeacherID = 3, TeacherName = "Dr. Sarah Chen" }
            );

            modelBuilder.Entity<Course>().HasData(
                new Course
                {
                    CourseID = 1,
                    CourseName = "Mathematics I",
                    Fk_TeacherID = 1,
                    CourseStartDate = new DateTime(2025, 9, 1),
                    CourseEndDate = new DateTime(2025, 12, 15)
                },
                new Course
                {
                    CourseID = 2,
                    CourseName = "Computer Science",
                    Fk_TeacherID = 2,
                    CourseStartDate = new DateTime(2025, 9, 1),
                    CourseEndDate = new DateTime(2025, 12, 15)
                },
                new Course
                {
                    CourseID = 3,
                    CourseName = "History 101",
                    Fk_TeacherID = 3,
                    CourseStartDate = new DateTime(2025, 9, 1),
                    CourseEndDate = new DateTime(2025, 12, 15)
                },
                new Course
                {
                    CourseID = 4,
                    CourseName = "Mathematics II",
                    Fk_TeacherID = 1,
                    CourseStartDate = new DateTime(2026, 1, 7),
                    CourseEndDate = new DateTime(2026, 5, 30)
                }
            );

            modelBuilder.Entity<Classroom>().HasData(
                new Classroom { ClassroomID = 1, ClassroomName = "A101" },
                new Classroom { ClassroomID = 2, ClassroomName = "B205" },
                new Classroom { ClassroomID = 3, ClassroomName = "C303" }
            );

            modelBuilder.Entity<Student>().HasData(
                new Student { StudentID = 1, StudentFirstName = "Alice", StudentLastName = "Johnson" },
                new Student { StudentID = 2, StudentFirstName = "Bob", StudentLastName = "Williams" },
                new Student { StudentID = 3, StudentFirstName = "Charlie", StudentLastName = "Brown" },
                new Student { StudentID = 4, StudentFirstName = "Diana", StudentLastName = "Smith" }
            );

            modelBuilder.Entity<Grade>().HasData(
                new Grade { GradeID = 1, Fk_StudentID = 1, Fk_TeacherID = 1, GradeScale = "A", GradedDate = new DateOnly(2025, 12, 18) },
                new Grade { GradeID = 2, Fk_StudentID = 1, Fk_TeacherID = 2, GradeScale = "B+", GradedDate = new DateOnly(2025, 12, 18) },
                new Grade { GradeID = 3, Fk_StudentID = 2, Fk_TeacherID = 2, GradeScale = "A-", GradedDate = new DateOnly(2025, 12, 18) },
                new Grade { GradeID = 4, Fk_StudentID = 2, Fk_TeacherID = 3, GradeScale = "C", GradedDate = new DateOnly(2025, 12, 18) },
                new Grade { GradeID = 5, Fk_StudentID = 3, Fk_TeacherID = 1, GradeScale = "B", GradedDate = new DateOnly(2025, 12, 18) },
                new Grade { GradeID = 6, Fk_StudentID = 1, Fk_TeacherID = 1, GradeScale = "Pending", GradedDate = new DateOnly(2025, 9, 1) }
            );

            modelBuilder.Entity<Enrollment>().HasData(
                new Enrollment { EnrollmentID = 1, Fk_StudentID = 1, Fk_CourseID = 1, Fk_GradeID = 1, RegistrationDate = new DateOnly(2025, 9, 1) },
                new Enrollment { EnrollmentID = 2, Fk_StudentID = 1, Fk_CourseID = 2, Fk_GradeID = 2, RegistrationDate = new DateOnly(2025, 9, 1) },
                new Enrollment { EnrollmentID = 3, Fk_StudentID = 2, Fk_CourseID = 2, Fk_GradeID = 3, RegistrationDate = new DateOnly(2025, 9, 1) },
                new Enrollment { EnrollmentID = 4, Fk_StudentID = 2, Fk_CourseID = 3, Fk_GradeID = 4, RegistrationDate = new DateOnly(2025, 9, 1) },
                new Enrollment { EnrollmentID = 5, Fk_StudentID = 3, Fk_CourseID = 1, Fk_GradeID = 5, RegistrationDate = new DateOnly(2025, 9, 1) },
                new Enrollment { EnrollmentID = 6, Fk_StudentID = 4, Fk_CourseID = 1, Fk_GradeID = 6, RegistrationDate = new DateOnly(2025, 9, 1) },
                new Enrollment { EnrollmentID = 7, Fk_StudentID = 4, Fk_CourseID = 4, Fk_GradeID = 6, RegistrationDate = new DateOnly(2026, 1, 7) }
            );

            modelBuilder.Entity<Scheduling>().HasData(
                new Scheduling { SchedulingID = 1, Fk_CourseID = 1, Fk_ClassroomID = 1, SchedulingStartDateTime = new DateTime(2025, 9, 8, 8, 0, 0), SchedulingEndDateTime = new DateTime(2025, 9, 8, 9, 0, 0) },
                new Scheduling { SchedulingID = 2, Fk_CourseID = 2, Fk_ClassroomID = 2, SchedulingStartDateTime = new DateTime(2025, 9, 8, 9, 0, 0), SchedulingEndDateTime = new DateTime(2025, 9, 8, 10, 0, 0) },
                new Scheduling { SchedulingID = 3, Fk_CourseID = 3, Fk_ClassroomID = 3, SchedulingStartDateTime = new DateTime(2025, 9, 8, 10, 0, 0), SchedulingEndDateTime = new DateTime(2025, 9, 8, 11, 0, 0) },
                new Scheduling { SchedulingID = 4, Fk_CourseID = 1, Fk_ClassroomID = 1, SchedulingStartDateTime = new DateTime(2025, 9, 10, 8, 0, 0), SchedulingEndDateTime = new DateTime(2025, 9, 10, 9, 0, 0) },
                new Scheduling { SchedulingID = 5, Fk_CourseID = 4, Fk_ClassroomID = 1, SchedulingStartDateTime = new DateTime(2026, 1, 14, 9, 0, 0), SchedulingEndDateTime = new DateTime(2026, 1, 14, 10, 0, 0) }
            );
        }
    }
}
