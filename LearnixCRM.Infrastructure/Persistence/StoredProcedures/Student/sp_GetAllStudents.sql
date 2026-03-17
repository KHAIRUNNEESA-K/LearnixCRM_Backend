CREATE PROCEDURE sp_GetAllStudents
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
       *
    FROM Students;
END
GO