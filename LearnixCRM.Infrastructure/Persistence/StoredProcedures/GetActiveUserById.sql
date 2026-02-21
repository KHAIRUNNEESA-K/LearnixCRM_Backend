CREATE PROCEDURE [dbo].[sp_GetActiveUserById]
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM Users
    WHERE UserId = @UserId
    AND Status = 3
    AND DeletedAt IS NULL
	AND UserRole <> 1;
END