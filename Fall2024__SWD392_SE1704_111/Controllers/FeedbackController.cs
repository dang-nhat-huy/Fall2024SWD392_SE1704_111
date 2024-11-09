using BusinessObject;
using BusinessObject.ResponseDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;
using Service.IService;
using Service.Service;
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

        [Authorize]
        [HttpPost("createFeedback")]
        public async Task<IActionResult> CreateFeedback([FromBody] CreateFeedbackDTO request)
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

        [Authorize]
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

        [Authorize]
        [HttpPost("changeFeedbackStatus/{feedbackId}")]
        public async Task<IActionResult> ChangeFeedbackStatus([FromRoute] int feedbackId)
        {
            var response = await _feedbackService.ChangeStatusFeedbackById(feedbackId);

            if (response.Status != Const.SUCCESS_UPDATE_CODE)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
        [Authorize]
        [HttpGet("PagingFeedbackList")]
        public async Task<IActionResult> GetVoucherPaging([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            // Gọi service để lấy danh sách người dùng
            var response = await _feedbackService.GetAllFeedbackPagingAsync(pageNumber, pageSize);

            // Trả về phản hồi
            if (response == null)
            {
                return BadRequest(response); // Trả về mã lỗi nếu không thành công
            }

            return Ok(response); // Trả về mã 200 nếu thành công
        } 
        [Authorize]
        [HttpGet("PagingFeedbackList1")]
        public async Task<IActionResult> GetVoucherPaging1([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            // Gọi service để lấy danh sách người dùng
            var response = await _feedbackService.GetAllFeedbackPagingAsync1(pageNumber, pageSize);

            // Trả về phản hồi
            if (response == null)
            {
                return BadRequest(response); // Trả về mã lỗi nếu không thành công
            }

            return Ok(response); // Trả về mã 200 nếu thành công
        }

        [Authorize]
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
        [Authorize]
        [HttpGet("GetFeedbackById/{feedbackId}")]
        public async Task<IActionResult> GetFeedbackById([FromRoute] int feedbackId)
        {

            var response = await _feedbackService.GetFeedbackByIdAsync(feedbackId);

            // Trả về phản hồi
            if (response.Status != Const.SUCCESS_READ_CODE)
            {
                return BadRequest(response); // Trả về mã lỗi nếu không thành công
            }

            return Ok(response); // Trả về mã 200 nếu thành công
        }
        [Authorize]
        [HttpGet("SearchByDescription")]
        public async Task<IActionResult> SearchFeedbackByDescription([FromQuery] string query, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            var response = await _feedbackService.SearchFeedbackByDescriptionAsync(query, pageNumber, pageSize);

            if (response == null || response.Items.Count == 0)
            {
                return NotFound(new ResponseDTO(Const.FAIL_READ_CODE, "No feedbacks found with the specified description."));
            }

            return Ok(response);
        }
    }
}
