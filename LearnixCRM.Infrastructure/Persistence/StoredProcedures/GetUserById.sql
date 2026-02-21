CREATE PROCEDURE [dbo].[sp_GetUserById]
    @UserId INT
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
    WHERE UserId = @UserId
      AND UserRole <> 1
      AND DeletedAt IS NULL;
END
