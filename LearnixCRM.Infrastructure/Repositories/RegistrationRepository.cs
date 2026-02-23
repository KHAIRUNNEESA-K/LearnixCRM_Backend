using Dapper;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Infrastructure.Repositories
{
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly LearnixDbContext _dbContext;
        private readonly IDbConnection _dbConnection;

        public RegistrationRepository(LearnixDbContext dbContext , IDbConnection dbConnection)
        {
            _dbContext = dbContext;
            _dbConnection = dbConnection;
        }

        public async Task<bool> IsEmailRegisteredAsync(string email)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Email", email);
            var result = await _dbConnection.QueryFirstOrDefaultAsync<int>(
                "sp_CheckEmailExists",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            return result > 0;
        }
        public async Task<bool> IsContactNumberRegisteredAsync(string contactNumber)
        {
            return await _dbContext.Users
                .AnyAsync(u => u.ContactNumber == contactNumber
                               && u.DeletedAt == null);
        }

        public async Task CreateUserAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

    }
}
