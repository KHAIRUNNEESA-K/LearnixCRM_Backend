CREATE PROCEDURE [dbo].[sp_CreatePasswordReset]
    @Email NVARCHAR(256),
    @Token NVARCHAR(256),
    @RequestedBy NVARCHAR(256)
AS
BEGIN
INSERT INTO PasswordResets (Email, Token, CreatedAt, ExpiryDate, IsUsed)
VALUES (
    @Email,
    @Token,
    GETUTCDATE(),                 
    DATEADD(MINUTE, 30, GETUTCDATE()), 
    0
)

END