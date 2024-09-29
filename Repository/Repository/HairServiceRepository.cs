using BusinessObject.Model;
using BusinessObject.Paging;
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

       
    }
}
