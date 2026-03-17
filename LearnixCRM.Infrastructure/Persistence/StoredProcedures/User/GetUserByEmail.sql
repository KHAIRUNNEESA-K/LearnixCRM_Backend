CREATE PROCEDURE [dbo].[sp_GetUserByEmail]
    @Email NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
       *
    FROM Users
    WHERE LOWER(Email) = LOWER(@Email)
      AND DeletedAt IS NULL
END