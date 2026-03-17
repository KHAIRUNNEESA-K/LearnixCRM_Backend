using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Infrastructure.Repositories
{
    public class CourseRepository: ICourseRepository
    {
        private readonly LearnixDbContext _context;

        public CourseRepository(LearnixDbContext context)
        {
            _context = context;
        }
        public async Task<List<Course>> GetAllAsync()
        {
            return await _context.Courses
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }
        public async Task<bool> ExistsAsync(int courseId)
        {
            return await _context.Courses
                .AnyAsync(x => x.CourseId == courseId);
        }

    }
}
