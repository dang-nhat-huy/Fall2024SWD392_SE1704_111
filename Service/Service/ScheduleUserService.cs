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
using static BusinessObject.RequestDTO.RequestDTO;
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

        public async Task<ResponseDTO> createScheduleUser(createScheduleUser request)
        {
            try
            {
                // Lấy người dùng hiện tại
                var user = await _jWTService.GetCurrentUserAsync();
                if (user == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "User not found.");
                }

                //// Kiểm tra xem UserId có phải là Stylist không
                //var targetUser = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
                //if (targetUser == null)
                //{
                //    return new ResponseDTO(Const.FAIL_READ_CODE, "User not found.");
                //}

                //// Kiểm tra xem Schedule đã tồn tại chưa (dựa trên StartDate, StartTime, EndDate, EndTime)
                //var existingSchedule = await _unitOfWork.ScheduleRepository
                //    .GetAll()  // Hoặc phương thức nào để lấy tất cả Schedule
                //    .FirstOrDefaultAsync(s => s.StartDate == request.StartDate && s.StartTime == request.StartTime && s.EndDate == request.EndDate && s.EndTime == request.EndTime);

                //if (existingSchedule != null)
                //{
                //    return new ResponseDTO(Const.FAIL_READ_CODE, "Schedule already exists.");
                //}

                // Tạo Schedule mới từ DTO request
                var schedule = new Schedule
                {
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    Status = ScheduleEnum.Available,  // Giả sử trạng thái ban đầu là Available
                    CreateBy = user.UserName,
                    CreateDate = DateTime.Now
                };

                // Lưu Schedule vào cơ sở dữ liệu
                var createdSchedule = await _unitOfWork.ScheduleRepository.CreateAsync(schedule);

                var existingScheduleUser = await _unitOfWork.ScheduleUserRepository
                    .GetAll()
                    .FirstOrDefaultAsync(su => su.UserId == request.UserId &&
                    su.Schedule.StartTime == request.StartTime && su.Schedule.EndTime == request.EndTime && 
                    su.Schedule.StartDate == request.StartDate && su.Schedule.EndDate == request.EndDate);

                if (existingScheduleUser != null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "Schedule user is already assigned to this schedule.");
                }


                var scheduleUser = new ScheduleUser
                {
                    UserId = request.UserId,
                    ScheduleId = schedule.ScheduleId,
                    Status = ScheduleUserEnum.Assign,  // Giả sử trạng thái ban đầu là Assigned
                    CreateBy = user.UserName,
                    CreateDate = DateTime.Now
                };

                // Lưu ScheduleUser vào cơ sở dữ liệu
                await _unitOfWork.ScheduleUserRepository.CreateAsync(scheduleUser);
                return new ResponseDTO(Const.SUCCESS_CREATE_CODE, "create schedule stylist successfully", request);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
    }
}
