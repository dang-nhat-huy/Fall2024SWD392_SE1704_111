using AutoMapper;
using BusinessObject;
using BusinessObject.Model;
using BusinessObject.Paging;
using BusinessObject.ResponseDTO;
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
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HairServiceService(IHairServiceRepository serviceRepository, UnitOfWork unitOfWork, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<HairService>> GetListServicesAsync(int pageNumber, int pageSize)
        {
            var query = _serviceRepository.GetAll();
            return await Paging.GetPagedResultAsync(query.AsQueryable(), pageNumber, pageSize);
        }

        public async Task<ResponseDTO> GetServiceByIdAsync(int id)
        {
            try
            {
                var service = await _unitOfWork.HairServiceRepository.GetByIdAsync(id);

                var result = _mapper.Map<ServicesDTO>(service);

                if (service == null)
                {
                    return new ResponseDTO(Const.SUCCESS_CREATE_CODE, "Services not found");
                }

                return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);

            }
            catch (Exception e)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, e.Message);
            }

        }
    }
}
