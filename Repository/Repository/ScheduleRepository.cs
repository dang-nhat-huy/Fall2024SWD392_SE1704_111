using BusinessObject.Model;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class ScheduleRepository : GenericRepository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository() { }

        public ScheduleRepository(HairSalonBookingContext context) => _context = context;

        public async Task<int> CreateScheduleAsync(Schedule entity)
        {

            _context.Add(entity);
            return await _context.SaveChangesAsync();


        }
    }
}
