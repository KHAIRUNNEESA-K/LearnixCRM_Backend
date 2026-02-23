namespace LearnixCRM.Infrastructure.Repositories
{
    using Dapper;
    using LearnixCRM.Application.Interfaces;
    using LearnixCRM.Domain.Entities;
    using LearnixCRM.Domain.Enum;
    using LearnixCRM.Infrastructure.Persistence.Context;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.Data;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class SalesLeadRepository : ISalesLeadRepository
    {
        private readonly IDbConnection _db;
        private readonly LearnixDbContext _context;
        private readonly int _currentUserId;

        public SalesLeadRepository(
            IDbConnection db,
            LearnixDbContext context,
            IHttpContextAccessor accessor)
        {
            _db = db;
            _context = context;

            var userId = accessor.HttpContext?.User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            _currentUserId = int.Parse(userId!);
        }

        public async Task<IEnumerable<Lead>> GetAllAsync()
        {
            return await _db.QueryAsync<Lead>(
                "sp_GetAllLeadsForSalesUser",
                new { AssignedToUserId = _currentUserId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Lead?> GetByIdAsync(int id)
        {
            return await _db.QueryFirstOrDefaultAsync<Lead>(
                "sp_GetLeadByIdForSalesUser",
                new
                {
                    LeadId = id,
                    AssignedToUserId = _currentUserId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Lead>> GetByStatusAsync(LeadStatus status)
        {
            return await _db.QueryAsync<Lead>(
                "sp_GetLeadsByStatusForSalesUser",
                new
                {
                    Status = (int)status,
                    AssignedToUserId = _currentUserId
                },
                commandType: CommandType.StoredProcedure);
        }


        public async Task AddAsync(Lead lead)
        {
            await _context.Leads.AddAsync(lead);

            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(Lead lead)
        {
            _context.Leads.Update(lead);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Lead lead)
        {
            lead.Delete(_currentUserId);

            _context.Leads.Update(lead);

            await _context.SaveChangesAsync();
        }
        public async Task AddBlacklistAsync(Blacklist blacklist)
        {
            await _context.Blacklists.AddAsync(blacklist);
            await _context.SaveChangesAsync();
        }

        public async Task AddStudentAsync(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }

    }
}
