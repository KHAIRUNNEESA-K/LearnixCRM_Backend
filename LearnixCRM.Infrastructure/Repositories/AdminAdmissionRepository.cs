using Dapper;
using LearnixCRM.Application.DTOs.Student;
using LearnixCRM.Application.Interfaces.Repositories;
using System.Data;

namespace LearnixCRM.Infrastructure.Repositories
{
    public class AdminAdmissionRepository : IAdminAdmissionRepository
    {
        private readonly IDbConnection _connection;

        public AdminAdmissionRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<AdminStudentResponseDto>> GetAllAsync()
        {
            return await _connection.QueryAsync<AdminStudentResponseDto>(
                "sp_GetAllAdmissions",
                commandType: CommandType.StoredProcedure);
        }
    }
}