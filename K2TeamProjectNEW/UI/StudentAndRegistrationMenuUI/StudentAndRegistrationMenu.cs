using K2TeamProjectNEW.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2TeamProjectNEW.UI.StudentAndRegistrationMenuUI
{
    public static class StudentAndRegistrationMenu
    {
        public static void Show(DataService data)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==============================");
                Console.WriteLine("       Studenter & Registrering");
                Console.WriteLine("==============================");
                Console.WriteLine("[1] Lägg till student");
                Console.WriteLine("[2] Ta bort student");
                Console.WriteLine("[3] Registrera student för kurs");
                Console.WriteLine("[4] Ändra studentinformation");
                Console.WriteLine("[5] Lista studenter");
                Console.WriteLine("[6] Visa studentens kurser & betyg");
                Console.WriteLine("[7] Tillbaka");
                Console.WriteLine("==============================");
                Console.Write("Välj alternativ: ");

                var menuChoiceStudentAndRegistrationMenu = Console.ReadLine();

                try
                {
                    switch (menuChoiceStudentAndRegistrationMenu)
                    {
                        case "1": //AddStudent(data); break;
                        case "2": //RemoveStudent(data); break;
                        case "3": //RegisterStudentForCourse(data); break;
                        case "4": //EditStudentInformation(data); break;
                        case "5": //ListStudents(data); break;
                        case "6": //ShowStudentCoursesAndGrades(data); break;
                        case "7": return;
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
