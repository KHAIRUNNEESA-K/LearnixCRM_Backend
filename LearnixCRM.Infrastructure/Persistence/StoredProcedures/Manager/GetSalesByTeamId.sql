CREATE PROCEDURE [dbo].[sp_GetSalesByTeamId]
    @TeamId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        s.UserId AS SalesUserId,
        s.FullName AS SalesUserName,
        s.Email
    FROM AssignUsers a
    INNER JOIN Users s ON a.SalesUserId = s.UserId
    WHERE a.TeamId = @TeamId
      AND a.IsActive = 1
      AND a.DeletedAt IS NULL
    ORDER BY s.FullName;
END