using Microsoft.AspNetCore.Mvc;
using Service.IService;
using Service.Service;

namespace Fall2024__SWD392_SE1704_111.Controllers
{
    public class ScheduleUserController : ControllerBase
    {
        private readonly IScheduleUserService _scheduleUserService;

        public ScheduleUserController(IScheduleUserService scheduleUserService)
        {
            _scheduleUserService = scheduleUserService;
        }

        [HttpGet("scheduleUserList")]
        public async Task<IActionResult> GetListSchedule(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _scheduleUserService.GetListScheduleUserAsync(pageNumber, pageSize);
            return Ok(result);
        }
    }
}
