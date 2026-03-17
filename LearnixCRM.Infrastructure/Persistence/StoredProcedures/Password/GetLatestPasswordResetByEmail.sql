CREATE PROCEDURE [dbo].[sp_GetLatestPasswordResetByEmail]
    @Email NVARCHAR(200)
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
    WHERE Email = LOWER(@Email)
      AND DeletedAt IS NULL
    ORDER BY CreatedAt DESC
END