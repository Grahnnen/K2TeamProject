using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using K2TeamProjectNEW.Data;
using K2TeamProjectNEW.Models;
using Microsoft.EntityFrameworkCore;

namespace K2TeamProjectNEW.UI.CourseAndTeacherMenuUI.Methods
{
    public static class CourseAndTeacherMethods
    {
        // Lägg till kurs
        public static void AddCourse(DataService data)
        {
            Console.Clear();
            Console.WriteLine("=== Lägg till kurs ===\n");

            Console.Write("Ange kursens namn: ");
            var courseName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(courseName) || courseName.All(char.IsDigit))
            {
                Console.WriteLine("Ogiltigt kursnamn!");
                Console.ReadKey();
                return;
            }

            if (data.DatabaseFirst.Courses
                .Any(c => c.CourseName.ToLower() == courseName.ToLower()))
            {
                Console.WriteLine("\nEn kurs med detta namn finns redan!");
                Console.ReadKey();
                return;
            }

            Console.Write("Ange kursens startdatum (yyyy-mm-dd): ");
            var startDateInput = Console.ReadLine();

            Console.Write("Ange kursens slutdatum (yyyy-mm-dd): ");
            var endDateInput = Console.ReadLine();

            if (!DateOnly.TryParse(startDateInput, out var startDate) ||
                !DateOnly.TryParse(endDateInput, out var endDate) ||
                endDate < startDate)
            {
                Console.WriteLine("Ogiltigt datum! Slutdatum får inte vara före startdatum!");
                Console.ReadKey();
                return;
            }

            bool attachTeacher = false;
            while (true)
            {
                Console.Clear();

                Console.WriteLine("=== Lägg till kurs ===\n");
                Console.WriteLine($"Kursnamn: {courseName}");
                Console.WriteLine($"Startdatum: {startDate:yyyy-MM-dd}");
                Console.WriteLine($"Slutdatum: {endDate:yyyy-MM-dd}");

                Console.Write("\nVill du koppla denna kurs till en lärare? (Ja/Nej): ");
                var choice = Console.ReadLine()?.Trim();

                if (choice.Equals("Ja", StringComparison.OrdinalIgnoreCase))
                {
                    attachTeacher = true;
                    break;
                }

                else if (choice.Equals("Nej", StringComparison.OrdinalIgnoreCase))
                {
                    attachTeacher = false;
                    break;
                }
                else
                {
                    Console.WriteLine("\nOgiltigt val! Du måste skriva 'Ja' eller 'Nej'");
                    Console.ReadKey();
                }
            }

            int? teacherId = null;

            if (attachTeacher)
            {
                var teachers = data.DatabaseFirst.Teachers.ToList();
                if (teachers.Count == 0)
                {
                    Console.WriteLine("Inga lärare finns att tilldela!");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("\nTillgängliga lärare:\n");
                Console.WriteLine($"{"ID",-5} {"Namn",-20}");
                Console.WriteLine(new string('-', 25));

                foreach (var t in teachers)
                {
                    Console.WriteLine($"{t.TeacherID,-5} {t.TeacherName,-20}");
                }

                Console.Write("\nAnge lärarens ID: ");
                if (!int.TryParse(Console.ReadLine(), out int tId) || !teachers.Any(t => t.TeacherID == tId))
                {
                    Console.WriteLine("Ogiltigt ID eller lärare hittades inte!");
                    Console.ReadKey();
                    return;
                }

                teacherId = tId;
            }

            // Hämta nästa lediga courseid eftersom det inte genereras automatiskt
            int nextCourseId = data.DatabaseFirst.Courses.Any()
                ? data.DatabaseFirst.Courses.Max(c => c.CourseID) + 1
                : 1;

            var course = new Course
            {
                CourseID = nextCourseId,
                CourseName = courseName,
                CourseStartDate = startDate,
                CourseEndDate = endDate,
                FkTeacherID = teacherId // Null om ingen lärare kopplas
            };

            data.DatabaseFirst.Courses.Add(course);
            data.DatabaseFirst.SaveChanges();

            if (!teacherId.HasValue)
            {
                Console.WriteLine("\nKursen lades till utan lärare!");
            }

            Console.WriteLine("\n=== Kurs tillagd ===");
            Console.WriteLine($"Kurs: {course.CourseName}");
            Console.WriteLine($"KursID: {course.CourseID}");
            Console.WriteLine($"Startdatum: {course.CourseStartDate}");
            Console.WriteLine($"Slutdatum: {course.CourseEndDate}");

            if (teacherId.HasValue)
            {
                var teacher = data.DatabaseFirst.Teachers.First(t => t.TeacherID == teacherId);
                Console.WriteLine($"Tilldelad lärare: {teacher.TeacherName} (ID: {teacher.TeacherID})");
            }
            else
            {
                Console.WriteLine("Ingen lärare kopplad!");
            }

            Console.ReadKey();
        }

        // Lägg till lärare
        public static void AddTeacher(DataService data)
        {
            Console.Clear();
            Console.WriteLine("=== Lägg till lärare ===\n");

            Console.Write("Ange lärarens namn: ");
            var teacherName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(teacherName) || teacherName.All(char.IsDigit))
            {
                Console.WriteLine("Ogiltigt lärarnamn!");
                Console.ReadKey();
                return;
            }

            // Hämta nästa lediga teacherid eftersom det inte genereras automatiskt
            int nextTeacherId = data.DatabaseFirst.Teachers.Any()
            ? data.DatabaseFirst.Teachers.Max(t => t.TeacherID) + 1
            :1;

            var teacher = new Teacher
            {
                TeacherID = nextTeacherId, // Tilldelar nytt id
                TeacherName = teacherName
            };

            data.DatabaseFirst.Teachers.Add(teacher);
            data.DatabaseFirst.SaveChanges();

            bool attachCourses = false;
            while (true)
            {
                Console.Clear();

                Console.WriteLine("=== Lägg till lärare ===\n");
                Console.WriteLine($"Lärarnamn: {teacherName}");
                Console.WriteLine($"LärareID: {nextTeacherId}");

                Console.Write("\nVill du koppla denna lärare till en kurs? (Ja/Nej): ");
                var choice = Console.ReadLine()?.Trim();

                if (choice.Equals("Ja", StringComparison.OrdinalIgnoreCase))
                {
                    attachCourses = true;
                    break;
                }

                else if (choice.Equals("Nej", StringComparison.OrdinalIgnoreCase))
                {
                    attachCourses = false;
                    break;
                }
                else
                {
                    Console.WriteLine("\nOgiltigt val! Du måste skriva 'Ja' eller 'Nej'");
                    Console.ReadKey();
                }
            }

            var linkedCourses = new List<string>();

            if (attachCourses)
            {
                // Koppla tillagd lärare till kurser
                var courses = data.DatabaseFirst.Courses
                .Include(c => c.FkTeacher)
                .OrderBy(c => c.CourseID)
                .ToList();

                if (courses.Count > 0)
                {
                    Console.WriteLine("\nTillgängliga kurser:\n");
                    Console.WriteLine($"{"ID",-5} {"Kurs",-25} {"Lärare",-20} {"Start",-12} {"Slut",-12}");
                    Console.WriteLine(new string('-', 80));

                    foreach (var c in courses)
                    {
                        string teacherNameCourse = c.FkTeacher != null ? c.FkTeacher.TeacherName : "-";
                        string startDate = c.CourseStartDate.HasValue ? c.CourseStartDate.Value.ToString("yyyy-MM-dd") : "-";
                        string endDate = c.CourseEndDate.HasValue ? c.CourseEndDate.Value.ToString("yyyy-MM-dd") : "-";

                        Console.WriteLine($"{c.CourseID,-5} {c.CourseName,-25} {teacherNameCourse,-20} {startDate,-12} {endDate,-12}");
                    }

                    Console.Write("\nAnge kurs-ID som läraren ska undervisa: ");
                    var courseIdInput = Console.ReadLine();

                    if (!string.IsNullOrEmpty(courseIdInput))
                    {
                        var ids = courseIdInput.Split(',')
                                                .Select(s => int.TryParse(s.Trim(), out var id) ? id : -1)
                                                .Where(id => id != -1);

                        foreach (var courseId in ids)
                        {
                            var course = data.DatabaseFirst.Courses.Find(courseId);
                            if (course != null)
                            {
                                course.FkTeacherID = teacher.TeacherID;
                                linkedCourses.Add(course.CourseName);
                            }
                        }

                        data.DatabaseFirst.SaveChanges();
                    }
                }
            }

            Console.WriteLine("\n=== Lärare tillagd ===");
            Console.WriteLine($"Namn: {teacher.TeacherName}");
            Console.WriteLine($"ID: {teacher.TeacherID}");
            if (linkedCourses.Count > 0)
                Console.WriteLine("Undervisar kurs: " + string.Join(", ", linkedCourses));
            else
                Console.WriteLine("Inga kurser kopplade!");

            Console.ReadKey();
        }

        // Lista alla kurser
        public static void ListCourses(DataService data)
        {
            Console.Clear();
            Console.WriteLine("=== Alla kurser ===\n");

            var courses = data.DatabaseFirst.Courses
                .OrderBy(c => c.CourseID)
                .Include(c => c.FkTeacher)
                .ToList();

            if (!courses.Any())
            {
                Console.WriteLine("Inga kurser finns!");
                return;
            }

            // Tabellrubik
            Console.WriteLine($"{"Kurser",-25} {"Lärare",-30} {"Start",-12} {"Slut",-12}");
            Console.WriteLine(new string('-', 85));

            foreach (var c in courses)
            {
                string teacherInfo = c.FkTeacher != null
                ? $"{c.FkTeacher.TeacherName} [ID:{c.FkTeacher.TeacherID}]"
                : "-";

                string startDate = c.CourseStartDate.HasValue
                    ? c.CourseStartDate.Value.ToString("yyyy-MM-dd")
                    : "-";

                string endDate = c.CourseEndDate.HasValue
                    ? c.CourseEndDate.Value.ToString("yyyy-MM-dd")
                    : "-";

                Console.WriteLine(
                    $"{c.CourseName + " [ID:" + c.CourseID + "]",-25} " +
                    $"{teacherInfo,-30} " +
                    $"{startDate,-12} " +
                    $"{endDate,-12}"
                );
            }
            Console.ReadKey();
        }

        // Lista alla lärare
        public static void ListTeachers(DataService data)
        {
            Console.Clear();
            Console.WriteLine("=== Alla lärare ===\n");

            var teachers = data.DatabaseFirst.Teachers
                .Include(t => t.Courses)
                .OrderBy(t => t.TeacherID)
                .ToList();

            if (!teachers.Any())
            {
                Console.WriteLine("Inga lärare finns!");
                return;
            }

            Console.WriteLine($"{"Lärare",-30} {"LärareID",-10} {"Kurser (ID)"}");
            Console.WriteLine(new string('-', 75));

            foreach (var t in teachers)
            {

                if (t.Courses != null && t.Courses.Any())
                {
                    string courses = string.Join(", ", t.Courses.Select(c => $"{c.CourseName} ({c.CourseID})"));
                    Console.WriteLine($"{t.TeacherName,-30} {t.TeacherID,-10} {courses}");
                }
                else
                {
                    // Lärare utan kurser
                    Console.WriteLine($"{t.TeacherName,-30} {t.TeacherID,-10} -");
                }
            }
            Console.ReadKey();
        }

        // Ta bort kurs
        public static void RemoveCourse(DataService data)
        {
            Console.Clear();
            Console.WriteLine("=== Ta bort kurs ===\n");

            var courses = data.DatabaseFirst.Courses
                .Include(c => c.FkTeacher)
                .OrderBy(c => c.CourseID)
                .ToList();

            if (!courses.Any())
            {
                Console.WriteLine("Inga kurser finns att ta bort!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"{"ID",-5} {"Kurs",-25} {"Lärare",-20} {"Start",-12} {"Slut",-12}");
            Console.WriteLine(new string('-', 80));

            foreach (var c in courses)
            {
                string teacherName = c.FkTeacher != null ? c.FkTeacher.TeacherName : "-";
                string startDate = c.CourseStartDate.HasValue ? c.CourseStartDate.Value.ToString("yyyy-MM-dd") : "-";
                string endDate = c.CourseEndDate.HasValue ? c.CourseEndDate.Value.ToString("yyyy-MM-dd") : "-";

                Console.WriteLine($"{c.CourseID,-5} {c.CourseName,-25} {teacherName,-20} {startDate,-12} {endDate,-12}");
            }

            Console.WriteLine("\nAnge ID på kursen som ska tas bort: ");
            var courseIdInput = Console.ReadLine();

            var courseIds = courseIdInput.Split(',')
                                 .Select(s => int.TryParse(s.Trim(), out var id) ? id : -1)
                                 .Where(id => id != -1)
                                 .Distinct()
                                 .ToList();

            if (!courseIds.Any())
            {
                Console.WriteLine("Ogiltigt ID eller kurs hittades inte!");
                Console.ReadKey();
                return;
            }

            foreach (var courseId in courseIds)
            {
                var courseToRemove = data.DatabaseFirst.Courses
                .Include(c => c.FkTeacher)
                .FirstOrDefault(c => c.CourseID == courseId);

                if (courseToRemove == null)
                {
                    Console.WriteLine($"Kursen med ID {courseId} hittades inte!");
                    continue;
                }

                /// Hämta kopplingar som listor
                var enrollmentsToUpdate = data.CodeFirst.Enrollments
                    .Where(e => e.FkCourseID == courseId)
                    .ToList();

                var schedulingsToUpdate = data.CodeFirst.Schedulings
                    .Where(s => s.FkCourseID == courseId)
                    .ToList();

                // Ta bort kursen (CASCADE tar hand om kopplingar i bakgrunden)
                data.DatabaseFirst.Courses.Remove(courseToRemove);
                data.DatabaseFirst.SaveChanges();

                Console.WriteLine($"\nKursen '{courseToRemove.CourseName}' har tagits bort!");
                if (enrollmentsToUpdate.Count > 0)
                    Console.WriteLine($"Kopplingar till {enrollmentsToUpdate.Count} registrering(ar) har tagits bort!");
                if (schedulingsToUpdate.Count > 0)
                    Console.WriteLine($"Kopplingar till {schedulingsToUpdate.Count} schemaläggning(ar) har tagits bort!");
            }

            Console.ReadKey();
        }

        //Ta bort lärare
        public static void RemoveTeacher(DataService data)
        {
            Console.Clear();
            Console.WriteLine("=== Ta bort lärare ===\n");

            var teachers = data.DatabaseFirst.Teachers
                .Include(t => t.Courses)
                .OrderBy(t => t.TeacherID)
                .ToList();

            if (!teachers.Any())
            {
                Console.WriteLine("Inga lärare finns att ta bort!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"{"ID",-5} {"Lärare",-25} {"Kurser"}");
            Console.WriteLine(new string('-', 60));

            foreach (var t in teachers)
            {
                string courses = t.Courses != null && t.Courses.Any()
                    ? string.Join(", ", t.Courses.Select(c => $"{c.CourseName} ({c.CourseID})"))
                    : "-";

                Console.WriteLine($"{t.TeacherID,-5} {t.TeacherName,-25} {courses}");
            }

            Console.WriteLine("\nAnge ID på vilken eller vilka lärare som ska tas bort: ");
            var teacherIdInput = Console.ReadLine();

            var teacherIds = teacherIdInput.Split(',')
                                           .Select(s => int.TryParse(s.Trim(), out var id) ? id : -1)
                                           .Where(id => id != -1)
                                           .Distinct()
                                           .ToList();

            if (!teacherIds.Any())
            {
                Console.WriteLine("Ogiltigt ID eller lärare hittades inte!");
                Console.ReadKey();
                return;
            }

            foreach (var teacherId in teacherIds)
            {
                var teacherToRemove = data.DatabaseFirst.Teachers
                    .Include(t => t.Courses)
                    .FirstOrDefault(t => t.TeacherID == teacherId);

                if (teacherToRemove == null)
                {
                    Console.WriteLine($"Läraren med ID {teacherId} hittades inte!");
                    continue;
                }

                // Hämta kurser och betyg som listor
                var coursesToUpdate = data.DatabaseFirst.Courses
                    .Where(c => c.FkTeacherID == teacherId)
                    .ToList();

                var gradesToUpdate = data.CodeFirst.Grades
                    .Where(g => g.FkTeacherID == teacherId)
                    .ToList();

                // Sätt FK till null på kurser
                foreach (var course in coursesToUpdate)
                    course.FkTeacherID = null;

                // Ta bort läraren (CASCADE tar hand om betyg)
                data.DatabaseFirst.Teachers.Remove(teacherToRemove);
                data.DatabaseFirst.SaveChanges();

                Console.WriteLine($"\nLäraren '{teacherToRemove.TeacherName}' har tagits bort!");
                if (coursesToUpdate.Count > 0)
                    Console.WriteLine($"Kopplingar till {coursesToUpdate.Count} kurs(er) har tagits bort!");
                if (gradesToUpdate.Count > 0)
                    Console.WriteLine($"Kopplingar till {gradesToUpdate.Count} betyg har tagits bort!");
            }

            Console.ReadKey();
        }

        // Koppla lärare till kurs
        public static void LinkTeacherAndCourse(DataService data)
        {
            Console.Clear();
            Console.WriteLine("=== Koppla lärare till kurs ===\n");

            // Hämta alla lärare och kurser
            var teachers = data.DatabaseFirst.Teachers
                .Include(t => t.Courses)
                .OrderBy(t => t.TeacherID)
                .ToList();

            var courses = data.DatabaseFirst.Courses
                .Include(c => c.FkTeacher)
                .OrderBy(c => c.CourseID)
                .ToList();

            if (!teachers.Any() || !courses.Any())
            {
                Console.WriteLine("Inga lärare eller kurser finns!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Vill du koppla:\n1) Lärare till kurs\n2) Kurs till lärare");
            var choiceConnect = Console.ReadLine()?.Trim();

            if (choiceConnect == "1")
            {
                // Visa lärare utan kurser = Courses-listan är tom eller null (lärare kan ha flera kurser)
                var teachersWithoutCourse = teachers.Where(t => t.Courses == null || !t.Courses.Any()).ToList();

                if (!teachersWithoutCourse.Any())
                {
                    Console.WriteLine("Alla lärare har redan kurser!");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("\nTillgängliga lärare utan kurser:");
                foreach (var t in teachersWithoutCourse)
                    Console.WriteLine($"{t.TeacherID} - {t.TeacherName}");

                Console.WriteLine("\nAnge lärarens ID: ");
                if (!int.TryParse(Console.ReadLine(), out int tId) || !teachersWithoutCourse.Any(t => t.TeacherID == tId))
                {
                    Console.WriteLine("Ogiltigt lärar-ID!");
                    Console.ReadKey();
                    return;
                }

                // Visa kurser utan lärare = FkTeacherID är null (1 kurs har max 1 lärare)
                var coursesWithoutTeachers = courses.Where(c => c.FkTeacherID == null).ToList();

                if (!coursesWithoutTeachers.Any())
                {
                    Console.WriteLine("Alla kurser har redan lärare!");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("\nTillgängliga kurser utan lärare:");
                foreach (var c in coursesWithoutTeachers)
                    Console.WriteLine($"{c.CourseID} - {c.CourseName}");

                Console.WriteLine("\nAnge kursens ID: ");
                if (!int.TryParse(Console.ReadLine(), out int cId) || !coursesWithoutTeachers.Any(c => c.CourseID == cId))
                {
                    Console.WriteLine("Ogiltigt kurs-ID!");
                    Console.ReadKey();
                    return;
                }

                var course = data.DatabaseFirst.Courses.Find(cId);
                course.FkTeacherID = tId;
                data.DatabaseFirst.SaveChanges();

                Console.WriteLine($"\nLäraren '{teachers.First(t => t.TeacherID == tId).TeacherName}' har kopplats till kursen '{course.CourseName}'");
            }
            else if (choiceConnect == "2")
            {
                var coursesWithoutTeachers = courses.Where(c => c.FkTeacherID == null).ToList();

                if (!coursesWithoutTeachers.Any())
                {
                    Console.WriteLine("Alla kurser har redan lärare!");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("\nTillgängliga kurser utan lärare:");
                foreach (var c in coursesWithoutTeachers)
                    Console.WriteLine($"{c.CourseID} - {c.CourseName}");

                Console.WriteLine("\nAnge kursens ID: ");
                if (!int.TryParse(Console.ReadLine(), out int cId) || !coursesWithoutTeachers.Any(c => c.CourseID == cId))
                {
                    Console.WriteLine("Ogiltigt kurs-ID!");
                    Console.ReadKey();
                    return;
                }

                var teachersWithoutCourse = teachers.Where(t => t.Courses == null || !t.Courses.Any()).ToList();

                if (!teachersWithoutCourse.Any())
                {
                    Console.WriteLine("Alla lärare har redan kurser!");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("\nTillgängliga lärare utan kurser:");
                foreach (var t in teachersWithoutCourse)
                    Console.WriteLine($"{t.TeacherID} - {t.TeacherName}");

                Console.WriteLine("\nAnge lärarens ID: ");
                if (!int.TryParse(Console.ReadLine(), out int tId) || !teachersWithoutCourse.Any(t => t.TeacherID == tId))
                {
                    Console.WriteLine("Ogiltigt lärar-ID!");
                    Console.ReadKey();
                    return;
                }

                var course = data.DatabaseFirst.Courses.Find(cId);
                course.FkTeacherID = tId;
                data.DatabaseFirst.SaveChanges();

                Console.WriteLine($"\nKursen '{course.CourseName}' har kopplats till läraren '{teachers.First(t => t.TeacherID == tId).TeacherName}'");
            }
            else
            {
                Console.WriteLine("Ogiltigt val!");
            }

            Console.ReadKey();
        }
    }
}
