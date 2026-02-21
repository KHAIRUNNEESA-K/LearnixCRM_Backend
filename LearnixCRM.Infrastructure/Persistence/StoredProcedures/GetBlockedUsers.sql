CREATE PROCEDURE [dbo].[sp_GetBlockedUsers]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        UserId,
        FullName,
        Email,
        PasswordHash,
        UserRole,
        Status,
        CreatedAt,
        CreatedBy,
        UpdatedAt,
        UpdatedBy
    FROM Users
    WHERE Status = 5
      AND DeletedAt IS NULL
	  AND UserRole <> 1;
END