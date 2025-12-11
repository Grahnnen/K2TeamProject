using K2TeamProjectNEW.Data;
using K2TeamProjectNEW.UI.CourseAndTeacherMenuUI.Methods;
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
                Console.WriteLine("[6] Ta bort lärare");
                Console.WriteLine("[7] Koppla lärare till kurs");
                Console.WriteLine("[8] Tillbaka");
                Console.WriteLine("==============================");
                Console.Write("Välj alternativ: ");

                var menuChoiceCourseAndTeacherMenu = Console.ReadLine();

                try
                {
                    switch (menuChoiceCourseAndTeacherMenu)
                    {
                        case "1": CourseAndTeacherMethods.AddCourse(data); break;
                        case "2": CourseAndTeacherMethods.AddTeacher(data); break;
                        case "3": CourseAndTeacherMethods.ListCourses(data); break;
                        case "4": CourseAndTeacherMethods.ListTeachers(data); break;
                        case "5": CourseAndTeacherMethods.RemoveCourse(data); break;
                        case "6": CourseAndTeacherMethods.RemoveTeacher(data); break;
                        case "7": CourseAndTeacherMethods.LinkTeacherAndCourse(data); break;
                        case "8": return;
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
