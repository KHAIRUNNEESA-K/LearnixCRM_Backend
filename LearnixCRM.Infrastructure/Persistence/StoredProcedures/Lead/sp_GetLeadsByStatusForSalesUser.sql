CREATE PROCEDURE sp_GetLeadsByStatusForSalesUser
    @AssignedToUserId INT,
    @Status INT
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
    WHERE AssignedToUserId = @AssignedToUserId
      AND Status = @Status
      AND DeletedAt IS NULL
    ORDER BY CreatedAt DESC;

END

