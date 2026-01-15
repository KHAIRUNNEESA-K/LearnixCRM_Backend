CREATE PROCEDURE [dbo].[sp_GetUserForLogin]
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
        Status
    FROM Users
    WHERE Email = LOWER(@Email)
      AND DeletedAt IS NULL;
END