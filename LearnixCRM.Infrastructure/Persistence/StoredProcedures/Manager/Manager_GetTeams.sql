CREATE PROCEDURE sp_Manager_GetTeams
    @ManagerUserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        TeamId,
        TeamName
    FROM Teams
    WHERE ManagerUserId = @ManagerUserId
      AND IsActive = 1
      AND DeletedAt IS NULL
      AND s.Status = 3
END