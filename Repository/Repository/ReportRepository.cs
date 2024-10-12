using BusinessObject.Model;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class ReportRepository : GenericRepository<Report>, IReportRepository
    {
        public ReportRepository() { }
        public ReportRepository(HairSalonBookingContext context) => _context = context;
        public async Task<User?> GetUserByCurrentId(int userId)
        {
            return await _context.Users
                .Include(u => u.UserProfile)  // Include UserProfile
                .FirstOrDefaultAsync(u => u.UserId == userId);  // Tìm user theo userId
        }
    }
}
