using K2TeamProjectNEW.Data;
using K2TeamProjectNEW.UI.CourseAndTeacherMenuUI;
using K2TeamProjectNEW.UI.ReportMenuUI;
using K2TeamProjectNEW.UI.ScheduleMenuUI;
using K2TeamProjectNEW.UI.StudentAndRegistrationMenuUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2TeamProjectNEW.UI.MainMenuUI
{
    public static class MainMenu
    {
        public static void Show(DataService data)
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("==============================");
                Console.WriteLine("       Huvudmeny");
                Console.WriteLine("==============================");
                Console.WriteLine("[1] Kurser & Lärare");
                Console.WriteLine("[2] Studenter & Registrering");
                Console.WriteLine("[3] Scheman");
                Console.WriteLine("[4] Rapporter");
                Console.WriteLine("[0] Avsluta");
                Console.WriteLine("==============================");
                Console.Write("Välj alternativ: ");

                var menuChoiceMainMenu = Console.ReadLine();

                switch (menuChoiceMainMenu)
                {
                    case "1": CourseAndTeacherMenu.Show(data); break;
                    case "2": StudentAndRegistrationMenu.Show(data); break;
                    case "3": ScheduleMenu.Show(data); break;
                    case "4": ReportMenu.Show(data); break;
                    case "0": running = false; return;
                    default:
                        Console.WriteLine("Felaktigt val!");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}