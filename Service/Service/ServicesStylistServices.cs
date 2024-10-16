using AutoMapper;
using BusinessObject;
using BusinessObject.Model;
using BusinessObject.ResponseDTO;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class ServicesStylistServices : IServicesStylistServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ServicesStylistServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseDTO> GetStylistsByServiceIdAsync(int serviceId)
        {
            try
            {
                var stylists = await _unitOfWork.ServicesStylistRepository.GetStylistsByServiceIdAsync(serviceId);

                if (stylists == null || !stylists.Any())
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "No stylists found for the provided service ID.");
                }
                else
                {
                    var stylistResponse = _mapper.Map<List<StylistResponseDTO>>(stylists);
                    return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, stylistResponse);
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
    }
}
