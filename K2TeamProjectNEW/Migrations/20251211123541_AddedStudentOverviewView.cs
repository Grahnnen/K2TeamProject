using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace K2TeamProjectNEW.Migrations
{
	/// <inheritdoc />
	public partial class AddedStudentOverviewView : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(
				""""
                IF OBJECT_ID(N'dbo.vwStudentOverview', N'V') IS NOT NULL
                    DROP VIEW dbo.vwStudentOverview;
                GO
                CREATE VIEW dbo.vwStudentOverview AS
                SELECT
                    s.StudentID,
                    s.StudentFirstName,
                    s.StudentLastName,
                    c.CourseID,
                    c.CourseName,
                    t.TeacherID AS CourseTeacherID,
                    t.TeacherName AS CourseTeacherName,
                    g.GradeID,
                    g.GradeScale,
                    gt.TeacherID AS GradingTeacherID,
                    gt.TeacherName AS GradingTeacherName,
                    g.GradedDate
                FROM dbo.Enrollments e
                LEFT JOIN dbo.Students s ON s.StudentID = e.FkStudentID
                LEFT JOIN dbo.Course c ON c.CourseID = e.FkCourseID
                LEFT JOIN dbo.Teacher t ON t.TeacherID = c.FkTeacherID
                LEFT JOIN dbo.Grades g ON g.GradeID = e.FkGradeID
                LEFT JOIN dbo.Teacher gt ON gt.TeacherID = g.FkTeacherID;
                """");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(
				"""
                IF OBJECT_ID(N'dbo.vwStudentOverview', N'V') IS NOT NULL
                    DROP VIEW dbo.vwStudentOverview;
                """);
		}
	}
}
