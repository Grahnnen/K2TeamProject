using K2TeamProjectNEW.Data;
using K2TeamProjectNEW.UI.MainMenuUI;
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

			var options = new DbContextOptionsBuilder<DatabaseFirstContext>().UseSqlServer(connectionString).Options;

			using var context = new DatabaseFirstContext(options);

            var codeFirst = new CodeFirstContext();

            var databaseFirst = new DatabaseFirstContext();

            var data = new DataService(codeFirst, databaseFirst);

            MainMenu.Show(data);
        }
    }
}
