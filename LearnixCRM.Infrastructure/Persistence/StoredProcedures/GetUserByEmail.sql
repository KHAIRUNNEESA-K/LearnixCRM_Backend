CREATE PROCEDURE [dbo].[sp_GetUserByEmail]
    @Email NVARCHAR(200)
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
        UpdatedBy,
        DeletedAt,
        DeletedBy
    FROM Users
    WHERE LOWER(Email) = LOWER(@Email)
      AND DeletedAt IS NULL
END
