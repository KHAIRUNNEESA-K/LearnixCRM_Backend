CREATE PROCEDURE [dbo].[sp_GetTeamById]
    @TeamId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        TeamId,
        TeamName,
        ManagerUserId,
        IsActive,
        CreatedAt,
        CreatedBy
    FROM Teams
    WHERE TeamId = @TeamId
      AND DeletedAt IS NULL;
END