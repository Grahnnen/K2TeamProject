using K2TeamProjectNEW.Models;
using K2TeamProjectNEW.Models.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace K2TeamProjectNEW.Data
{
	public partial class CodeFirstContext : DbContext
	{
		public CodeFirstContext()
		{
		}

		public CodeFirstContext(DbContextOptions<CodeFirstContext> options)
			: base(options)
		{
		}
		public DbSet<Classroom> Classrooms { get; set; }
		public DbSet<Enrollment> Enrollments { get; set; }
		public DbSet<Grade> Grades { get; set; }
		public DbSet<Scheduling> Schedulings { get; set; }
		public DbSet<Student> Students { get; set; }

		public DbSet<StudentOverview> StudentOverview { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
            if (!optionsBuilder.IsConfigured)
            {
                // Try to read connection string from appsettings.json in the project directory.
                // Directory.GetCurrentDirectory() works reliably when running CLI or VS.
                var basePath = Directory.GetCurrentDirectory();
                var config = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                    .Build();

                var connectionString = config.GetConnectionString("DefaultConnection");
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new InvalidOperationException("Connection string 'DefaultConnection' not found in appsettings.json.");
                }

                optionsBuilder.UseSqlServer(connectionString);
            }
        }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Scheduling>()
				.HasOne(s => s.Course)
				.WithMany()
				.HasForeignKey(s => s.FkCourseID);

			modelBuilder.Entity<Grade>()
				.HasOne(g => g.Student)
				.WithMany()
				.HasForeignKey(g => g.FkStudentID);

			modelBuilder.Entity<Grade>()
				.HasOne(g => g.Teacher)
				.WithMany()
				.HasForeignKey(g => g.FkTeacherID);

			modelBuilder.Entity<Enrollment>()
				.HasOne(e => e.Grade)
				.WithMany()
				.HasForeignKey(e => e.FkGradeID)
				.OnDelete(DeleteBehavior.NoAction);
			modelBuilder.Entity<Enrollment>()
				.HasOne(e => e.Course)
				.WithMany()
				.HasForeignKey(e => e.FkCourseID)
				.OnDelete(DeleteBehavior.Cascade);
			modelBuilder.Entity<Enrollment>()
				.HasOne(e => e.Student)
				.WithMany()
				.HasForeignKey(e => e.FkStudentID)
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<StudentOverview>(eb =>
			{
				eb.HasNoKey();
				eb.ToView("vwStudentOverview");
				eb.Property(v => v.GradedDate).HasColumnType("date");
			});

			modelBuilder.Entity<Teacher>()
				.ToTable("Teacher", tb => tb.ExcludeFromMigrations());

			modelBuilder.Entity<Course>()
				.ToTable("Course", tb => tb.ExcludeFromMigrations());

			

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
				new Grade { GradeID = 1, FkStudentID = 1, FkTeacherID = 1, GradeScale = "A", GradedDate = new DateOnly(2025, 12, 18) },
				new Grade { GradeID = 2, FkStudentID = 1, FkTeacherID = 2, GradeScale = "B+", GradedDate = new DateOnly(2025, 12, 18) },
				new Grade { GradeID = 3, FkStudentID = 2, FkTeacherID = 2, GradeScale = "A-", GradedDate = new DateOnly(2025, 12, 18) },
				new Grade { GradeID = 4, FkStudentID = 2, FkTeacherID = 3, GradeScale = "C", GradedDate = new DateOnly(2025, 12, 18) },
				new Grade { GradeID = 5, FkStudentID = 3, FkTeacherID = 1, GradeScale = "B", GradedDate = new DateOnly(2025, 12, 18) },
				new Grade { GradeID = 6, FkStudentID = 1, FkTeacherID = 1, GradeScale = "Pending", GradedDate = new DateOnly(2025, 9, 1) }
			);

			modelBuilder.Entity<Enrollment>().HasData(
				new Enrollment { EnrollmentID = 1, FkStudentID = 1, FkCourseID = 1, FkGradeID = 1, RegistrationDate = new DateOnly(2025, 9, 1) },
				new Enrollment { EnrollmentID = 2, FkStudentID = 1, FkCourseID = 2, FkGradeID = 2, RegistrationDate = new DateOnly(2025, 9, 1) },
				new Enrollment { EnrollmentID = 3, FkStudentID = 2, FkCourseID = 2, FkGradeID = 3, RegistrationDate = new DateOnly(2025, 9, 1) },
				new Enrollment { EnrollmentID = 4, FkStudentID = 2, FkCourseID = 2, FkGradeID = 4, RegistrationDate = new DateOnly(2025, 9, 1) },
				new Enrollment { EnrollmentID = 5, FkStudentID = 3, FkCourseID = 1, FkGradeID = 5, RegistrationDate = new DateOnly(2025, 9, 1) },
				new Enrollment { EnrollmentID = 6, FkStudentID = 4, FkCourseID = 1, FkGradeID = 6, RegistrationDate = new DateOnly(2025, 9, 1) },
				new Enrollment { EnrollmentID = 7, FkStudentID = 4, FkCourseID = 2, FkGradeID = 6, RegistrationDate = new DateOnly(2026, 1, 7) }
			);

			modelBuilder.Entity<Scheduling>().HasData(
				new Scheduling { SchedulingID = 1, FkCourseID = 1, FkClassroomID = 1, SchedulingStartDateTime = new DateTime(2025, 9, 8, 8, 0, 0), SchedulingEndDateTime = new DateTime(2025, 9, 8, 9, 0, 0) },
				new Scheduling { SchedulingID = 2, FkCourseID = 2, FkClassroomID = 2, SchedulingStartDateTime = new DateTime(2025, 9, 8, 9, 0, 0), SchedulingEndDateTime = new DateTime(2025, 9, 8, 10, 0, 0) },
				new Scheduling { SchedulingID = 3, FkCourseID = 2, FkClassroomID = 3, SchedulingStartDateTime = new DateTime(2025, 9, 8, 10, 0, 0), SchedulingEndDateTime = new DateTime(2025, 9, 8, 11, 0, 0) },
				new Scheduling { SchedulingID = 4, FkCourseID = 1, FkClassroomID = 1, SchedulingStartDateTime = new DateTime(2025, 9, 10, 8, 0, 0), SchedulingEndDateTime = new DateTime(2025, 9, 10, 9, 0, 0) },
				new Scheduling { SchedulingID = 5, FkCourseID = 2, FkClassroomID = 1, SchedulingStartDateTime = new DateTime(2026, 1, 14, 9, 0, 0), SchedulingEndDateTime = new DateTime(2026, 1, 14, 10, 0, 0) }
			);

			OnModelCreatingPartial(modelBuilder);
		}
		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
	}
}
