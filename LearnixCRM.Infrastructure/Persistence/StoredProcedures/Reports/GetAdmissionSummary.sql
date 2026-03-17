CREATE PROCEDURE sp_GetAdmissionSummary
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.Name AS Course,
        COUNT(s.StudentId) AS StudentCount,
        SUM(c.Fee) AS Revenue,
        CASE 
            WHEN COUNT(s.StudentId) > 10 THEN 'Up'
            WHEN COUNT(s.StudentId) BETWEEN 5 AND 10 THEN 'Stable'
            ELSE 'Down'
        END AS Trend
    FROM Students s
    INNER JOIN Leads l ON s.LeadId = l.LeadId
    INNER JOIN Courses c ON l.CourseId = c.CourseId
    GROUP BY c.Name
    ORDER BY StudentCount DESC
END