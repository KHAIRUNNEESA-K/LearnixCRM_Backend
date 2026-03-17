CREATE PROCEDURE sp_GetFollowUpByIdForSalesUser
    @FollowUpId INT,
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
    WHERE f.FollowUpId = @FollowUpId
      AND l.AssignedToUserId = @SalesUserId
      AND f.DeletedAt IS NULL
END