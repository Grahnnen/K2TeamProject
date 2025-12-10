using K2TeamProjectNEW.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2TeamProjectNEW.UI.CourseAndTeacherMenuUI
{
    public static class CourseAndTeacherMenu
    {
        public static void Show(DataService data)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==============================");
                Console.WriteLine("       Kurser & Lärare");
                Console.WriteLine("==============================");
                Console.WriteLine("[1] Lägg till kurs");
                Console.WriteLine("[2] Lägg till lärare");
                Console.WriteLine("[3] Lista kurser");
                Console.WriteLine("[4] Lista lärare");
                Console.WriteLine("[5] Ta bort kurs");
                Console.WriteLine("[6] Tillbaka");
                Console.WriteLine("==============================");
                Console.Write("Välj alternativ: ");

                var menuChoiceCourseAndTeacherMenu = Console.ReadLine();

                try
                {
                    switch (menuChoiceCourseAndTeacherMenu)
                    {
                        case "1": //AddCourse(data); break;
                        case "2": //AddTeacher(data); break;
                        case "3": //ListCourse(data); break;
                        case "4": //ListTeacher(data); break;
                        case "5": //RemoveCourse(data); break;
                        case "6": return;
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
