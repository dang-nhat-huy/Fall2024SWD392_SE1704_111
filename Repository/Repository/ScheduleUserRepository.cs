using BusinessObject;
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
    public class ScheduleUserRepository : GenericRepository<ScheduleUser>, IScheduleUserRepository
    {
        public ScheduleUserRepository() { }

        public ScheduleUserRepository(HairSalonBookingContext context) => _context = context;

        public async Task<IQueryable<ScheduleUser>> GetListScheduleByRoleAsync()
        {
            return _context.ScheduleUsers.Include(get => get.Schedule)
                .Include(y => y.User)
                .Where(u => u.User.Role == UserRole.Stylist);
        }

        public async Task<ScheduleUser?> GetByUserAndScheduleIdAsync(int userId, int? scheduleId)
        {
            return await _context.ScheduleUsers
                .FirstOrDefaultAsync(su => su.UserId == userId && su.ScheduleId == scheduleId);
        }

        public async Task<List<ScheduleUser>> GetScheduleUserByStylistIdAsync(int stylistId)
        {
            return await _context.ScheduleUsers
                .Where(u => u.UserId == stylistId)
                .Include("Schedule")
                .ToListAsync();
        }

        public async Task<List<ScheduleUser>> GetScheduleUsersOfStylistsAsync()
        {
            return await _context.ScheduleUsers
                .Include(su => su.User)              // Bao gồm thông tin User
                    .ThenInclude(u => u.UserProfile) // Bao gồm thông tin UserProfile
                .Include(su => su.Schedule)          // Bao gồm thông tin Schedule
                .Where(su => su.User != null && su.User.Role == UserRole.Stylist) // Lọc stylist
                .OrderBy(su => su.Schedule.StartDate) // Sắp xếp theo ngày bắt đầu
                .ThenBy(su => su.Schedule.StartTime)  // Sau đó sắp xếp theo giờ bắt đầu
                .ToListAsync();
        }

        public async Task<List<int>> GetUserIdsByScheduleIdAsync(int scheduleId)
        {
            return await _context.ScheduleUsers
                .Where(su => su.ScheduleId == scheduleId)
                .Select(su => su.UserId.GetValueOrDefault())  // Dùng GetValueOrDefault() để lấy giá trị int hoặc 0 nếu null
                .ToListAsync();
        }

        //public async Task<int?> GetExcludedUserIdByScheduleIdAsync(int scheduleId)
        //{
        //    return await _context.ScheduleUsers
        //        .Where(su => su.ScheduleId == scheduleId)
        //        .Select(su => su.UserId)
        //        .FirstOrDefaultAsync();
        //}

        public async Task<List<int>> GetExcludedUserIdsByScheduleIdAsync(int scheduleId)
        {
            var list = await _context.ScheduleUsers
                .Where(su => su.ScheduleId == scheduleId && su.UserId.HasValue)
                .Select(su => su.UserId.Value)
                .ToListAsync();

            return list;
        }
    }
}
