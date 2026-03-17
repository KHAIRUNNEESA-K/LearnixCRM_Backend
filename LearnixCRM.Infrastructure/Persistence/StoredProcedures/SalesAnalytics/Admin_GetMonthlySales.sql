CREATE PROCEDURE sp_Admin_GetMonthlySales
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        DATENAME(MONTH, a.CreatedAt) AS MonthName,
        COUNT(*) AS StudentsEnrolled
    FROM Admissions a
    WHERE a.CreatedAt >= DATEADD(MONTH, -12, GETDATE())
    GROUP BY 
        DATENAME(MONTH, a.CreatedAt),
        MONTH(a.CreatedAt)
    ORDER BY MONTH(a.CreatedAt)
END;