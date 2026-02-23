CREATE PROCEDURE [dbo].[sp_GetSalesByManagerId]
    @ManagerUserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        s.UserId AS SalesUserId,
        s.FullName AS SalesUserName,
        s.Email AS Email,            
        a.AssignId AS AssignmentId,
        a.CreatedAt,
        a.CreatedBy
    FROM AssignUsers a
    INNER JOIN Users s ON a.SalesUserId = s.UserId
    WHERE a.ManagerUserId = @ManagerUserId
      AND a.IsActive = 1
      AND a.DeletedAt IS NULL
    ORDER BY s.FullName;
END