using K2TeamProject.models;
using Microsoft.EntityFrameworkCore;

namespace K2TeamProject.data
{
    public class K2TeamProjectDbContext : DbContext
    {
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Scheduling> Schedulings { get; set; }
        public DbSet<Student> Students { get; set; }
    }
}
