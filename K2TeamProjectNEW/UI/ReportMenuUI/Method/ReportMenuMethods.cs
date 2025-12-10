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
	}
}
