using Dapper;
using LearnixCRM.Application.DTOs.User;
using LearnixCRM.Application.Interfaces.Repositories;
using System.Data;

namespace LearnixCRM.Infrastructure.Repositories
{
    public class AssignedSalesRepository : IAssignedSalesRepository
    {
        private readonly IDbConnection _db;

        public AssignedSalesRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<SalesUserDto>> GetSalesByManagerAsync(int managerUserId)
        {
            return await _db.QueryAsync<SalesUserDto>(
                "sp_Manager_GetAllAssignedSales",
                new { ManagerUserId = managerUserId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<SalesUserDto?> GetSalesByManagerAndSalesIdAsync(int managerUserId,int salesUserId)
        {
            return await _db.QueryFirstOrDefaultAsync<SalesUserDto>(
                "sp_Manager_GetAssignedSalesById",
                new
                {
                    ManagerUserId = managerUserId,
                    SalesUserId = salesUserId
                },
                commandType: CommandType.StoredProcedure
            );
        }

    }
}