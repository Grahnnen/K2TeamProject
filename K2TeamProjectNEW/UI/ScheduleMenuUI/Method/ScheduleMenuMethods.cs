using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using K2TeamProjectNEW.Data;
using K2TeamProjectNEW.Models;
using Microsoft.EntityFrameworkCore;

namespace K2TeamProjectNEW.UI.ScheduleMenuUI.Method
{
    public static class ScheduleMenuMethods
    {
        public static void AddSchedulePost(DataService data)
        {
            Console.Clear();
            Console.WriteLine("=== Lägg till ny schemapost ===\n");

            DateTime startTime;
            while (true)
            {
                Console.Write("Ange startdatum och tid (yyyy-MM-dd HH:mm): ");
                if (DateTime.TryParse(Console.ReadLine(), out startTime))
                {
                    break;
                }
                Console.WriteLine("Ogiltigt format! Använd yyyy-MM-dd HH:mm.");
            }

            DateTime endTime;
            while (true)
            {
                Console.Write("Ange slutdatum och tid (yyyy-MM-dd HH:mm): ");
                if (DateTime.TryParse(Console.ReadLine(), out endTime) && endTime > startTime)
                {
                    break;
                }
                Console.WriteLine("Ogiltigt format! Sluttid måste vara efter starttid.");
            }

            var courses = data.DatabaseFirst.Courses.ToList();
            if (!courses.Any())
            {
                Console.WriteLine("Inga kurser finns att schemalägga!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nTillgängliga kurser:");
            Console.WriteLine($"{"ID",-5} {"Kursnamn",-25}");
            Console.WriteLine(new string('-', 30));
            foreach (var c in courses)
            {
                Console.WriteLine($"{c.CourseID,-5} {c.CourseName,-25}");
            }

            int courseId;
            while (true)
            {
                Console.Write("\nAnge kursens ID: ");
                if (int.TryParse(Console.ReadLine(), out courseId) && courses.Any(c => c.CourseID == courseId))
                {
                    break;
                }
                Console.WriteLine("Ogiltigt ID!");
            }

            var classrooms = data.CodeFirst.Classrooms.ToList();
            if (!classrooms.Any())
            {
                Console.WriteLine("Inga klassrum finns att schemalägga!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nTillgängliga klassrum:");
            Console.WriteLine($"{"ID",-5} {"Klassrum",-15}");
            Console.WriteLine(new string('-', 20));
            foreach (var r in classrooms)
            {
                Console.WriteLine($"{r.ClassroomID,-5} {r.ClassroomName,-15}");
            }

            int classroomId;
            while (true)
            {
                Console.Write("\nAnge klassrummets ID: ");
                if (int.TryParse(Console.ReadLine(), out classroomId) && classrooms.Any(r => r.ClassroomID == classroomId))
                {
                    break;
                }
                Console.WriteLine("Ogiltigt ID!");
            }

            bool conflict = data.CodeFirst.Schedulings.Any(s =>
                s.FkClassroomID == classroomId &&
                s.SchedulingStartDateTime < endTime &&
                s.SchedulingEndDateTime > startTime);

            if (conflict)
            {
                Console.WriteLine("\nVarning: Detta klassrum är redan bokat under den angivna tiden!");
                Console.WriteLine("Schemaposten lades inte till.");
                Console.ReadKey();
                return;
            }

            var newScheduling = new Scheduling
            {
                SchedulingStartDateTime = startTime,
                SchedulingEndDateTime = endTime,
                FkCourseID = courseId,
                FkClassroomID = classroomId
            };

            try
            {
                data.CodeFirst.Schedulings.Add(newScheduling);
                data.CodeFirst.SaveChanges();

                Console.WriteLine("\n=== Schemapost tillagd! ===");
                Console.WriteLine($"ID: {newScheduling.SchedulingID}");
                Console.WriteLine($"Tid: {newScheduling.SchedulingStartDateTime:yyyy-MM-dd HH:mm} - {newScheduling.SchedulingEndDateTime:HH:mm}");
                Console.WriteLine($"Kurs ID: {courseId}, Klassrum ID: {classroomId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nEtt fel uppstod vid sparande: {ex.Message}");
            }

            Console.ReadKey();
        }

        public static void RemoveSchedulePost(DataService data)
        {
            Console.Clear();
            Console.WriteLine("=== Ta bort schemapost ===\n");

            var schedulings = data.CodeFirst.Schedulings
                .Include(s => s.Course)
                .Include(s => s.Classroom)
                .OrderBy(s => s.SchedulingStartDateTime)
                .ToList();

            if (!schedulings.Any())
            {
                Console.WriteLine("Inga schemaposter finns att ta bort!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"{"ID",-5} {"Starttid",-20} {"Sluttid",-20} {"Kurs",-25} {"Klassrum",-10}");
            Console.WriteLine(new string('-', 85));

            foreach (var s in schedulings)
            {
                Console.WriteLine(
                    $"{s.SchedulingID,-5} " +
                    $"{s.SchedulingStartDateTime:yyyy-MM-dd HH:mm},-20 " +
                    $"{s.SchedulingEndDateTime:yyyy-MM-dd HH:mm},-20 " +
                    $"{s.Course?.CourseName ?? "N/A",-25} " +
                    $"{s.Classroom?.ClassroomName ?? "N/A",-10}"
                );
            }

            Console.Write("\nAnge ID på schemaposten som ska tas bort: ");
            if (!int.TryParse(Console.ReadLine(), out int removeId))
            {
                Console.WriteLine("Ogiltigt ID!");
                Console.ReadKey();
                return;
            }

            var scheduleToRemove = data.CodeFirst.Schedulings.Find(removeId);

            if (scheduleToRemove == null)
            {
                Console.WriteLine($"Schemapost med ID {removeId} hittades inte!");
                Console.ReadKey();
                return;
            }

            try
            {
                data.CodeFirst.Schedulings.Remove(scheduleToRemove);
                data.CodeFirst.SaveChanges();
                Console.WriteLine($"\nSchemapost ID {removeId} har tagits bort!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nEtt fel uppstod vid borttagning: {ex.Message}");
            }

            Console.ReadKey();
        }

        public static void ListAllSchedulePosts(DataService data)
        {
            Console.Clear();
            Console.WriteLine("=== Alla schemaposter ===\n");

            var schedulings = data.CodeFirst.Schedulings
                .Include(s => s.Course)
                .Include(s => s.Classroom)
                .OrderBy(s => s.SchedulingStartDateTime)
                .ToList();

            if (!schedulings.Any())
            {
                Console.WriteLine("Inga schemaposter finns!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"{"ID",-5} {"Starttid",-20} {"Sluttid",-20} {"Kurs (ID)",-25} {"Klassrum (ID)",-15}");
            Console.WriteLine(new string('-', 90));

            foreach (var s in schedulings)
            {
                string courseInfo = s.Course != null ? $"{s.Course.CourseName} ({s.FkCourseID})" : "N/A";
                string classroomInfo = s.Classroom != null ? $"{s.Classroom.ClassroomName} ({s.FkClassroomID})" : "N/A";

                Console.WriteLine(
                    $"{s.SchedulingID,-5} " +
                    $"{s.SchedulingStartDateTime:yyyy-MM-dd HH:mm},-20 " +
                    $"{s.SchedulingEndDateTime:yyyy-MM-dd HH:mm},-20 " +
                    $"{courseInfo,-25} " +
                    $"{classroomInfo,-15}"
                );
            }

            Console.ReadKey();
        }
    }
}