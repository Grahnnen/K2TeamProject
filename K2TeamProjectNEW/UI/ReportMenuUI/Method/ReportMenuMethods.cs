using K2TeamProjectNEW.Data;
using K2TeamProjectNEW.Models;
using K2TeamProjectNEW.Models.Views;
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
			var overviewRows = data.CodeFirst.Set<StudentOverview>()
				.AsNoTracking()
				.OrderBy(v => v.StudentLastName)
				.ThenBy(v => v.StudentFirstName)
				.ThenBy(v => v.CourseName)
				.ToList();

			if (overviewRows.Count == 0)
			{
				Console.WriteLine("Ingen elev har påbörjat en klass.");
				return;
			}

			var groupedByStudent = overviewRows
				.GroupBy(v => new { v.StudentID, v.StudentFirstName, v.StudentLastName })
				.OrderBy(g => g.Key.StudentLastName)
				.ThenBy(g => g.Key.StudentFirstName);


			Console.Clear();
			Console.WriteLine("------------------------------");
			foreach (var group in groupedByStudent)
			{
				Console.WriteLine($"Student: {group.Key.StudentFirstName} {group.Key.StudentLastName} (ID: {group.Key.StudentID})");

				foreach (var v in group)
				{
					var courseName = v.CourseName ?? "Okänd kurs";
					var courseTeacherName = v.CourseTeacherName ?? "Okänd kurslärare";
					var gradeScale = v.GradeScale ?? "Inte betygsatt";
					var gradingTeacherName = v.GradingTeacherName ?? "Okänd betygsättande lärare";
					var gradedDateText = v.GradedDate.HasValue
						? v.GradedDate.Value.ToString("yyyy-MM-dd")
						: "N/A";

					Console.WriteLine($"  - Kurs: {courseName}");
					Console.WriteLine($"    Kurslärare: {courseTeacherName}");
					Console.WriteLine($"    Betyg: {gradeScale}");
					Console.WriteLine($"    Betygsatt av: {gradingTeacherName}");
					Console.WriteLine($"    Betygsdatum: {gradedDateText}");
				}
				Console.WriteLine("------------------------------");
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

		public static void ShowGradeStatistics(DataService data)
		{
			var approvedSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
			{
				"A","B","C","D","E"
			};
			var notApprovedSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
			{
				"F"
			};
			var pendingSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
			{
				"Pending"
			};

			Console.Clear();
			Console.WriteLine("Rapport: Godkända och icke godkända per period");
			Console.WriteLine("Välj periodtyp:");
			Console.WriteLine("1) Helår");
			Console.WriteLine("2) Halvår (H1/H2)");
			Console.WriteLine("3) Kvartal (Q1–Q4)");
			Console.Write("Ditt val: ");

			var choice = Console.ReadLine();
			if (string.IsNullOrWhiteSpace(choice))
			{
				Console.WriteLine("Ogiltigt val.");
				Console.ReadKey();
				return;
			}

			Console.Write("Ange år (t.ex. 2025): ");
			var yearText = Console.ReadLine();
			if (!int.TryParse(yearText, out var year))
			{
				Console.WriteLine("Ogiltigt år.");
				Console.ReadKey();
				return;
			}

			int? half = null;
			int? quarter = null;

			switch (choice)
			{
				case "1":
					break;
				case "2":
					Console.Write("Ange halvår (1 eller 2): ");
					var halfText = Console.ReadLine();
					if (!int.TryParse(halfText, out var h) || (h != 1 && h != 2))
					{
						Console.WriteLine("Ogiltigt halvår.");
						Console.ReadKey();
						return;
					}
					half = h;
					break;
				case "3":
					Console.Write("Ange kvartal (1–4): ");
					var qText = Console.ReadLine();
					if (!int.TryParse(qText, out var q) || q < 1 || q > 4)
					{
						Console.WriteLine("Ogiltigt kvartal.");
						Console.ReadKey();
						return;
					}
					quarter = q;
					break;
				default:
					Console.WriteLine("Ogiltigt val.");
					Console.ReadKey();
					return;
			}

			//Get quarter, halfyear or fullyear
			var (start, end, periodLabel) = GetPeriodRange(year, half, quarter);

			var grades = data.CodeFirst.Set<Enrollment>()
				.Include(e => e.Grade)
				.Include(e => e.Student)
				.Where(e => e.Grade != null &&
							e.Grade.GradedDate >= start &&
							e.Grade.GradedDate <= end)
				.Select(e => e.Grade)
				.ToList();

			int approvedCount = 0;
			int notApprovedCount = 0;
			int pendingCount = 0;

			foreach (var g in grades)
			{
				var scale = g.GradeScale?.Trim();

				// Pending: null/empty
				if (string.IsNullOrWhiteSpace(scale) || pendingSet.Contains(scale))
				{
					pendingCount++;
					continue;
				}
				
				// Handle plus/minus variants like A+, A-
				var first = char.ToUpperInvariant(scale[0]);

				// Approved: A–E (any +/-)
				if ((first >= 'A' && first <= 'E') || approvedSet.Contains(scale))
				{
					approvedCount++;
					continue;
				}

				// Not approved: F (any +/-)
				if (first == 'F' || notApprovedSet.Contains(scale))
				{
					notApprovedCount++;
					continue;
				}
				//Unnkown grade
				notApprovedCount++;
			}

			Console.Clear();
			Console.WriteLine($"Period: {periodLabel} ({start:yyyy-MM-dd}–{end:yyyy-MM-dd})");
			Console.WriteLine($"Antal godkända: {approvedCount}");
			Console.WriteLine($"Antal icke godkända: {notApprovedCount}");
			Console.WriteLine($"Antal väntar betyg (pending): {pendingCount}");
			Console.WriteLine();

			var scaleBreakdown = grades
				.GroupBy(g =>
				{
					var s = g.GradeScale?.Trim();
					if (string.IsNullOrWhiteSpace(s)) return "Pending";
					if (pendingSet.Contains(s)) return "Pending";
					return s;
				})
				.OrderBy(g => g.Key)
				.Select(g => new { Scale = g.Key, Count = g.Count() })
				.ToList();

			Console.WriteLine("Fördelning per betygsskala:");
			foreach (var item in scaleBreakdown)
			{
				Console.WriteLine($"- {item.Scale}: {item.Count}");
			}

			Console.WriteLine();
			Console.WriteLine("Tryck valfri tangent för att återgå.");
			Console.ReadKey();
		}

		//Ai magic returns quarter or halfyear
		private static (DateOnly start, DateOnly end, string label) GetPeriodRange(int year, int? half, int? quarter)
		{
			if (quarter.HasValue)
			{
				var startMonth = (quarter.Value - 1) * 3 + 1;
				var endMonth = startMonth + 2;

				var start = new DateOnly(year, startMonth, 1);
				var end = new DateOnly(year, endMonth, DateTime.DaysInMonth(year, endMonth));
				return (start, end, $"Kvartal Q{quarter} {year}");
			}

			if (half.HasValue)
			{
				if (half.Value == 1)
				{
					var start = new DateOnly(year, 1, 1);
					var end = new DateOnly(year, 6, DateTime.DaysInMonth(year, 6));
					return (start, end, $"Halvår H1 {year}");
				}
				else
				{
					var start = new DateOnly(year, 7, 1);
					var end = new DateOnly(year, 12, DateTime.DaysInMonth(year, 12));
					return (start, end, $"Halvår H2 {year}");
				}
			}

			// Full year
			var yStart = new DateOnly(year, 1, 1);
			var yEnd = new DateOnly(year, 12, 31);
			return (yStart, yEnd, $"Helår {year}");
		}
	}
}