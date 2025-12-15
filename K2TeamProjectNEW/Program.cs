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
            // Initialisera alla databaskopplingar och DataService
            var data = AppBootstrapper.Initialize();

            // Starta huvudmenyn
            MainMenu.Show(data);
        }
    }
}
