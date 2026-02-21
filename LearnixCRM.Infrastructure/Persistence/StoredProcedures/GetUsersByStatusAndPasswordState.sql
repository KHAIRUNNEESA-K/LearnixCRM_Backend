CREATE PROCEDURE sp_GetUsersByStatusAndPasswordState
    @Status INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        UserId,
        FullName,
        Email,
        UserRole,
        Status
    FROM Users
    WHERE Status = @Status
      AND PasswordHash IS NULL
      AND DeletedAt IS NULL
END
