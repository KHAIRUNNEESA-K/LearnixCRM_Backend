CREATE PROCEDURE [dbo].[sp_GetRejectedUsers]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        *
    FROM Users
    WHERE Status = 4
      AND DeletedAt IS NULL
	  AND UserRole <> 1;
END