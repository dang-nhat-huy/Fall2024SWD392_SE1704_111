using BusinessObject;
using BusinessObject.ResponseDTO;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using Service.Service;
using static BusinessObject.RequestDTO.RequestDTO;

namespace Fall2024__SWD392_SE1704_111.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class ScheduleUserController : ControllerBase
    {
        private readonly IScheduleUserService _scheduleUserService;

        public ScheduleUserController(IScheduleUserService scheduleUserService)
        {
            _scheduleUserService = scheduleUserService;
        }

        [HttpGet("getAllStylistSchedules")]
        public async Task<IActionResult> GetAllStylistSchedulesAsync()
        {
            var result = await _scheduleUserService.GetListScheduleUserAsync();
            return Ok(result);
        }

        [HttpGet("currentStylistSchedule")]
        public async Task<IActionResult> GetScheduleUserOfCurrentUser()
        {
            try
            {
                var response = await _scheduleUserService.GetScheduleUserOfCurrentUser();

                if (response.Status == Const.SUCCESS_READ_CODE)
                {
                    return Ok(response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }

        [HttpPost("createScheduleUser")]
        public async Task<IActionResult> CreateScheduleUser([FromBody] createScheduleUser createScheduleUser)
        {

            try
            {
                var result = await _scheduleUserService.createScheduleUser(createScheduleUser);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        [HttpGet("StylistSchedule")]
        public async Task<IActionResult> GetScheduleUserOfStylist()
        {
            try
            {
                var response = await _scheduleUserService.GetSchedulesOfStylistsAsync();

                if (response.Status == Const.SUCCESS_READ_CODE)
                {
                    return Ok(response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }

        [HttpPost("StylistByDate")]
        public async Task<IActionResult> GetStylistByStartTimeAndStartDate([FromBody]getStartDateAndStartTime request)
        {
            try
            {
                var response = await _scheduleUserService.GetStylistsByScheduleAsync(request);

                if (response.Status == Const.SUCCESS_READ_CODE)
                {
                    return Ok(response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }

        [HttpGet("ScheduleOfNoStylist")]
        public async Task<IActionResult> GetScheduleUserOfNoStylist()
        {
            try
            {
                var response = await _scheduleUserService.GetSchedulesOfNoStylistAssignAsync();

                if (response.Status == Const.SUCCESS_READ_CODE)
                {
                    return Ok(response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }

        [HttpPut("AssignStylistToSchedule/{scheduleId}")]
        public async Task<IActionResult> UpdateScheduleUser(int scheduleId)
        {
            try
            {
                // Gọi service để cập nhật ScheduleUser
                var response = await _scheduleUserService.UpdateScheduleUserAsync(scheduleId);

                // Kiểm tra kết quả từ service và trả về phản hồi tương ứng
                if (response.Status == Const.SUCCESS_UPDATE_CODE)
                {
                    return Ok(response); // Trả về HTTP 200 với kết quả
                }
                else if (response.Status == Const.FAIL_READ_CODE)
                {
                    return NotFound(response); // Trả về HTTP 404 nếu không tìm thấy lịch
                }
                else
                {
                    return BadRequest(response); // Trả về HTTP 400 cho các lỗi khác
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ và trả về HTTP 500
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message));
            }
        }

        [HttpGet("currentStylistSchedules")]
        public async Task<IActionResult> GetSchedulesOfCurrentStylist()
        {
            try
            {
                // Gọi service để lấy lịch của Stylist hiện tại
                var response = await _scheduleUserService.GetSchedulesOfCurrentStylistAsync();

                // Kiểm tra kết quả trả về từ service
                if (response.Status == Const.SUCCESS_READ_CODE)
                {
                    return Ok(response);  // Trả về danh sách lịch nếu thành công
                }
                else
                {
                    return BadRequest(response);  // Trả về lỗi nếu không thành công
                }
            }
            catch (Exception ex)
            {
                // Trả về lỗi khi xảy ra ngoại lệ
                return StatusCode(500, new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message));
            }
        }

    }
}
