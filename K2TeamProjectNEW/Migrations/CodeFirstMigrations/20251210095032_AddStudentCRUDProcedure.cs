using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace K2TeamProjectNEW.Migrations.K2TeamProjectCodeFirstDb
{
    /// <inheritdoc />
    public partial class AddStudentCRUDProcedure : Migration
    {
        // SQL to create the Stored Procedure
        private const string CreateStudentCrudSP = @"
            CREATE PROCEDURE CRUD_Student
                @StudentID int,
                @StudentFirstName nvarchar(150),
                @StudentLastName nvarchar(150),
                @OperationType int  -- 1: Insert, 2: Update, 3: Delete
            AS
            BEGIN
                SET NOCOUNT ON;

                -- === 1. INSERT Operation ===
                IF @OperationType = 1 AND @StudentID = 0
                BEGIN
                    INSERT INTO Students (StudentFirstName, StudentLastName)
                    VALUES (@StudentFirstName, @StudentLastName);
            
                    SELECT SCOPE_IDENTITY() AS NewStudentID; -- Return the ID of the new student
                    RETURN;
                END

                -- === 2. UPDATE Operation ===
                IF @OperationType = 2
                BEGIN
                    UPDATE Students
                    SET 
                        StudentFirstName = @StudentFirstName,
                        StudentLastName = @StudentLastName
                    WHERE StudentID = @StudentID;
                    
                    IF @@ROWCOUNT = 0
                    BEGIN
                        RAISERROR('Student ID not found for update.', 16, 1);
                        RETURN;
                    END
                    RETURN;
                END

                -- === 3. DELETE Operation ===
                IF @OperationType = 3
                BEGIN
                    DELETE FROM Students
                    WHERE StudentID = @StudentID;

                    IF @@ROWCOUNT = 0
                    BEGIN
                        RAISERROR('Student ID not found for deletion.', 16, 1);
                        RETURN;
                    END
                    RETURN;
                END

                -- === Invalid Operation Type ===
                IF @OperationType NOT IN (1, 2, 3)
                BEGIN
                    RAISERROR('Invalid OperationType specified. Use 1 for Insert, 2 for Update, or 3 for Delete.', 16, 1);
                END
            END";

        // SQL to drop the Stored Procedure
        private const string DropStudentCrudSP = "DROP PROCEDURE CRUD_Student";


        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(CreateStudentCrudSP);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(DropStudentCrudSP);
        }
    }
}