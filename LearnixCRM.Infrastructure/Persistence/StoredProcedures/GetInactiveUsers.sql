CREATE PROCEDURE [dbo].[sp_GetInactiveUsers]
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
        ContactNumber,
        CreatedAt,
        CreatedBy,
        UpdatedAt,
        UpdatedBy
    FROM Users
    WHERE Status = 0
      AND DeletedAt IS NULL
	  AND UserRole <> 1;
END
