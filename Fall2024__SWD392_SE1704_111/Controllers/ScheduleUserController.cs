using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using Service.Service;

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
    }
}
