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



        public async Task<ResponseDTO> CreateServiceAsync(CreateServiceDTO request)
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




        public async Task<ResponseDTO> UpdateServiceAsync(RequestDTO.UpdateServiceDTO request, int serviceId)
        {
            try
            {
                var service = await _unitOfWork.HairServiceRepository.GetByIdAsync(serviceId);
                if (service == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "Service not found");
                }

                // Map the updated values to the existing service object
                _mapper.Map(request, service);

                service.UpdateDate = DateTime.Now;

                // Save the changes to the database
                var result = await _unitOfWork.HairServiceRepository.UpdateAsync(service);
                if (result == null)
                {
                    return new ResponseDTO(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, "Update Service Failed");
                }

                return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, "Update Service status Succeeded");
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message, ex);
            }
        }

        public async Task<ResponseDTO> ChangeServiceStatusAsync(RequestDTO.RemoveServiceDTO request, int servicetId)
        {
            try
            {
                
                var service = await _unitOfWork.HairServiceRepository.GetByIdAsync(servicetId);
                if (service == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, "Hair service not found !");
                }

                // Sử dụng AutoMapper để ánh xạ thông tin từ DTO vào user

                service.Status = request.Status;
                service.UpdateDate = DateTime.Now;

                // Lưu các thay đổi vào cơ sở dữ liệu
                await _unitOfWork.HairServiceRepository.UpdateAsync(service);

                return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, "Change Status Succeed");
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
                
                var services = await _unitOfWork.HairServiceRepository.GetServiceByNameAsync(serviceName);

               
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


        
   

