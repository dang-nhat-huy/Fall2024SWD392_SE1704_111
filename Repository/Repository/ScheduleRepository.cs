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

        public async Task<Schedule?> GetNextScheduleAsync(int currentScheduleId)
        {
            // Lấy lịch hiện tại
            var currentSchedule = await _context.Schedules
                .Where(s => s.ScheduleId == currentScheduleId)
                .FirstOrDefaultAsync();

            if (currentSchedule == null)
            {
                return null; // Nếu lịch không tồn tại, trả về null
            }

            // Lấy tất cả lịch mà có thời gian bắt đầu sau thời gian kết thúc của lịch hiện tại
            var nextSchedules = await _context.Schedules
                .Where(s => s.StartDate > currentSchedule.EndDate ||
                             (s.StartDate == currentSchedule.EndDate && s.StartTime > currentSchedule.EndTime))
                .OrderBy(s => s.StartDate)
                .ThenBy(s => s.StartTime) // Sắp xếp theo thời gian bắt đầu
                .ToListAsync();

            // Trả về lịch đầu tiên trong danh sách lịch kế tiếp
            return nextSchedules.FirstOrDefault();
        }
    }
}
