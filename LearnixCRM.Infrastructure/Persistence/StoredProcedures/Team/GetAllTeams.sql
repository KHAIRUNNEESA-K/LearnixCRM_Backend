CREATE PROCEDURE sp_GetAllTeams
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        t.TeamId,
        t.TeamName,
        t.ManagerUserId,
        u.FullName AS ManagerName
    FROM Teams t
    JOIN Users u ON t.ManagerUserId = u.UserId
    WHERE t.DeletedAt IS NULL
    ORDER BY t.TeamName;
END