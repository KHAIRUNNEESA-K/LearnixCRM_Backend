CREATE PROCEDURE sp_GetBlacklistByEmailOrPhone
    @Email NVARCHAR(100) = NULL,
    @Phone NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Return blacklisted entry matching email or phone
    SELECT 
        BlacklistId,
        Email,
        Phone,
        Reason,
        BlacklistedOn,
        CreatedBy,
        CreatedAt
    FROM Blacklists
    WHERE DeletedAt IS NULL  -- Only active entries
      AND (
            (@Email IS NOT NULL AND Email = @Email)
         OR (@Phone IS NOT NULL AND Phone = @Phone)
      )
    ORDER BY BlacklistedOn DESC;
END
GO