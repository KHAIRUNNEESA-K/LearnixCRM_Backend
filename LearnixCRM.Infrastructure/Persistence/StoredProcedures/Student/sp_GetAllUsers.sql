CREATE PROCEDURE sp_GetAllUsers
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
       *
    FROM Students;
END
GO