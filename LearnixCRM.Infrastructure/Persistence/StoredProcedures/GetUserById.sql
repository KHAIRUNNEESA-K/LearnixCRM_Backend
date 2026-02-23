CREATE PROCEDURE [dbo].[sp_GetUserById]
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
       *
    FROM Users
    WHERE UserId = @UserId
      AND UserRole <> 1
      AND DeletedAt IS NULL;
END
