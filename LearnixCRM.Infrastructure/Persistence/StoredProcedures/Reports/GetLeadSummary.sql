CREATE PROCEDURE sp_GetLeadSummary
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        FORMAT(l.CreatedAt,'yyyy-MM') AS Month,
        COUNT(l.LeadId) AS TotalLeads,
        SUM(CASE WHEN l.Status = 3 THEN 1 ELSE 0 END) AS Converted,
        SUM(CASE WHEN l.Status <> 3 THEN 1 ELSE 0 END) AS Pending,
        SUM(c.Fee) AS Revenue
    FROM Leads l
    LEFT JOIN Courses c ON l.CourseId = c.CourseId
    GROUP BY FORMAT(l.CreatedAt,'yyyy-MM')
    ORDER BY Month DESC
END