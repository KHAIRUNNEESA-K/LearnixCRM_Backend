CREATE PROCEDURE sp_GetAllLeadsForSalesUser
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
        UpdatedBy,
        DeletedAt,
        DeletedBy
    FROM Leads
    WHERE AssignedToUserId = @AssignedToUserId
      AND DeletedAt IS NULL
    ORDER BY CreatedAt DESC;

END