CREATE PROCEDURE [dbo].[sp_ResetPassword]
    @Email NVARCHAR(256),
    @PasswordHash NVARCHAR(256),
    @Token NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    IF EXISTS (SELECT 1 FROM PasswordResets WHERE Token = @Token AND IsUsed = 1)
    BEGIN
        ROLLBACK TRANSACTION;
        RAISERROR('Token already used', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM PasswordResets WHERE Token = @Token AND ExpiryDate <= GETUTCDATE())
    BEGIN
        ROLLBACK TRANSACTION;
        RAISERROR('Token expired', 16, 1);
        RETURN;
    END

 
    UPDATE Users
    SET PasswordHash = @PasswordHash, Status = 1
    WHERE Email = @Email;

    UPDATE PasswordResets
    SET IsUsed = 1,
        Email = @Email,
        UpdatedAt = GETUTCDATE()
    WHERE Token = @Token;

    COMMIT TRANSACTION;
END
