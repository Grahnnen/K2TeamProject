using K2TeamProjectNEW.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2TeamProjectNEW.UI.ScheduleMenuUI
{
    public static class ScheduleMenu
    {
        public static void Show(DataService data)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==============================");
                Console.WriteLine("       Scheman");
                Console.WriteLine("==============================");
                Console.WriteLine("[1] Lägg till schemapost");
                Console.WriteLine("[2] Ta bort schemapost");
                Console.WriteLine("[3] Visa alla schemaposter");
                Console.WriteLine("[4] Tillbaka");
                Console.WriteLine("==============================");
                Console.Write("Välj alternativ: ");

                var menuChoiceScheduleMenu = Console.ReadLine();

                try
                {
                    switch (menuChoiceScheduleMenu)
                    {
                        case "1": //AddSchedulePost(data); break;
                        case "2": //RemoveSchedulePost(data); break;
                        case "3": //ListAllSchedulePosts(data); break;
                        case "4": return;
                        default:
                            Console.WriteLine("Felaktigt val.");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ett fel uppstod: {ex.Message}");
                    Console.ReadKey();
                }
            }
        }
    }
}
