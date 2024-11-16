using BusinessObject;
using BusinessObject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<bool> ExistsByNameAsync(string name);

        Task<bool> ExistsByEmailAsync(string email);

        Task<User?> GetUserByCurrentId(int userId);

        Task<User?> GetUserByIdAsync(int userId);

        Task<List<User?>> GetUserByNameAsync(string fullName);

        Task<User?> GetUserByUserNameAsync(string userName);
        IQueryable<User> GetListUserByUserName(string userName);
        IQueryable<User> GetUsersExcludingCurrentUserAndRoleAsync(int currentUserId, UserRole? role);
        IQueryable<User> GetUsersByNameExcludingCurrentUserAndRoleAsync(int currentUserId, UserRole? role, string? userName);
        Task<List<User>> GetAllStylistsAsync();
        Task<List<User>> GetStylistsByStatusAsync(UserStatus status);
        Task<List<User>> GetStylistsExcludingIdsAsync(int? excludedUserIds, UserStatus status);
    }
}
