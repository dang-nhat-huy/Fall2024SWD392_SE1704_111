using BusinessObject;
using BusinessObject.Model;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Repository.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository() { }

        public UserRepository(HairSalonBookingContext context) => _context = context;

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Users.AnyAsync(u => u.UserName.ToLower() == name.ToLower());
        }

        public async Task<IQueryable<ScheduleUser>> GetUserByRoleAsync()
        {
            return _context.ScheduleUsers.Include(get => get.Schedule)
                .Include(y => y.User)
                .Where(u => u.User.Role == UserRole.Stylist);
        }
    }
}
