using Microsoft.AspNetCore.Mvc;
using Service.IService;

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
        public async Task<IActionResult> GetListSchedule(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _scheduleService.GetListScheduleAsync(pageNumber, pageSize);
            return Ok(result);
        }
    }
}
