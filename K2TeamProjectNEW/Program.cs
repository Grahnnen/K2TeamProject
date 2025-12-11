using K2TeamProjectNEW.Data;
using K2TeamProjectNEW.Models;
using K2TeamProjectNEW.UI.MainMenuUI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace K2TeamProjectNEW
{
    internal class Program
    {
        static void Main(string[] args)
        {
			var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

			var connectionString = builder.GetConnectionString("DefaultConnection");

			var dbOptions = new DbContextOptionsBuilder<DatabaseFirstContext>()
                .UseSqlServer(connectionString)
                .Options;

            var databaseFirst = new DatabaseFirstContext(dbOptions);
            var codeFirst = new CodeFirstContext(
                new DbContextOptionsBuilder<CodeFirstContext>()
                    .UseSqlServer(connectionString)
                    .Options
            );

            var data = new DataService(codeFirst, databaseFirst);

            MainMenu.Show(data);
        }
    }
}
