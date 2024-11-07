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
    public class ScheduleRepository : GenericRepository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository() { }

        public ScheduleRepository(HairSalonBookingContext context) => _context = context;

        public async Task<int> CreateScheduleAsync(Schedule entity)
        {

            _context.Add(entity);
            return await _context.SaveChangesAsync();


        }

        public async Task<Schedule> GetScheduleById(int scheduleId)
        {
            return await GetByIdAsync(scheduleId);
        }

        public async Task<List<Schedule>> GetScheduleHistoryByScheduleIdAsync(int scheduleId)
        {
            return await _context.Schedules
                 .Where(s => s.ScheduleId == scheduleId)
                 .Include(s => s.BookingDetails)
                     .ThenInclude(bd => bd.Booking) // Bao gồm thông tin Booking của BookingDetail
                 .Include(s => s.BookingDetails)
                     .ThenInclude(bd => bd.Service) // Bao gồm thông tin Service của BookingDetail
                 .Include(s => s.BookingDetails)
                     .ThenInclude(bd => bd.Stylist) // Bao gồm thông tin Stylist của BookingDetail
                 .Include(s => s.ScheduleUsers) // Bao gồm thông tin ScheduleUser liên quan
                .ToListAsync(); // Lấy đối tượng Schedule đầu tiên hoặc null nếu không có
        } 
    }
}
