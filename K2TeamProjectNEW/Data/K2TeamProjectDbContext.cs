using System;
using System.Collections.Generic;
using K2TeamProjectNEW.Models;
using Microsoft.EntityFrameworkCore;

namespace K2TeamProjectNEW.Data;

public partial class K2TeamProjectDbContext : DbContext
{
    public K2TeamProjectDbContext()
    {
    }

    public K2TeamProjectDbContext(DbContextOptions<K2TeamProjectDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	=> optionsBuilder.UseSqlServer("Server=localhost;Database=K2SchoolDb;Trusted_Connection=True;TrustServerCertificate=True;");

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
			entity.Property(e => e.Fk_TeacherID).HasColumnName("Fk_TeacherID");

			entity.HasOne(d => d.FkTeacher).WithMany(p => p.Courses)
				.HasForeignKey(d => d.Fk_TeacherID)
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
				Fk_TeacherID = 1
			},
			new Course
			{
				CourseID = 2,
				CourseName = "Engelska 2",
				CourseStartDate = new DateOnly(2025, 12, 20),
				CourseEndDate = new DateOnly(2025, 12, 21),
				Fk_TeacherID = 1
			}
		);

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
