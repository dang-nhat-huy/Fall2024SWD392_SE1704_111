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

        Task<User?> GetUserByCurrentId(int userId);

        Task<User?> GetUserByIdAsync(int userId);

        Task<List<User?>> GetUserByNameAsync(string fullName);

        Task<User?> GetUserByUserNameAsync(string userName);
    }
}
