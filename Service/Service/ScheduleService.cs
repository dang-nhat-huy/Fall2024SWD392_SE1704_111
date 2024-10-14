using AutoMapper;
using BusinessObject;
using BusinessObject.Model;
using BusinessObject.Paging;
using BusinessObject.RequestDTO;
using BusinessObject.ResponseDTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.ResponseDTO.ResponseDTO;

namespace Service.Service
{
    public class ScheduleService : IScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public ScheduleService(IUnitOfWork unitOfWork, IConfiguration config, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _mapper = mapper;
        }

       
        public IQueryable<Schedule> GetListSchedule()
        {
            var query = _unitOfWork.ScheduleRepository.GetAll();
            return query;
        }
        public async Task<ResponseDTO> GetListScheduleAsync()
        {
            try
            {
                var query = await _unitOfWork.ScheduleRepository.GetAllWithTwoInclude("Bookings", "ScheduleUsers").ToListAsync();

                if (query == null || !query.Any())
                {
                    return new ResponseDTO(Const.SUCCESS_CREATE_CODE, "Empty List");
                }

                // Sử dụng AutoMapper để ánh xạ danh sách
                var result = _mapper.Map<List<ScheduleDTO>>(query);

                return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }
            catch (Exception e)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, e.Message);
            }
        }
        public async Task<ResponseDTO> CreateSchedule(RequestDTO.CreateScheduleDTO request)
        {
            try
            {
                // Sử dụng AutoMapper 
                var schedule = _mapper.Map<Schedule>(request);


                schedule.CreateDate = DateTime.Now;
                schedule.UpdateDate = DateTime.Now;

                // Lưu các thay đổi vào cơ sở dữ liệu
                int result = await _unitOfWork.ScheduleRepository.CreateScheduleAsync(schedule);

                return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }


            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message, ex);
            }
        }

        public async Task<ResponseDTO> DeleteSchedule(RequestDTO.RemoveScheduleDTO request, int scheduleId)
        {
            try
            {

                var schedule = await _unitOfWork.ScheduleRepository.GetByIdAsync(scheduleId);
                if (schedule == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, "Schedule not found !");
                }

                // Sử dụng AutoMapper để ánh xạ thông tin từ DTO vào user

                schedule.Status = (ScheduleEnum?)request.Status;
                schedule.UpdateDate= DateTime.Now;

                // Lưu các thay đổi vào cơ sở dữ liệu
                await _unitOfWork.ScheduleRepository.UpdateAsync(schedule);

                return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, "Change Status Succeed");
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }


        public async Task<ResponseDTO> UpdateSchedule(RequestDTO.UpdateScheduleDTO request, int scheduleId)
        {
            try
            {
                var schedule = await _unitOfWork.ScheduleRepository.GetByIdAsync(scheduleId);
                if (schedule == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "Schedule not found");
                }

                // Map the updated values to the existing service object
                _mapper.Map(request, schedule);

                schedule.UpdateDate = DateTime.Now;

                // Save the changes to the database
                var result = await _unitOfWork.ScheduleRepository.UpdateAsync(schedule);
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
    }
}
