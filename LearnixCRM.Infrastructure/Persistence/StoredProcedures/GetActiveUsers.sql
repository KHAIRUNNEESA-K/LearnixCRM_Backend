CREATE PROCEDURE [dbo].[sp_GetActiveUsers]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
       *
    FROM Users
    WHERE Status = 3
      AND DeletedAt IS NULL
	  AND UserRole <> 1;
END