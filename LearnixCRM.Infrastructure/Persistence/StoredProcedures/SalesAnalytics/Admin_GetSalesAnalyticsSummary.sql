CREATE PROCEDURE sp_Admin_GetSalesAnalyticsSummary
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TotalRevenue DECIMAL(18,2)
    DECLARE @TotalSales INT
    DECLARE @TotalLeads INT
    DECLARE @ConvertedLeads INT
    DECLARE @ConversionRate DECIMAL(10,2)
    DECLARE @ActiveTeams INT

    SELECT @TotalRevenue = ISNULL(SUM(c.Fee),0)
    FROM Students a
    JOIN Courses c ON a.CourseId = c.CourseId

    SELECT @TotalSales = COUNT(*) 
    FROM Students

    SELECT @TotalLeads = COUNT(*) 
    FROM Leads


SELECT @ConvertedLeads = COUNT(*)
FROM Leads
WHERE Status = 4

    SET @ConversionRate =
        CASE
            WHEN @TotalLeads = 0 THEN 0
            ELSE (@ConvertedLeads * 100.0 / @TotalLeads)
        END

    SELECT @ActiveTeams = COUNT(*)
    FROM Teams
    WHERE IsActive = 1

    SELECT
        @TotalRevenue AS TotalRevenue,
        @TotalSales AS TotalSales,
        @ConversionRate AS AvgConversionRate,
        @ActiveTeams AS ActiveTeams
END