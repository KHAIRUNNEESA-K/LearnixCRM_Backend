CREATE PROCEDURE [dbo].[sp_CheckEmailExists]
    @Email NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(1)
    FROM dbo.Users
    WHERE Email = @Email;
END
