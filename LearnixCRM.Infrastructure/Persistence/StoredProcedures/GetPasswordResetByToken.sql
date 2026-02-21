CREATE PROCEDURE [dbo].[sp_GetPasswordResetByToken]
    @Token NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        ResetId,
        Email,
        Token,
        ExpiryDate,
        IsUsed,
        CreatedAt,
        CreatedBy,
        UpdatedAt,
        UpdatedBy,
        DeletedAt,
        DeletedBy
    FROM PasswordResets
    WHERE Token = @Token
      AND IsUsed = 0
      AND ExpiryDate > GETUTCDATE()
      AND DeletedAt IS NULL
END