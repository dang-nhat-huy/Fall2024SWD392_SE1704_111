using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.ResponseDTO;

namespace BusinessObject
{
    public class VnPayLibrary
    {
        private SortedList<string, string> _requestData = new SortedList<string, string>();
        private SortedList<string, string> _responseData = new SortedList<string, string>();

        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
        }

        public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
        {
            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in _requestData)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    data.Append(kv.Key + "=" + kv.Value + "&");
                }
            }

            string rawData = data.ToString().TrimEnd('&');
            string vnp_SecureHash = HmacSHA512(vnp_HashSecret, rawData); // Tạo mã băm
            string paymentUrl = baseUrl + "?" + rawData + "&vnp_SecureHash=" + vnp_SecureHash;

            return paymentUrl;
        }

        public VnPayResponseModel GetFullResponseData(IQueryCollection query, string vnp_HashSecret)
        {
            foreach (var (key, value) in query)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    _responseData.Add(key, value);
                }
            }

            string vnp_SecureHash = query["vnp_SecureHash"];
            if (!string.IsNullOrEmpty(vnp_SecureHash))
            {
                string rawData = string.Join("&", _responseData.Select(kv => kv.Key + "=" + kv.Value));
                string checkSignature = HmacSHA512(vnp_HashSecret, rawData);

                // So sánh mã hash từ phản hồi với mã hash tạo ra từ secret key
                if (checkSignature.Equals(vnp_SecureHash, StringComparison.InvariantCultureIgnoreCase))
                {
                    return new VnPayResponseModel
                    {
                        VnPayResponseCode = _responseData["vnp_ResponseCode"],
                        Message = "Success",
                        TransactionId = _responseData["vnp_TransactionNo"],
                        BookingId = _responseData["vnp_TxnRef"]
                    };
                }
                else
                {
                    return new VnPayResponseModel
                    {
                        VnPayResponseCode = "97", // Mã lỗi không hợp lệ
                        Message = "Invalid signature"
                    };
                }
            }

            return null;
        }

        private string HmacSHA512(string key, string inputData)
        {
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputData));
                return BitConverter.ToString(hashValue).Replace("-", "").ToLower();
            }
        }
    }
}