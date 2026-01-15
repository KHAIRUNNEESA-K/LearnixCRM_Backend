CREATE PROCEDURE [dbo].[sp_GetPendingInvites]
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
        UpdatedBy
    FROM UserInvites
    WHERE IsUsed = 0
      AND ExpiryDate > GETUTCDATE()
      AND DeletedAt IS NULL;
END