using Dapper;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Infrastructure.Repositories
{
    public class StudentRepository :IStudentRepository
    {
        private readonly IDbConnection _db;
        public StudentRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _db.QueryAsync<Student>(
                "sp_GetAllUsers",
                commandType: CommandType.StoredProcedure);
        }
        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _db.QueryFirstOrDefaultAsync<Student>(
                "sp_GetStudentById",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }
    }
}
