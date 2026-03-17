CREATE PROCEDURE sp_GetUsersByStatusAndPasswordState
    @Status INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
      *
    FROM Users
    WHERE Status = @Status
      AND PasswordHash IS NULL
      AND DeletedAt IS NULL
END
