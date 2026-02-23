CREATE PROCEDURE sp_GetStudentById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
     *
    FROM Students
    WHERE StudentId = @Id;
END
GO