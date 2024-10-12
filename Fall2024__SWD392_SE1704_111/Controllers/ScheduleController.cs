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
        public async Task<IActionResult> GetListSchedule()
        {
            var result = await _scheduleService.GetListScheduleAsync();
            return Ok(result);
        }
    }
}
