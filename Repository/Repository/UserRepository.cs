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

        public async Task<User?> GetUserByCurrentId(int userId)
        {
            return await _context.Users
                .Include(u => u.UserProfile)  // Include UserProfile
                .FirstOrDefaultAsync(u => u.UserId == userId);  // Tìm user theo userId
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await GetByIdAsync(userId);
        }

        public async Task<List<User?>> GetUserByNameAsync(string fullName)
        {
            return await _context.Users
                .Where(u => u.UserName.ToLower().StartsWith(fullName.ToLower()))
                .ToListAsync(); // Trả về danh sách
        }

        public async Task<User?> GetUserByUserNameAsync(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }
    }
}
