using BusinessObject.Model;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class ServicesStylistRepository : GenericRepository<ServicesStylist>, IServicesStylistRepository
    {
        public ServicesStylistRepository() { }

        public ServicesStylistRepository(HairSalonBookingContext context) => _context = context;

        
    }
}
