CREATE PROCEDURE sp_GetAdmissionDashboard
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CurrentMonth INT
    DECLARE @PreviousMonth INT

    SELECT @CurrentMonth = COUNT(*)
    FROM Students
    WHERE MONTH(JoinedDate) = MONTH(GETDATE())
      AND YEAR(JoinedDate) = YEAR(GETDATE())

    SELECT @PreviousMonth = COUNT(*)
    FROM Students
    WHERE MONTH(JoinedDate) = MONTH(DATEADD(MONTH,-1,GETDATE()))
      AND YEAR(JoinedDate) = YEAR(DATEADD(MONTH,-1,GETDATE()))

    SELECT
        @CurrentMonth AS TotalAdmissions,
        CASE 
            WHEN @PreviousMonth = 0 THEN 0
            ELSE ((@CurrentMonth - @PreviousMonth) * 100.0 / @PreviousMonth)
        END AS GrowthRate,

        CASE
            WHEN @CurrentMonth = 0 THEN 0
            ELSE (
                SELECT COUNT(*) * 100.0 / @CurrentMonth
                FROM Students
                WHERE MONTH(JoinedDate)=MONTH(GETDATE())
            )
        END AS Efficiency
END