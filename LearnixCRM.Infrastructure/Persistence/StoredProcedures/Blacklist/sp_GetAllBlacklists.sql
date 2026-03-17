CREATE PROCEDURE sp_GetAllBlacklists
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
    WHERE DeletedAt IS NULL 
    ORDER BY BlacklistedOn DESC;
END
GO
