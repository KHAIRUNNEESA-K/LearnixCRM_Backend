CREATE PROCEDURE [dbo].[sp_GetPendingUsers]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        *
    FROM Users
    WHERE Status = 1
      AND DeletedAt IS NULL
	  AND UserRole <> 1;
END