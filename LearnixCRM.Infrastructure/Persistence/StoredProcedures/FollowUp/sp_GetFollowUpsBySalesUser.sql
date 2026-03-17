CREATE PROCEDURE sp_GetFollowUpsBySalesUser
    @SalesUserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT f.FollowUpId,
           f.LeadId,
           f.FollowUpDate,
           f.Remark
    FROM FollowUps f
    INNER JOIN Leads l ON f.LeadId = l.LeadId
    WHERE l.AssignedToUserId = @SalesUserId
      AND f.DeletedAt IS NULL
END