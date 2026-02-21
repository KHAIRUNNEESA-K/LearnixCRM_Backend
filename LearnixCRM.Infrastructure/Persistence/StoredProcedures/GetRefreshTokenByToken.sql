CREATE PROCEDURE sp_GetRefreshTokenByToken
    @Token NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM RefreshTokens
    WHERE Token = @Token
END