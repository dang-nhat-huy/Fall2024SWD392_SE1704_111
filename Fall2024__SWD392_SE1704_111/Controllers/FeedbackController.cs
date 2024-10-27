using BusinessObject;
using BusinessObject.ResponseDTO;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;
using Service.IService;
using static BusinessObject.RequestDTO.RequestDTO;

namespace Fall2024__SWD392_SE1704_111.Controllers
{


    [Route("api/v1/feedbacks")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        //[Authorize(Roles = "Customer, Manager")]
        [HttpPost("createFeedback")]
        public async Task<IActionResult> CreateFeedback([FromBody] FeedbackRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest(new ResponseDTO(Const.FAIL_READ_CODE, "Invalid request."));
            }

            var response = await _feedbackService.CreateFeedbackAsync(request);

            if (response.Status != Const.SUCCESS_CREATE_CODE)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        //[Authorize(Roles = "Customer, Manager")]
        [HttpPost("updateFeedback/{feedbackId}")]
        public async Task<IActionResult> UpdateFeedback([FromBody] FeedbackRequestDTO request, [FromRoute] int feedbackId)
        {
            if (request == null)
            {
                return BadRequest(new ResponseDTO(Const.FAIL_READ_CODE, "Invalid request."));
            }

            var response = await _feedbackService.UpdateFeedbackAsync(request, feedbackId);

            if (response.Status != Const.SUCCESS_UPDATE_CODE)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        //[Authorize(Roles = "Manager")]
        [HttpPost("changeFeedbackStatus/{feedbackId}")]
        public async Task<IActionResult> ChangeFeedbackStatus([FromRoute] int feedbackId, [FromBody] FeedbackStatusEnum newStatus)
        {
            var response = await _feedbackService.ChangeFeedbackStatusAsync(feedbackId, newStatus);

            if (response.Status != Const.SUCCESS_UPDATE_CODE)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        //[Authorize(Roles = "Customer, Manager")]
        [HttpGet("feedbackList")]
        public async Task<IActionResult> GetFeedbackList()
        {
            var response = await _feedbackService.GetFeedbackListAsync();

            if (response.Status != Const.SUCCESS_READ_CODE)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
