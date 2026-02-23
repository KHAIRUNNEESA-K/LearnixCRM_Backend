CREATE PROCEDURE sp_GetLeadByIdForSalesUser
    @LeadId INT,
    @AssignedToUserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        LeadId,
        FullName,
        Email,
        Phone,
        CourseInterested,
        Source,
        Status,
        AssignedToUserId,
        Remark,
        CreatedAt,
        CreatedBy,
        UpdatedAt,
        UpdatedBy
    FROM Leads
    WHERE LeadId = @LeadId
      AND AssignedToUserId = @AssignedToUserId
      AND DeletedAt IS NULL;

END
