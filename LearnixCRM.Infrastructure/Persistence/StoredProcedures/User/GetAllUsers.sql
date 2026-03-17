CREATE PROCEDURE [dbo].[sp_GetAllUsers]
AS
BEGIN
    SELECT 
    *
    FROM Users
    WHERE DeletedAt IS NULL
      AND UserRole <> 1;
END

