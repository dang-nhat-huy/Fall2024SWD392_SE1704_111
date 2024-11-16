using BusinessObject;
using BusinessObject.Model;
using BusinessObject.ResponseDTO;
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

        public IQueryable<User> GetListUserByUserName(string userName)
        {
            return _context.Users
                .Where(u => u.UserName.ToLower().StartsWith(userName.ToLower()));
        }

        public IQueryable<User> GetUsersExcludingCurrentUserAndRoleAsync(int currentUserId, UserRole? role)
        {
            // Lấy danh sách người dùng và loại bỏ người dùng hiện tại
            var users = _context.Users.Where(u => u.UserId != currentUserId);

            // Nếu vai trò là "Manager", loại bỏ luôn những người dùng có vai trò "Admin"
            if (role.HasValue && role.Value == UserRole.Manager)
            {
                users = users.Where(u => u.Role != UserRole.Admin); // Lọc ra những người không phải "Admin"
            }

            return users; // Trả về IQueryable<User>
        }

        public IQueryable<User> GetUsersByNameExcludingCurrentUserAndRoleAsync(int currentUserId, UserRole? role, string? userName)
        {
            // Lấy danh sách người dùng và loại bỏ người dùng hiện tại
            var users = _context.Users.Where(u => u.UserId != currentUserId);

            // Nếu vai trò là "Manager", loại bỏ luôn những người dùng có vai trò "Admin"
            if (role.HasValue && role.Value == UserRole.Manager)
            {
                users = users.Where(u => u.Role != UserRole.Admin); // Lọc ra những người không phải "Admin"
            }

            // Nếu có tham số userName, thêm điều kiện tìm kiếm theo tên người dùng
            if (!string.IsNullOrEmpty(userName))
            {
                users = users.Where(u => u.UserName.Contains(userName)); // Tìm kiếm người dùng theo username
            }

            return users; // Trả về IQueryable<User>
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            // Kiểm tra xem có bất kỳ người dùng nào với email đã cho hay không
            return await _context.Users.AnyAsync(u => u.UserProfile.Email == email);
        }

        public async Task<List<User>> GetAllStylistsAsync()
        {
            return await _context.Users
                .Include(u => u.UserProfile) // Bao gồm thông tin UserProfile nếu cần
                .Where(u => u.Role == UserRole.Stylist) // Lọc chỉ stylist
                .ToListAsync();
        }

        public async Task<List<User>> GetStylistsByStatusAsync(UserStatus status)
        {
            return await _context.Users
                .Where(u => u.Role == UserRole.Stylist && u.Status == status)
                .Include(u => u.UserProfile)
                .ToListAsync();
        }


        public async Task<List<User>> GetStylistsExcludingIdsAsync(int? excludedUserId, UserStatus status)
        {
            return await _context.Users
                .Where(u => u.Role == UserRole.Stylist && u.Status == status && u.UserId != excludedUserId)
                .Include(u => u.UserProfile)
                .ToListAsync();
        }



    }
}
