CREATE PROCEDURE [dbo].[sp_GetRejectedUsers]
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
    WHERE Status = 4
      AND DeletedAt IS NULL
	  AND UserRole <> 1;
END