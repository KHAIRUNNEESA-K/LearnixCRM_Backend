using Dapper;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LearnixCRM.Infrastructure.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly LearnixDbContext _context;
        private readonly IDbConnection _db;

        public CourseRepository(
            LearnixDbContext context,
            IDbConnection db)
        {
            _context = context;
            _db = db;
        }
        public async Task AddAsync(Course course)
        {
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Course>> GetAllAsync()
        {
            var result = await _db.QueryAsync<Course>(
                "sp_GetAllCourses",
                commandType: CommandType.StoredProcedure);

            return result.ToList();
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _db.QueryFirstOrDefaultAsync<Course>(
                "sp_GetCourseById",
                new { CourseId = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateAsync(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Courses
                .AnyAsync(c => c.Name == name && c.IsActive);
        }

        public async Task<bool> ExistsByNameExceptIdAsync(string name, int id)
        {
            return await _context.Courses
                .AnyAsync(c => c.Name == name && c.CourseId != id && c.IsActive);
        }
    }
}