CREATE PROCEDURE sp_GetAllAdmissions
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.StudentId,
        s.FullName AS StudentName,
        s.Email,
        s.Phone,
        c.Name AS CourseName,
        c.Fee,
        s.JoinedDate,
        u.FullName AS SalesName
    FROM Students s
    INNER JOIN Leads l 
        ON s.LeadId = l.LeadId
    INNER JOIN Users u 
        ON l.AssignedToUserId = u.UserId
    INNER JOIN Courses c 
        ON l.CourseId = c.CourseId
    WHERE s.DeletedAt IS NULL
    ORDER BY s.JoinedDate DESC
END