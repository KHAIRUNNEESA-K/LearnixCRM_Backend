CREATE PROCEDURE [dbo].[sp_GetPendingUsers]
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
    WHERE Status = 'Pending'
      AND DeletedAt IS NULL;
END