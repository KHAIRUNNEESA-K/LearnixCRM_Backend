CREATE PROCEDURE [dbo].[sp_GetApproveUsers]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        *
    FROM Users
    WHERE Status = 2
      AND DeletedAt IS NULL
	  AND UserRole <> 1;
END