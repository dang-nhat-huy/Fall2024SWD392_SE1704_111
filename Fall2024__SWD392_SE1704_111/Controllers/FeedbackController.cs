using BusinessObject;
using BusinessObject.ResponseDTO;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;
using Service.IService;
using static BusinessObject.RequestDTO.RequestDTO;

namespace Fall2024__SWD392_SE1704_111.Controllers
{
    [Route("api/v1/feedback")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }
        [HttpGet("feedbackList")]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            try
            {
                var response = await _feedbackService.GetAllFeedbacksAsync();
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
        [HttpPost("CreateFeedback")]
        public async Task<IActionResult> Feedback([FromBody] FeedbackRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest(new ResponseDTO(Const.FAIL_READ_CODE, "Invalid request."));
            }
            var response = await _feedbackService.CreateFeedback(request);
            if (response.Status != Const.SUCCESS_CREATE_CODE)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpDelete("DeleteFeedback")]
        public async Task<IActionResult> DeleteFeedback(int feedbackId)
        {
            try
            {
                var result = await _feedbackService.DeleteFeedbackAsync(feedbackId);
                if (result)
                {
                    return Ok(new { message = "Feedback Deleted." });
                }
                else
                {
                    return NotFound(new { message = "Feedback Not Found" });
                }
            }
            catch (Exception ex)
            {
                // Ghi log exception để debug
                return StatusCode(500, $"Đã xảy ra lỗi: {ex.Message}");
            }
        }



    }
}
