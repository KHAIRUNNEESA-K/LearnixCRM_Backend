CREATE PROCEDURE sp_GetAllstudents
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
       *
    FROM Students;
END
GO

