using BusinessObject.Model;
using BusinessObject.ResponseDTO;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{

    public class UserProfileRepository : GenericRepository<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository() { }
       
        public UserProfileRepository(HairSalonBookingContext context) => _context = context;

        public async Task<List<UserProfile>> getAllUserProfile()
        {
            var result = await _context.UserProfiles.ToListAsync();
            return result;
        }

        public async Task<User?> GetUserByCurrentId(int userId)
        {
            return await _context.Users
                .Include(u => u.UserProfile)  // Include UserProfile
                .FirstOrDefaultAsync(u => u.UserId == userId);  // Tìm user theo userId
        }
    }
}
