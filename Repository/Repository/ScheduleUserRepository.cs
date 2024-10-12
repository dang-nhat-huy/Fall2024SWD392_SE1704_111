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

        public async Task<IQueryable<ScheduleUser>> GetUserByRoleAsync()
        {
            return _context.ScheduleUsers.Include(get => get.Schedule)
                .Include(y => y.User)
                .Where(u => u.User.Role == UserRole.Stylist);
        }
    }
}
