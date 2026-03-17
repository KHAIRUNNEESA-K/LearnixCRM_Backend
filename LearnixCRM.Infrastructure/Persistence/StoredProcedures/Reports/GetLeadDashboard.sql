CREATE PROCEDURE sp_GetLeadDashboard
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CurrentMonth INT
    DECLARE @PreviousMonth INT

    SELECT @CurrentMonth = COUNT(*)
    FROM Leads
    WHERE MONTH(CreatedAt) = MONTH(GETDATE())
      AND YEAR(CreatedAt) = YEAR(GETDATE())

    SELECT @PreviousMonth = COUNT(*)
    FROM Leads
    WHERE MONTH(CreatedAt) = MONTH(DATEADD(MONTH,-1,GETDATE()))
      AND YEAR(CreatedAt) = YEAR(DATEADD(MONTH,-1,GETDATE()))

    SELECT
        @CurrentMonth AS TotalLeads,
        SUM(CASE WHEN Status = 3 THEN 1 ELSE 0 END) AS Converted,
        SUM(CASE WHEN Status <> 3 THEN 1 ELSE 0 END) AS Pending,
        SUM(c.Fee) AS Revenue,
        CASE 
            WHEN @PreviousMonth = 0 THEN 0
            ELSE ((@CurrentMonth - @PreviousMonth) * 100.0 / @PreviousMonth)
        END AS GrowthRate,
        CASE
            WHEN @CurrentMonth = 0 THEN 0
            ELSE (SUM(CASE WHEN Status = 3 THEN 1 ELSE 0 END) * 100.0 / @CurrentMonth)
        END AS Efficiency
    FROM Leads l
    LEFT JOIN Courses c ON l.CourseId = c.CourseId
    WHERE MONTH(l.CreatedAt) = MONTH(GETDATE())
      AND YEAR(l.CreatedAt) = YEAR(GETDATE())
END