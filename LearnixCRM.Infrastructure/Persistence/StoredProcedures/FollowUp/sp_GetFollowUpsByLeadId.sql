CREATE PROCEDURE sp_GetFollowUpsByLeadId
    @LeadId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT FollowUpId,
           LeadId,
           FollowUpDate,
           Remark
    FROM FollowUps
    WHERE LeadId = @LeadId
      AND DeletedAt IS NULL
END