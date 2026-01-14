using Dapper;
using LearnixCRM.Application.Interfaces;
using LearnixCRM.Domain.Entities;
using System.Data;

public class PasswordResetRepository : IPasswordResetRepository
{
    private readonly IDbConnection _db;

    public PasswordResetRepository(IDbConnection db)
    {
        _db = db;
    }
    public async Task CreatePasswordResetAsync(string email, string token, string requestedBy)
    {
        await _db.ExecuteAsync(
            "sp_CreatePasswordReset",
            new { Email = email, Token = token, RequestedBy = requestedBy },
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<PasswordReset?> GetByTokenAsync(string token)
    {
        return await _db.QueryFirstOrDefaultAsync<PasswordReset>(
            "sp_GetPasswordResetByToken",
            new { Token = token },
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _db.QueryFirstOrDefaultAsync<User>(
            "sp_GetUserByEmail",
            new { Email = email.ToLower() },
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task ResetPasswordAsync(string email, string passwordHash, string token)
    {
        await _db.ExecuteAsync(
            "sp_ResetPassword", 
            new { Email = email, PasswordHash = passwordHash, Token = token },
            commandType: CommandType.StoredProcedure
        );
    }
}
