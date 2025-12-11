using K2TeamProjectNEW.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace K2TeamProjectNEW.Data;

public partial class DatabaseFirstContext : DbContext
{
    public DatabaseFirstContext()
    {
    }

    public DatabaseFirstContext(DbContextOptions<DatabaseFirstContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

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
		modelBuilder.Entity<Course>(entity =>
		{
			entity.HasKey(e => e.CourseID).HasName("PK__Course__C92D71879A78CCFE");

			entity.ToTable("Course");

			entity.Property(e => e.CourseID)
				.ValueGeneratedNever()
				.HasColumnName("CourseID");
			entity.Property(e => e.CourseName).HasMaxLength(50);
			entity.Property(e => e.FkTeacherID).HasColumnName("FkTeacherID");

			entity.HasOne(d => d.FkTeacher).WithMany(p => p.Courses)
				.HasForeignKey(d => d.FkTeacherID)
				.HasConstraintName("FK__Course__Fk_Teach__398D8EEE");
		});

		modelBuilder.Entity<Teacher>(entity =>
		{
			entity.HasKey(e => e.TeacherID).HasName("PK__Teacher__EDF259440181DB12");

			entity.ToTable("Teacher");

			entity.Property(e => e.TeacherID)
				.ValueGeneratedNever()
				.HasColumnName("TeacherID");
			entity.Property(e => e.TeacherName).HasMaxLength(100);
		});

		// Seed data
		modelBuilder.Entity<Teacher>().HasData(
			new Teacher { TeacherID = 1, TeacherName = "Markus Silva" },
			new Teacher { TeacherID = 2, TeacherName = "Jenny Larsson" },
			new Teacher { TeacherID = 3, TeacherName = "Henrik Andersson" }
		);

		modelBuilder.Entity<Course>().HasData(
			new Course
			{
				CourseID = 1,
				CourseName = "Engelska 1",
				CourseStartDate = new DateOnly(2025, 12, 18),
				CourseEndDate = new DateOnly(2025, 12, 19),
				FkTeacherID = 1
			},
			new Course
			{
				CourseID = 2,
				CourseName = "Engelska 2",
				CourseStartDate = new DateOnly(2025, 12, 20),
				CourseEndDate = new DateOnly(2025, 12, 21),
				FkTeacherID = 1
			}
		);

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
