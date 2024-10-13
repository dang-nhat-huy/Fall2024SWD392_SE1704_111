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

        public async Task<Booking?> GetBookingById(int bookingId)
        {
            return await _context.Bookings.FirstOrDefaultAsync(b =>b.BookingId == bookingId);
        }

        public async Task<Report?> GetReportById(int reportId)
        {
            return await _context.Reports
                .Include(r => r.BookingId)
                .FirstOrDefaultAsync(b => b.ReportId == reportId);
        }

        public async Task<User?> GetUserByCurrentId(int userId)
        {
            return await _context.Users
                .Include(u => u.UserProfile)  // Include UserProfile
                .FirstOrDefaultAsync(u => u.UserId == userId);  // Tìm user theo userId
        }

        public async Task<int> CreateReportAsync(Report entity)
        {
            
                _context.Add(entity);
                return await _context.SaveChangesAsync();
            
            
        }
    }
}
