using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace K2TeamProjectNEW.Data
{
    public static class AppBootstrapper
    {
        public static DataService Initialize()
        {
            // Unicode-symboler
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            // Ladda konfiguration från appsettings.json
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = builder.GetConnectionString("DefaultConnection");

            // Skapa DatabaseFirstContext
            var dbOptionsDatabaseFirst = new DbContextOptionsBuilder<DatabaseFirstContext>()
                .UseSqlServer(connectionString)
                .Options;
            var databaseFirst = new DatabaseFirstContext(dbOptionsDatabaseFirst);

            // Skapa CodeFirstContext
            var dbOptionsCodeFirst = new DbContextOptionsBuilder<CodeFirstContext>()
                .UseSqlServer(connectionString)
                .Options;
            var codeFirst = new CodeFirstContext(dbOptionsCodeFirst);

            // Skapa och returnera DataService
            return new DataService(codeFirst, databaseFirst);
        }
    }
}
