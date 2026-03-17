CREATE PROCEDURE sp_Admin_GetManagerTeamPerformance
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        m.FullName AS ManagerName,
        t.TeamName,

        COUNT(s.StudentId) AS TotalSales,

        CASE 
            WHEN COUNT(l.LeadId) = 0 THEN 0
            ELSE (COUNT(s.StudentId) * 100.0 / COUNT(l.LeadId))
        END AS ConversionRate,

        ISNULL(SUM(c.Fee),0) AS RevenueGenerated

    FROM Teams t

    INNER JOIN Users m
        ON t.ManagerUserId = m.UserId

    LEFT JOIN AssignUsers au
        ON au.TeamId = t.TeamId
        AND au.IsActive = 1

    LEFT JOIN Users sales
        ON sales.UserId = au.SalesUserId

    LEFT JOIN Leads l
        ON l.AssignedToUserId = sales.UserId

    LEFT JOIN Students s
        ON s.LeadId = l.LeadId

    LEFT JOIN Courses c
        ON s.CourseId = c.CourseId

    GROUP BY
        m.FullName,
        t.TeamName

END;