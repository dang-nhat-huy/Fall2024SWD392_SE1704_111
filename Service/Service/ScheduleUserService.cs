using AutoMapper;
using BusinessObject;
using BusinessObject.Model;
using BusinessObject.Paging;
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
    public class ScheduleUserService : IScheduleUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJWTService _jWTService;

        public ScheduleUserService(IUnitOfWork unitOfWork, IMapper mapper, IJWTService jWTService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jWTService = jWTService;
        }

        public async Task<List<ScheduleUserDTO>> GetListScheduleUserAsync()
        {
            // Lấy IQueryable từ repository
            var listQuery = await _unitOfWork.ScheduleUserRepository.GetListScheduleByRoleAsync();

            // Sử dụng ProjectTo để ánh xạ thành List<ScheduleUserDTO>
            var resultList = await _mapper.ProjectTo<ScheduleUserDTO>(listQuery).ToListAsync();

            return resultList; // Trả về danh sách ScheduleUserDTO
        }

        public async Task<ResponseDTO> GetScheduleUserOfCurrentUser()
        {
            try
            {
                // Lấy người dùng hiện tại
                var user = await _jWTService.GetCurrentUserAsync();
                if (user == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "User not found.");
                }

                
                var scheduleUsers = await _unitOfWork.ScheduleUserRepository.GetScheduleUserByStylistIdAsync(user.UserId);
                if (scheduleUsers == null || scheduleUsers.Count == 0)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "No Schedule of Stylist found.");
                }

                
                List<ScheduleCurrentUserDTO> currentStylistScheduleDto = new List<ScheduleCurrentUserDTO>();

                foreach (var scheduleUser in scheduleUsers)
                {
                    //var dto = _mapper.Map<ScheduleCurrentUserDTO>(scheduleUser);
                    var scheduleResponse = _mapper.Map<ScheduledDetailDTO>(scheduleUser.Schedule);
                    var dto = new ScheduleCurrentUserDTO
                    {
                        ScheduleUserId = scheduleUser.ScheduleUserId,
                        Status = scheduleUser.Status,
                        Schedule = scheduleResponse
                    };
                    currentStylistScheduleDto.Add(dto);
                }
                
                //foreach (var schedule in currentStylistScheduleDto)
                //{
                //    schedule.Schedule = schedule.Schedule
                //        .GroupBy(s => s.ScheduleId)  
                //        .Select(g => g.First())     
                //        .ToList();

                //}
                return new ResponseDTO(Const.SUCCESS_READ_CODE, "Schedule of Stylist retrieved successfully.", currentStylistScheduleDto);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
    }
}
