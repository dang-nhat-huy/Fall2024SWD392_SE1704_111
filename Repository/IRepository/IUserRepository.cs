using BusinessObject;
using BusinessObject.Models;
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

        
    }
}
