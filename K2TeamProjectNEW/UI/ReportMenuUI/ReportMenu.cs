using K2TeamProjectNEW.Data;
using K2TeamProjectNEW.UI.ReportMenuUI.Method;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2TeamProjectNEW.UI.ReportMenuUI
{
    public static class ReportMenu
    {
        public static void Show(DataService data)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==============================");
                Console.WriteLine("       Rapporter");
                Console.WriteLine("==============================");
                Console.WriteLine("[1] Studentöversikt");
                Console.WriteLine("[2] Aktiva kurser");
                Console.WriteLine("[3] Betygsstatistik");
                Console.WriteLine("[4] Tillbaka");
                Console.WriteLine("==============================");
                Console.Write("Välj alternativ: ");

                var menuChoiceReportMenu = Console.ReadLine();

                try
                {
                    switch (menuChoiceReportMenu)
                    {
                        case "1": ReportMenuMethods.ShowStudentOverview(data); break;
                        case "2": ReportMenuMethods.ShowActiveCourses(data); break;
                        case "3": ReportMenuMethods.ShowGradeStatistics(data); break;
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
