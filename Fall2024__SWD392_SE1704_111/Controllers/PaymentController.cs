using BusinessObject.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using static BusinessObject.RequestDTO.RequestDTO;

namespace Fall2024__SWD392_SE1704_111.Controllers
{
    [Route("api/v1/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;


        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("checkout")]
        public IActionResult CheckOut(Booking booking,string payment)
        {
            if (payment == "VNPay")
            {
                var vnPayModel = new VnPaymentRequestModel
                {
                    TotalPrice = booking.TotalPrice,
                    CreateDate = DateTime.Now,
                    Description = $"{booking.Customer.UserName} {booking.Customer.Phone}",
                    FullName = booking.Customer.UserName,
                    BookingId = booking.BookingId,
                };
                return Redirect(_paymentService.CreatePaymentUrl(HttpContext, vnPayModel));
            }
            return BadRequest();
        }

        [HttpGet("PaymentCallBack")]
        public IActionResult PaymentCallBack()
        {
            var response = _paymentService.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                return BadRequest();
            }
            return Ok("Success");
        }
    }
}
