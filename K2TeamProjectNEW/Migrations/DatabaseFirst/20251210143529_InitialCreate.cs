using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace K2TeamProjectNEW.Migrations.DatabaseFirst
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teacher",
                columns: table => new
                {
                    TeacherID = table.Column<int>(type: "int", nullable: false),
                    TeacherName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Teacher__EDF259440181DB12", x => x.TeacherID);
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    CourseID = table.Column<int>(type: "int", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FkTeacherID = table.Column<int>(type: "int", nullable: true),
                    CourseStartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CourseEndDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Course__C92D71879A78CCFE", x => x.CourseID);
                    table.ForeignKey(
                        name: "FK__Course__Fk_Teach_398D8EEE",
                        column: x => x.FkTeacherID,
                        principalTable: "Teacher",
                        principalColumn: "TeacherID");
                });

            migrationBuilder.InsertData(
                table: "Teacher",
                columns: new[] { "TeacherID", "TeacherName" },
                values: new object[,]
                {
                    { 1, "Markus Silva" },
                    { 2, "Jenny Larsson" },
                    { 3, "Henrik Andersson" }
                });

            migrationBuilder.InsertData(
                table: "Course",
                columns: new[] { "CourseID", "CourseEndDate", "CourseName", "CourseStartDate", "FkTeacherID" },
                values: new object[,]
                {
                    { 1, new DateOnly(2025, 12, 19), "Engelska 1", new DateOnly(2025, 12, 18), 1 },
                    { 2, new DateOnly(2025, 12, 21), "Engelska 2", new DateOnly(2025, 12, 20), 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Course_FkTeacherID",
                table: "Course",
                column: "FkTeacherID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "Teacher");
        }
    }
}
