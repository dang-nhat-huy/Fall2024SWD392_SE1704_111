using BusinessObject.ResponseDTO;
using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using static BusinessObject.RequestDTO.RequestDTO;
using Microsoft.AspNetCore.Authorization;
using Service.Service;

namespace Fall2024__SWD392_SE1704_111.Controllers
{
    [Route("api/v1/schedule")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet("scheduleList")]
        public async Task<IActionResult> GetListSchedule()
        {
            var result = await _scheduleService.GetListScheduleAsync();
            return Ok(result);
        }
        [Authorize(Roles = "Manager")]
        [HttpPost("createSchedule")]
        public async Task<IActionResult> CreateSchedule([FromBody] CreateScheduleDTO request)
        {
            // Kiểm tra xem request có hợp lệ không
            if (request == null)
            {
                return BadRequest(new ResponseDTO(Const.FAIL_READ_CODE, "Invalid request."));
            }

            // Gọi service để tạo report
            var response = await _scheduleService.CreateSchedule(request);

            // Kiểm tra kết quả và trả về phản hồi phù hợp
            if (response.Status != Const.SUCCESS_READ_CODE)
            {
                return BadRequest(response); // Trả về mã lỗi 400 với thông báo lỗi từ ResponseDTO
            }

            return Ok(response); // Trả về mã 200 nếu cập nhật thành công với thông tin trong ResponseDTO
        }

        [Authorize(Roles = "Manager")]
        [HttpPost("update/{scheduleId}")]
        public async Task<IActionResult> UpdateSchedule([FromBody] UpdateScheduleDTO request, [FromRoute] int scheduleId)
        {
            // Kiểm tra xem request có hợp lệ không
            if (request == null)
            {
                return BadRequest(new ResponseDTO(Const.FAIL_READ_CODE, "Invalid request."));
            }

            // Gọi service để tạo report
            var response = await _scheduleService.UpdateSchedule(request, scheduleId);

            // Kiểm tra kết quả và trả về phản hồi phù hợp
            if (response.Status != Const.SUCCESS_READ_CODE)
            {
                return BadRequest(response); // Trả về mã lỗi 400 với thông báo lỗi từ ResponseDTO
            }

            return Ok(response); // Trả về mã 200 nếu cập nhật thành công với thông tin trong ResponseDTO
        }

        [Authorize(Roles = "Manager")]
        [HttpPost("remove/{scheduleId}")]
        public async Task<IActionResult> RemoveSchedule([FromRoute] int scheduleId, [FromBody] RemoveScheduleDTO request)
        {
            // Kiểm tra xem request có hợp lệ không
            if (request == null)
            {
                return BadRequest(new ResponseDTO(Const.FAIL_READ_CODE, "Invalid request."));
            }

            // Gọi service để tạo report
            var response = await _scheduleService.DeleteSchedule(request, scheduleId);

            // Kiểm tra kết quả và trả về phản hồi phù hợp
            if (response.Status != Const.SUCCESS_READ_CODE)
            {
                return BadRequest(response); // Trả về mã lỗi 400 với thông báo lỗi từ ResponseDTO
            }

            return Ok(response); // Trả về mã 200 nếu cập nhật thành công với thông tin trong ResponseDTO
        }
        [Authorize(Roles = "Manager")]
        [HttpGet("PagingScheduleList")]
        public async Task<IActionResult> GetVoucherPaging([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            // Gọi service để lấy danh sách người dùng
            var response = await _scheduleService.GetAllSchedulePagingAsync(pageNumber, pageSize);

            // Trả về phản hồi
            if (response == null)
            {
                return BadRequest(response); // Trả về mã lỗi nếu không thành công
            }

            return Ok(response); // Trả về mã 200 nếu thành công
        }
        [HttpGet("GetScheduleById/{scheduleId}")]
        public async Task<IActionResult> GetScheduleById([FromRoute] int scheduleId)
        {

            var response = await _scheduleService.GetScheduleByIdAsync(scheduleId);

            // Trả về phản hồi
            if (response.Status != Const.SUCCESS_READ_CODE)
            {
                return BadRequest(response); // Trả về mã lỗi nếu không thành công
            }

            return Ok(response); // Trả về mã 200 nếu thành công
        }
        [Authorize(Roles = "Manager")]
        [HttpPost("changeScheduleStatus/{scheduleId}")]
        public async Task<IActionResult> ChangeScheduleStatus([FromRoute] int scheduleId)
        {
            var response = await _scheduleService.ChangeStatusScheduleById(scheduleId);

            if (response.Status != Const.SUCCESS_UPDATE_CODE)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
