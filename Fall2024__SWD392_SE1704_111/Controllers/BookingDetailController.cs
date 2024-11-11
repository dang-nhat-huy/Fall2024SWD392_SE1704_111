using BusinessObject;
using BusinessObject.ResponseDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using Service.Service;
using static BusinessObject.ResponseDTO.ReportDTO;

namespace Fall2024__SWD392_SE1704_111.Controllers
{
    [Route("api/v1/booking")]
    [ApiController]
    public class BookingDetailController : ControllerBase
    {
        private readonly IBookingDetailService _bookingDetailService;

        public BookingDetailController(IBookingDetailService bookingDetailService)
        {
            _bookingDetailService = bookingDetailService;

        }

        [Authorize]
        [HttpGet("{bookingDetailID}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookingDetailResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBookingDetail(int bookingDetailID)
        {
            var bookingDetail = await _bookingDetailService.GetBookingDetailByIdAsync(bookingDetailID);
            if (bookingDetail == null)
            {
                return NotFound();
            }
            return Ok(bookingDetail);
        }

        [HttpGet("bookingOfStylist")]
        public async Task<IActionResult> GetBookingHistoryOfCurrentUser()
        {
            try
            {
                var response = await _bookingDetailService.GetBookingOfCurrentStylist();

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
