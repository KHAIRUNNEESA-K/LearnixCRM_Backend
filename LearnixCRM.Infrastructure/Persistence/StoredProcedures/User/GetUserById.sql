CREATE PROCEDURE [dbo].[sp_GetUserById]
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        UserId,
        FullName,
        Email,
        UserRole,
        Status,
        ProfileImageUrl,
        EmployeeCode,
        DateOfJoining,
        ContactNumber,

        CASE 
            WHEN Status = 4 THEN RejectReason
            ELSE NULL
        END AS RejectReason

    FROM Users
    WHERE UserId = @UserId
      AND UserRole <> 1
      AND DeletedAt IS NULL;
END