ALTER PROCEDURE sp_GetPerformanceDashboard
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CurrentMonth INT
    DECLARE @PreviousMonth INT
    DECLARE @TotalLeads INT

    SELECT @CurrentMonth = COUNT(*)
    FROM Students
    WHERE MONTH(JoinedDate) = MONTH(GETDATE())
      AND YEAR(JoinedDate) = YEAR(GETDATE())

    SELECT @PreviousMonth = COUNT(*)
    FROM Students
    WHERE MONTH(JoinedDate) = MONTH(DATEADD(MONTH,-1,GETDATE()))
      AND YEAR(JoinedDate) = YEAR(DATEADD(MONTH,-1,GETDATE()))

    SELECT @TotalLeads = COUNT(*)
    FROM Leads

    SELECT
        @CurrentMonth AS TotalAdmissions,

        CASE 
            WHEN @PreviousMonth = 0 THEN 0
            ELSE ((@CurrentMonth - @PreviousMonth) * 100.0 / @PreviousMonth)
        END AS GrowthRate,

        CASE
            WHEN @TotalLeads = 0 THEN 0
            ELSE (@CurrentMonth * 100.0 / @TotalLeads)
        END AS Efficiency
END