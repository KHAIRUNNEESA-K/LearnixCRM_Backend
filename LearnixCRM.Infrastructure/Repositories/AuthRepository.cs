namespace LearnixCRM.Infrastructure.Repositories
{
    using System.Data;
    using Dapper;
    using LearnixCRM.Application.DTOs;
    using LearnixCRM.Application.Interfaces.Repositories;
    using LearnixCRM.Domain.Entities;

    public class AuthRepository : IAuthRepository
    {
        private readonly IDbConnection _dbConnection;

        public AuthRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<User?> GetUserForLoginAsync(string email)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Email", email);

            return await _dbConnection.QueryFirstOrDefaultAsync<User>(
                "sp_GetUserForLogin",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
