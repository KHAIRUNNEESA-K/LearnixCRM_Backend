namespace LearnixCRM.Infrastructure.Repositories
{
    using System.Data;
    using Dapper;
    using LearnixCRM.Application.DTOs;
    using LearnixCRM.Application.Interfaces;

    public class AuthRepository : IAuthRepository
    {
        private readonly IDbConnection _dbConnection;

        public AuthRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<LoginUserDto?> GetUserForLoginAsync(string email)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Email", email);

            return await _dbConnection.QueryFirstOrDefaultAsync<LoginUserDto>(
                "sp_GetUserForLogin",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
