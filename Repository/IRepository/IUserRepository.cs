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


    }
}
