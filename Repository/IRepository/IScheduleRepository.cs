using BusinessObject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IScheduleRepository : IGenericRepository<Schedule>
    {
        public  Task<int> CreateScheduleAsync(Schedule entity);
        Task<Schedule> GetScheduleById(int scheduleId);
    }
}
