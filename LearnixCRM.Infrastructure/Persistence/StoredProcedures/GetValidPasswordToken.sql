CREATE PROCEDURE sp_GetValidPasswordToken
(
    @TokenHash NVARCHAR(200)
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        UserPasswordTokenId,
        UserId,
        TokenHash,
        TokenType,
        Expiry,
        IsUsed,
        UsedAt,
        CreatedAt,
        CreatedBy,
        UpdatedAt,
        UpdatedBy
    FROM UserPasswordTokens
    WHERE TokenHash = @TokenHash
      AND IsUsed = 0
      AND Expiry > GETUTCDATE();
END
GO
