CREATE PROCEDURE sp_Manager_GetTeamMembers
    @TeamId INT,
    @ManagerUserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.UserId AS SalesUserId,
        s.FullName AS SalesUserName,
        s.Email
    FROM AssignUsers a
    INNER JOIN Users s ON a.SalesUserId = s.UserId
    INNER JOIN Teams t ON a.TeamId = t.TeamId
    WHERE a.TeamId = @TeamId
      AND t.ManagerUserId = @ManagerUserId
      AND a.IsActive = 1
      AND a.DeletedAt IS NULL

END