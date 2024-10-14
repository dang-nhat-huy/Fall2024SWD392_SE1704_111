using BusinessObject.Model;
using BusinessObject.Paging;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.ResponseDTO.ResponseDTO;

namespace Repository.Repository
{
    public class HairServiceRepository : GenericRepository<HairService>, IHairServiceRepository
    {
        public HairServiceRepository() { }

        public HairServiceRepository(HairSalonBookingContext context) => _context = context;
        public async Task<List<HairService?>> GetServiceByNameAsync(string serviceName)
        {
            return await _context.HairServices
                .Where(h => h.ServiceName.ToLower().StartsWith(serviceName.ToLower()))
                .ToListAsync(); // Return list of HairService
        }

    }
}
