namespace K2TeamProjectNEW.Models.Views
{
	public class StudentOverview
	{
		public int StudentID { get; set; }
		public string? StudentFirstName { get; set; }
		public string? StudentLastName { get; set; }

		public int? CourseID { get; set; }
		public string? CourseName { get; set; }
		public int? CourseTeacherID { get; set; }
		public string? CourseTeacherName { get; set; }

		public int? GradeID { get; set; }
		public string? GradeScale { get; set; }
		public int? GradingTeacherID { get; set; }
		public string? GradingTeacherName { get; set; }
		public DateOnly? GradedDate { get; set; }
	}
}