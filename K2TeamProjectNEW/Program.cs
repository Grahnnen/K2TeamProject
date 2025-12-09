using K2TeamProjectNEW.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace K2TeamProjectNEW
{
    internal class Program
    {
        static void Main(string[] args)
		{
			var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
			var connectionString = builder.GetConnectionString("DefaultConnection");

			var options = new DbContextOptionsBuilder<K2TeamProjectDbContext>()
				.UseSqlServer(connectionString)
				.Options;

			using var context = new K2TeamProjectDbContext(options);

			// Quick test: fetch courses and their teacher
			var coursesWithTeachers = context.Courses
				.Include(c => c.FkTeacher)
				.Select(c => new
				{
					c.CourseID,
					c.CourseName,
					TeacherName = c.FkTeacher != null ? c.FkTeacher.TeacherName : "(no teacher assigned)"
				})
				.OrderBy(x => x.CourseName)
				.ToList();

			foreach (var item in coursesWithTeachers)
			{
				Console.WriteLine($"Course: {item.CourseName} (ID: {item.CourseID}) -> Teacher: {item.TeacherName}");
			}
		}
	}
}
