CREATE PROCEDURE [dbo].[sp_GetAllUsers]
AS
BEGIN
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
    WHERE DeletedAt IS NULL
      AND UserRole <> 1;
END

