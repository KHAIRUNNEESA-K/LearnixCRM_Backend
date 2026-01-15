CREATE PROCEDURE [dbo].[sp_GetInviteByToken]
    @Token NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        InviteId,
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
    FROM UserInvites
    WHERE Token = @Token
      AND IsUsed = 0
      AND ExpiryDate > GETUTCDATE()
      AND DeletedAt IS NULL;
END
