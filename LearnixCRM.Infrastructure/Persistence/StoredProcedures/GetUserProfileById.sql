CREATE PROCEDURE sp_GetUserProfileById
    @UserId INT
AS
BEGIN
    SELECT 
        UserId,
        FullName,
        Email,
        ContactNumber,
        ProfileImageUrl,
        EmployeeCode,
        DateOfJoining,
        UserRole,
        Status
    FROM Users
    WHERE UserId = @UserId
      AND DeletedAt IS NULL
END
