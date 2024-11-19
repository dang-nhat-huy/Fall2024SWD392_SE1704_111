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

                // Kiểm tra xem Schedule đã tồn tại chưa (dựa trên StartDate, StartTime, EndDate, EndTime)
                var existingSchedule = await _unitOfWork.ScheduleRepository
                    .GetAll()
                    .FirstOrDefaultAsync(s => s.StartDate == request.StartDate && s.StartTime == request.StartTime &&
                                              s.EndDate == request.EndDate && s.EndTime == request.EndTime);

                Schedule schedule;
                if (existingSchedule != null)
                {
                    schedule = existingSchedule;
                }
                else
                {
                    // Tạo Schedule mới từ DTO request
                    schedule = new Schedule
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
                    var createSchedule = await _unitOfWork.ScheduleRepository.CreateAsync(schedule);
                }

                if (request.UserId != null)
                {
                    // Kiểm tra UserId có hợp lệ hay không
                    var targetUser = await _unitOfWork.UserRepository.GetUserByIdAsync(request.UserId);
                    if (targetUser == null)
                    {
                        return new ResponseDTO(Const.FAIL_READ_CODE, "User not found.");
                    }

                    // Kiểm tra nếu vai trò của người dùng không phải là Stylist
                    if (targetUser.Role != UserRole.Stylist)
                    {
                        return new ResponseDTO(Const.FAIL_READ_CODE, "The user is not a Stylist.");
                    }

                    // Kiểm tra xem ScheduleUser đã tồn tại chưa
                    var existingScheduleUser = await _unitOfWork.ScheduleUserRepository
                        .GetAll()
                        .FirstOrDefaultAsync(su => su.UserId == request.UserId &&
                                                   su.Schedule.StartTime == request.StartTime &&
                                                   su.Schedule.EndTime == request.EndTime &&
                                                   su.Schedule.StartDate == request.StartDate &&
                                                   su.Schedule.EndDate == request.EndDate);

                    if (existingScheduleUser != null)
                    {
                        return new ResponseDTO(Const.FAIL_READ_CODE, "Schedule user is already assigned to this schedule.");
                    }
                }

                // Tạo mới ScheduleUser với UserId có thể null
                var scheduleUser = new ScheduleUser
                {
                    UserId = request.UserId != 0 ? request.UserId : null, // UserId để trống nếu không được nhập
                    ScheduleId = schedule.ScheduleId,
                    Status = ScheduleUserEnum.Assign,  // Giả sử trạng thái ban đầu là Assigned
                    CreateBy = user.UserName,
                    CreateDate = DateTime.Now
                };

                // Lưu ScheduleUser vào cơ sở dữ liệu
                await _unitOfWork.ScheduleUserRepository.CreateAsync(scheduleUser);
                return new ResponseDTO(Const.SUCCESS_CREATE_CODE, "create schedule stylist successfully", schedule);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<ResponseDTO> GetSchedulesOfStylistsAsync()
        {
            try
            {
                // Gọi phương thức từ repository
                var scheduleUsers = await _unitOfWork.ScheduleUserRepository.GetScheduleUsersOfStylistsAsync();

                // Ánh xạ dữ liệu sang DTO
                var result = scheduleUsers.Select(su => new viewScheduleOfStylist
                {
                    FullName = su.User?.UserProfile.FullName,
                    StartTime = su.Schedule?.StartTime,
                    EndTime = su.Schedule?.EndTime,
                    StartDate = su.Schedule?.StartDate,
                    EndDate = su.Schedule?.EndDate,
                    Status = su.Schedule?.Status
                }).ToList();

                // Trả về kết quả
                return new ResponseDTO(Const.SUCCESS_READ_CODE, "Successfully retrieved schedule list.", result);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

    }
}
