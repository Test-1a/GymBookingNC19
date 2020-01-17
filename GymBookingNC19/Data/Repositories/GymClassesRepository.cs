using GymBookingNC19.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymBookingNC19.Data.Repositories
{
    public class GymClassesRepository
    {
        private readonly ApplicationDbContext _context;

        public GymClassesRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<GymClass> GetAsync(int? id)
        {
            return await _context.GymClasses
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<GymClass> GetWithAttendingMembersAsync(int? id)
        {
            return await _context.GymClasses
                .Include(a => a.AttendingMembers)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public bool GetAny(int id)
        {
            return _context.GymClasses.Any(e => e.Id == id);
        }

        public async Task<List<GymClass>> GetAllWithUsersAsync()
        {
            return await _context.GymClasses
               .Include(g => g.AttendingMembers)
               .ThenInclude(a => a.ApplicationUser)
               .ToListAsync();
        }

        public async Task<List<GymClass>> GetHistoryAsync()
        {
            return await _context.GymClasses
           .Include(g => g.AttendingMembers)
           .ThenInclude(a => a.ApplicationUser)
           .IgnoreQueryFilters()
           .Where(g => g.StartDate < DateTime.Now)
           .ToListAsync();
        }

        public async Task<List<GymClass>> GetAllBookingsAsync(string userId)
        {
            return await _context.ApplicationUserGymClasses
                .Where(ag => ag.ApplicationUserId == userId)
                .IgnoreQueryFilters()
                .Select(ag => ag.GymClass)
                .ToListAsync();
        }
    }
}
