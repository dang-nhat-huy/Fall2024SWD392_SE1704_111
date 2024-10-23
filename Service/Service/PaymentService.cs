using BusinessObject;
using BusinessObject.RequestDTO;
using BusinessObject.ResponseDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.RequestDTO.RequestDTO;

namespace Service.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _config;

        public PaymentService(IConfiguration config)
        {
            _config = config;
        }

        public string CreatePaymentUrl(HttpContext httpContext, VnPaymentRequestModel model)
        {
            // Lấy thông tin cấu hình từ appsettings.json
            string vnp_ReturnUrl = _config["VnPay:PaymentBackReturnUrl"];
            string vnp_Url = _config["VnPay:BaseURL"];
            string vnp_TmnCode = _config["VnPay:TmnCode"];
            string vnp_HashSecret = _config["VnPay:HashSecret"];

            VnPayLibrary vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", _config["VnPay:Version"]);
            vnpay.AddRequestData("vnp_Command", _config["VnPay:Command"]);
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (model.TotalPrice * 100).ToString()); // Số tiền thanh toán cần nhân với 100
            vnpay.AddRequestData("vnp_CurrCode", _config["VnPay:CurrCode"]);
            vnpay.AddRequestData("vnp_TxnRef", model.BookingId.ToString());
            vnpay.AddRequestData("vnp_OrderInfo", model.Description);
            vnpay.AddRequestData("vnp_OrderType", "other"); // Loại đơn hàng (có thể thay đổi)
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_ReturnUrl);
            vnpay.AddRequestData("vnp_IpAddr", httpContext.Connection.RemoteIpAddress?.ToString());
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));

            // Thông tin hóa đơn
            vnpay.AddRequestData("vnp_Bill_FullName", model.FullName);
            vnpay.AddRequestData("vnp_ExpireDate", DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss"));

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return paymentUrl;
        }

        public VnPayResponseModel PaymentExecute(IQueryCollection query)
        {
            VnPayLibrary vnpay = new VnPayLibrary();
            string vnp_HashSecret = _config["VnPay:HashSecret"];
            var response = vnpay.GetFullResponseData(query, vnp_HashSecret);
            return response;
        }
    }
}
    