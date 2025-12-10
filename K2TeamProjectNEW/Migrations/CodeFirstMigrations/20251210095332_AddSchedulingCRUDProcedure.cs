using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace K2TeamProjectNEW.Migrations.K2TeamProjectCodeFirstDb
{
    /// <inheritdoc />
    public partial class AddSchedulingCRUDProcedure : Migration
    {
        // SQL to create the Stored Procedure
        private const string CreateSchedulingCrudSP = @"
            CREATE PROCEDURE CRUD_Scheduling
                @SchedulingID int,
                @SchedulingStartDateTime datetime2,
                @SchedulingEndDateTime datetime2,
                @FkCourseID int,
                @FkClassroomID int,
                @OperationType int
            AS
            BEGIN
                SET NOCOUNT ON;

                -- === 1. INSERT Operation ===
                IF @OperationType = 1 AND @SchedulingID = 0
                BEGIN
                    INSERT INTO Schedulings (SchedulingStartDateTime, SchedulingEndDateTime, FkCourseID, FkClassroomID)
                    VALUES (@SchedulingStartDateTime, @SchedulingEndDateTime, @FkCourseID, @FkClassroomID);

                    SELECT SCOPE_IDENTITY() AS NewSchedulingID;
                    RETURN;
                END

                -- === 2. UPDATE Operation ===
                IF @OperationType = 2
                BEGIN
                    UPDATE Schedulings
                    SET 
                        SchedulingStartDateTime = @SchedulingStartDateTime,
                        SchedulingEndDateTime = @SchedulingEndDateTime,
                        FkCourseID = @FkCourseID,
                        FkClassroomID = @FkClassroomID
                    WHERE SchedulingID = @SchedulingID;
                    
                    IF @@ROWCOUNT = 0
                    BEGIN
                        RAISERROR('Scheduling ID not found for update.', 16, 1);
                        RETURN;
                    END
                    RETURN;
                END

                -- === 3. DELETE Operation ===
                IF @OperationType = 3
                BEGIN
                    DELETE FROM Schedulings
                    WHERE SchedulingID = @SchedulingID;

                    IF @@ROWCOUNT = 0
                    BEGIN
                        RAISERROR('Scheduling ID not found for deletion.', 16, 1);
                        RETURN;
                    END
                    RETURN;
                END

                -- === Invalid Operation Type ===
                IF @OperationType NOT IN (1, 2, 3)
                BEGIN
                    RAISERROR('Invalid OperationType specified.', 16, 1);
                END
            END";

        // SQL to drop the Stored Procedure
        private const string DropSchedulingCrudSP = "DROP PROCEDURE CRUD_Scheduling";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(CreateSchedulingCrudSP);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(DropSchedulingCrudSP);
        }
    }
}