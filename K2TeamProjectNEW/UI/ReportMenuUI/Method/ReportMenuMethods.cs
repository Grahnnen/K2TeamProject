using K2TeamProjectNEW.Data;
using K2TeamProjectNEW.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace K2TeamProjectNEW.UI.ReportMenuUI.Method
{
	public class ReportMenuMethods
	{
		public static void ShowStudentOverview(DataService data)
		{
			var enrollments = data.CodeFirst.Set<Enrollment>()
				.Include(e => e.Student)
				.Include(e => e.Course)
					.ThenInclude(c => c.FkTeacher)
				.Include(e => e.Grade)
					.ThenInclude(g => g.Teacher)
				.OrderBy(e => e.Student.StudentLastName)
				.ThenBy(e => e.Student.StudentFirstName)
				.ThenBy(e => e.Course.CourseName)
				.ToList();

			if (enrollments.Count == 0)
			{
				Console.WriteLine("Ingen elev har påbörjat en klass.");
				return;
			}

			var groupedByStudent = enrollments
				.GroupBy(e => e.Student)
				.OrderBy(g => g.Key.StudentLastName)
				.ThenBy(g => g.Key.StudentFirstName);

			Console.Clear();
			foreach (var group in groupedByStudent)
			{
				Console.WriteLine("------------------------------");
				var student = group.Key;
				Console.WriteLine($"Student: {student.StudentFirstName} {student.StudentLastName} (ID: {student.StudentID})");

				foreach (var e in group)
				{
					var courseName = e.Course?.CourseName ?? "Okänd kurs";
					var courseTeacherName = e.Course?.FkTeacher?.TeacherName ?? "Okänd kurslärare";
					var gradeScale = e.Grade?.GradeScale ?? "Inte betygsatt";
					var gradingTeacherName = e.Grade?.Teacher?.TeacherName ?? "Okänd betygsättande lärare";
					var gradedDateText = e.Grade != null
						? e.Grade.GradedDate.ToString("yyyy-MM-dd")
						: "N/A";

					Console.WriteLine($"  - Kurs: {courseName}");
					Console.WriteLine($"    Kurslärare: {courseTeacherName}");
					Console.WriteLine($"    Betyg: {gradeScale}");
					Console.WriteLine($"    Betygsatt av: {gradingTeacherName}");
					Console.WriteLine($"    Betygsdatum: {gradedDateText}");
				}
				Console.WriteLine("------------------------------");

				Console.WriteLine();
			}

			Console.ReadKey();
		}

		public static void ShowActiveCourses(DataService data)
		{
			var today = DateOnly.FromDateTime(DateTime.Today);

			// Courses active today
			var activeCourses = data.DatabaseFirst.Set<Course>()
				.Where(c => c.CourseStartDate <= today && today <= c.CourseEndDate)
				.OrderBy(c => c.CourseStartDate)
				.ToList();

			// Enrollments for those courses (may be empty if no students)
			var activeEnrollments = data.CodeFirst.Set<Enrollment>()
				.Include(e => e.Student)
				.Include(e => e.Course)
				.Where(e => e.Course != null &&
							e.Course.CourseStartDate <= today &&
							today <= e.Course.CourseEndDate)
				.ToList();

			// Upcoming courses
			var upcomingCourses = data.DatabaseFirst.Set<Course>()
				.Where(c => c.CourseStartDate > today)
				.OrderBy(c => c.CourseStartDate)
				.ToList();

			Console.Clear();

			if (activeCourses.Count == 0)
			{
				Console.WriteLine("Inga aktiva kurser just nu.");
				Console.WriteLine("Nästkommande kurser: ");
				foreach (var course in upcomingCourses)
				{
					Console.WriteLine($"{course.CourseName} Börjar: {course.CourseStartDate:yyyy-MM-dd}");
				}
				
				Console.ReadKey();
				return;
			}

			Console.WriteLine("Aktiva kurser:");
			foreach (var course in activeCourses)
			{
				Console.WriteLine($"- {course.CourseName} ({course.CourseStartDate:yyyy-MM-dd} -> {course.CourseEndDate:yyyy-MM-dd})");

				var studentsInCourse = activeEnrollments
					.Where(e => e.Course!.CourseID == course.CourseID)
					.Select(e => e.Student!)
					.OrderBy(s => s.StudentLastName)
					.ThenBy(s => s.StudentFirstName)
					.ToList();

				if (studentsInCourse.Count == 0)
				{
					Console.WriteLine("  Inga studenter inskrivna.");
				}
				else
				{
					Console.WriteLine("  Inskrivna studenter:");
					foreach (var s in studentsInCourse)
					{
						Console.WriteLine($"    - {s.StudentFirstName} {s.StudentLastName} (ID: {s.StudentID})");
					}
				}
			}

			Console.ReadKey();
		}
	}
}