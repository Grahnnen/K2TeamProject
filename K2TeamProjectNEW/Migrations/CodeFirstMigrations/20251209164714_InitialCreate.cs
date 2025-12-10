using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace K2TeamProjectNEW.Migrations.K2TeamProjectCodeFirstDb
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Classrooms",
                columns: table => new
                {
                    ClassroomID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassroomName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classrooms", x => x.ClassroomID);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentFirstName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    StudentLastName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentID);
                });

            migrationBuilder.CreateTable(
                name: "Schedulings",
                columns: table => new
                {
                    SchedulingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchedulingStartDateTime = table.Column<DateTime>(type: "datetime2", maxLength: 150, nullable: false),
                    SchedulingEndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FkCourseID = table.Column<int>(type: "int", nullable: false),
                    FkClassroomID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedulings", x => x.SchedulingID);
                    table.ForeignKey(
                        name: "FK_Schedulings_Classrooms_FkClassroomID",
                        column: x => x.FkClassroomID,
                        principalTable: "Classrooms",
                        principalColumn: "ClassroomID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schedulings_Course_FkCourseID",
                        column: x => x.FkCourseID,
                        principalTable: "Course",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    GradeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradedDate = table.Column<DateOnly>(type: "date", maxLength: 150, nullable: false),
                    GradeScale = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FkStudentID = table.Column<int>(type: "int", nullable: false),
                    FkTeacherID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.GradeID);
                    table.ForeignKey(
                        name: "FK_Grades_Students_FkStudentID",
                        column: x => x.FkStudentID,
                        principalTable: "Students",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Grades_Teacher_FkTeacherID",
                        column: x => x.FkTeacherID,
                        principalTable: "Teacher",
                        principalColumn: "TeacherID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    EnrollmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationDate = table.Column<DateOnly>(type: "date", maxLength: 150, nullable: false),
                    FkStudentID = table.Column<int>(type: "int", nullable: false),
                    FkCourseID = table.Column<int>(type: "int", nullable: false),
                    FkGradeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => x.EnrollmentID);
                    table.ForeignKey(
                        name: "FK_Enrollments_Course_FkCourseID",
                        column: x => x.FkCourseID,
                        principalTable: "Course",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollments_Grades_FkGradeID",
                        column: x => x.FkGradeID,
                        principalTable: "Grades",
                        principalColumn: "GradeID");
                    table.ForeignKey(
                        name: "FK_Enrollments_Students_FkStudentID",
                        column: x => x.FkStudentID,
                        principalTable: "Students",
                        principalColumn: "StudentID");
                });

            migrationBuilder.InsertData(
                table: "Classrooms",
                columns: new[] { "ClassroomID", "ClassroomName" },
                values: new object[,]
                {
                    { 1, "A101" },
                    { 2, "B205" },
                    { 3, "C303" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "StudentID", "StudentFirstName", "StudentLastName" },
                values: new object[,]
                {
                    { 1, "Alice", "Johnson" },
                    { 2, "Bob", "Williams" },
                    { 3, "Charlie", "Brown" },
                    { 4, "Diana", "Smith" }
                });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "GradeID", "FkStudentID", "FkTeacherID", "GradeScale", "GradedDate" },
                values: new object[,]
                {
                    { 1, 1, 1, "A", new DateOnly(2025, 12, 18) },
                    { 2, 1, 2, "B+", new DateOnly(2025, 12, 18) },
                    { 3, 2, 2, "A-", new DateOnly(2025, 12, 18) },
                    { 4, 2, 3, "C", new DateOnly(2025, 12, 18) },
                    { 5, 3, 1, "B", new DateOnly(2025, 12, 18) },
                    { 6, 1, 1, "Pending", new DateOnly(2025, 9, 1) }
                });

            migrationBuilder.InsertData(
                table: "Schedulings",
                columns: new[] { "SchedulingID", "FkClassroomID", "FkCourseID", "SchedulingEndDateTime", "SchedulingStartDateTime" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2025, 9, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 8, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2, 2, new DateTime(2025, 9, 8, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 8, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 3, 2, new DateTime(2025, 9, 8, 11, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 8, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 1, 1, new DateTime(2025, 9, 10, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 10, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 1, 2, new DateTime(2026, 1, 14, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 14, 9, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Enrollments",
                columns: new[] { "EnrollmentID", "FkCourseID", "FkGradeID", "FkStudentID", "RegistrationDate" },
                values: new object[,]
                {
                    { 1, 1, 1, 1, new DateOnly(2025, 9, 1) },
                    { 2, 2, 2, 1, new DateOnly(2025, 9, 1) },
                    { 3, 2, 3, 2, new DateOnly(2025, 9, 1) },
                    { 4, 2, 4, 2, new DateOnly(2025, 9, 1) },
                    { 5, 1, 5, 3, new DateOnly(2025, 9, 1) },
                    { 6, 1, 6, 4, new DateOnly(2025, 9, 1) },
                    { 7, 2, 6, 4, new DateOnly(2026, 1, 7) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_FkCourseID",
                table: "Enrollments",
                column: "FkCourseID");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_FkGradeID",
                table: "Enrollments",
                column: "FkGradeID");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_FkStudentID",
                table: "Enrollments",
                column: "FkStudentID");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_FkStudentID",
                table: "Grades",
                column: "FkStudentID");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_FkTeacherID",
                table: "Grades",
                column: "FkTeacherID");

            migrationBuilder.CreateIndex(
                name: "IX_Schedulings_FkClassroomID",
                table: "Schedulings",
                column: "FkClassroomID");

            migrationBuilder.CreateIndex(
                name: "IX_Schedulings_FkCourseID",
                table: "Schedulings",
                column: "FkCourseID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "Schedulings");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "Classrooms");

            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
