using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;
using Service.IService;

namespace Fall2024__SWD392_SE1704_111.Controllers
{
    [Route("api/v1/feedback")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        public FeedbackController (IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }
        [HttpGet("feedbackList")]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            try
            {
                var response = await _feedbackService.GetAllFeedbacksAsync();
                if(response.Status == Const.SUCCESS_READ_CODE)
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
