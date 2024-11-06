using BusinessObject;
using BusinessObject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IScheduleUserRepository : IGenericRepository<ScheduleUser>
    {
        Task<IQueryable<ScheduleUser>> GetListScheduleByRoleAsync();
        Task<ScheduleUser?> GetByUserAndScheduleIdAsync(int userId, int scheduleId);

    }
}
