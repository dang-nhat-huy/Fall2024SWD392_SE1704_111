using AutoMapper;
using BusinessObject;
using BusinessObject.Model;
using BusinessObject.Paging;
using BusinessObject.RequestDTO;
using BusinessObject.ResponseDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Repository.IRepository;
using Service.IService;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.RequestDTO.RequestDTO;
using static BusinessObject.ResponseDTO.ResponseDTO;

namespace Service.Service
{
    public class HairServiceService : IHairServiceService
    {
        private readonly IHairServiceRepository _serviceRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public HairServiceService(IHairServiceRepository serviceRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }


        public async Task<ResponseDTO> GetListServicesAsync()
        {
            try
            {
                var services = await _unitOfWork.HairServiceRepository.GetAllAsync();

                if (services == null || !services.Any())
                {
                    return new ResponseDTO(Const.SUCCESS_CREATE_CODE, "Empty List");
                }

                var result = _mapper.Map<List<ServicesDTO>>(services);

                return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }
            catch (Exception e)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, e.Message);
            }
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



        public async Task<ResponseDTO> CreateReportAsync(CreateServiceDTO request)
        {
            try
            {
                // Sử dụng AutoMapper 
                var report = _mapper.Map<HairService>(request);


                report.CreateDate = DateTime.Now;
                report.UpdateDate = DateTime.Now;

                // Lưu các thay đổi vào cơ sở dữ liệu
                var result = await _unitOfWork.HairServiceRepository.CreateAsync(report);

                return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }


            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message, ex);
            }
        }

    


    public async Task<ResponseDTO> UpdateReportAsync(RequestDTO.UpdateServiceDTO request, int serviceId)
    {
            try
            {
                var service = await _unitOfWork.HairServiceRepository.GetByIdAsync(serviceId);
                if (service == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "report not found");
                }
                // Sử dụng AutoMapper 
                 _mapper.Map<HairService>(request);

                service.UpdateDate = DateTime.Now;

                // Lưu các thay đổi vào cơ sở dữ liệu
                var result = await _unitOfWork.HairServiceRepository.UpdateAsync(service);
                if (result == null)
                {
                    return new ResponseDTO(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, "Update Report Failed");
                }

                return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, "Update Service status Succeed");
            }


            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message, ex);
            }
        }

    public async Task<ResponseDTO> ChangeReportStatusAsync(RequestDTO.RemoveServiceDTO request, int servicetId)
        {
            try { 
            var service = await _unitOfWork.HairServiceRepository.GetByIdAsync(servicetId);
            if (service == null)
            {
                return new ResponseDTO(Const.FAIL_READ_CODE, "report not found");
            }

            // Sử dụng AutoMapper 
            _mapper.Map(request, service);
            service.UpdateDate = DateTime.Now;

            // Lưu các thay đổi vào cơ sở dữ liệu
            var result = _unitOfWork.HairServiceRepository.UpdateAsync(service);

            if (result == null)
            {
                return new ResponseDTO(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, "Update Service status Failed");
            }

            return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, "Update Service status Succeed");
        }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
    }
}

        public async Task<ResponseDTO> GetServiceByNameAsync(string serviceName)
        {
            try
            {
                
                var services = await _unitOfWork.HairServiceRepository.GetByNameAsync(serviceName);

               
                if (services == null )
                {
                    return new ResponseDTO(Const.SUCCESS_CREATE_CODE, "No users found with the given name.");
                }

                
                var result = _mapper.Map<List<ServicesDTO>>(services);

                return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

    }


}


        
   

