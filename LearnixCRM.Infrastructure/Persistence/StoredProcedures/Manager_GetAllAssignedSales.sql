CREATE PROCEDURE [dbo].[sp_Manager_GetAllAssignedSales]
    @ManagerUserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        s.UserId AS SalesUserId,
        s.FullName AS SalesUserName,
        s.Email AS Email
    FROM AssignUsers a
    INNER JOIN Users s ON a.SalesUserId = s.UserId
    WHERE a.ManagerUserId = @ManagerUserId
      AND a.IsActive = 1
      AND a.DeletedAt IS NULL
      AND s.Status = 2 
    ORDER BY s.FullName;
END
