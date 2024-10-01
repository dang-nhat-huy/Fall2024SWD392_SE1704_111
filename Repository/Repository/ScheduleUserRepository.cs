using BusinessObject.Model;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class ScheduleUserRepository : GenericRepository<ScheduleUser>, IScheduleUserRepository
    {
        public ScheduleUserRepository() { }

        public ScheduleUserRepository(HairSalonBookingContext context) => _context = context;
    }
}
