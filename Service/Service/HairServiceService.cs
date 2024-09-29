using BusinessObject.Model;
using BusinessObject.Paging;
using Repository.IRepository;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.ResponseDTO.ResponseDTO;

namespace Service.Service
{
    public class HairServiceService : IHairServiceService
    {
        private readonly IHairServiceRepository _serviceRepository;

        public HairServiceService(IHairServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<PagedResult<HairService>> GetListServicesAsync(int pageNumber, int pageSize)
        {
            var query = _serviceRepository.GetAll();
            return await Paging.GetPagedResultAsync(query.AsQueryable(), pageNumber, pageSize);
        }
    }
}
