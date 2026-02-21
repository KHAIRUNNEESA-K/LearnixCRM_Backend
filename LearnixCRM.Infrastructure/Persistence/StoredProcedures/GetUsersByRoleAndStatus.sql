CREATE PROCEDURE sp_GetUsersByRoleAndStatus
    @Role NVARCHAR(50),
    @Status NVARCHAR(50)
AS
BEGIN
    SELECT *
    FROM Users
    WHERE UserRole = @Role
      AND Status = @Status
END
