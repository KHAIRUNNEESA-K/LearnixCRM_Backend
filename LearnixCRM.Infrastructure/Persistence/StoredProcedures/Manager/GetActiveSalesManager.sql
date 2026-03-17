CREATE PROCEDURE [dbo].[sp_GetActiveSalesManager]
    @SalesUserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        a.AssignId,
        a.TeamId,
        a.SalesUserId,
        t.ManagerUserId,
        a.IsActive,
        a.CreatedAt,
        a.CreatedBy,
        a.UpdatedAt,
        a.UpdatedBy,
        a.DeletedAt,
        a.DeletedBy
    FROM AssignUsers a
    INNER JOIN Teams t ON a.TeamId = t.TeamId
    INNER JOIN Users s ON a.SalesUserId = s.UserId
    INNER JOIN Users m ON t.ManagerUserId = m.UserId
    WHERE a.SalesUserId = @SalesUserId
      AND a.IsActive = 1
      AND a.DeletedAt IS NULL
      AND s.Status = 3
      AND m.Status = 3
    ORDER BY a.AssignId DESC;
END