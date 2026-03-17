ALTER PROCEDURE sp_GetPerformanceSummary
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        u.FullName AS Name,

        CASE 
            WHEN u.UserRole = 2 THEN 'Manager'
            WHEN u.UserRole = 3 THEN 'Sales'
        END AS Role,

        COUNT(s.StudentId) AS SalesTeamValue,

        CASE
            WHEN COUNT(s.StudentId) >= 10 THEN 'High Achiever'
            WHEN COUNT(s.StudentId) BETWEEN 5 AND 9 THEN 'Achiever'
            WHEN COUNT(s.StudentId) BETWEEN 1 AND 4 THEN 'Average'
            ELSE 'Needs Improvement'
        END AS Achievement,

        CASE
            WHEN COUNT(s.StudentId) >= 5 THEN 'Active'
            WHEN COUNT(s.StudentId) BETWEEN 1 AND 4 THEN 'Moderate'
            ELSE 'Low Performance'
        END AS Status

    FROM Users u

    LEFT JOIN Leads l
        ON l.AssignedToUserId = u.UserId

    LEFT JOIN Students s
        ON s.LeadId = l.LeadId

    WHERE u.UserRole IN (2,3)

    GROUP BY
        u.FullName,
        u.UserRole

    ORDER BY SalesTeamValue DESC
END