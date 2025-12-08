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

			var options = new DbContextOptionsBuilder<K2TeamProjectDbContext>().UseSqlServer(connectionString).Options;

			using var context = new K2TeamProjectDbContext(options);

		}
    }
}
