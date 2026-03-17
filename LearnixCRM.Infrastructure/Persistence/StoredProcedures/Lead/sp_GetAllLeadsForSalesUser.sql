Alter PROCEDURE sp_GetAllLeadsForSalesUser
    @AssignedToUserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        L.LeadId,
        L.FullName,
        L.Email,
        L.Phone,
        L.CourseId,
        C.Name as CourseName,   -- return course name
        L.Source,
        L.Status,
        L.AssignedToUserId,
        L.Remark,
        L.CreatedAt,
        L.CreatedBy,
        L.UpdatedAt,
        L.UpdatedBy,
        L.DeletedAt,
        L.DeletedBy
    FROM Leads L
    LEFT JOIN Courses C 
        ON L.CourseId = C.CourseId   -- join with course table
    WHERE L.AssignedToUserId = @AssignedToUserId
      AND L.DeletedAt IS NULL
    ORDER BY L.CreatedAt DESC;

END
