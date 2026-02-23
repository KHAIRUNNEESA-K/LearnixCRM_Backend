CREATE PROCEDURE sp_GetBlacklistById
    @BlacklistId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        BlacklistId,
        Email,
        Phone,
        Reason,
        BlacklistedOn,
        CreatedBy,
        CreatedAt
    FROM Blacklists
    WHERE BlacklistId = @BlacklistId
      AND DeletedAt IS NULL;  -- Only return active entries
END
GO