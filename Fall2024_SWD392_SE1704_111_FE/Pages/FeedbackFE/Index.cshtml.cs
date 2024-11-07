using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Model;
using BusinessObject.ResponseDTO;
using Newtonsoft.Json;

namespace Fall2024_SWD392_SE1704_111_FE.Pages.FeedbackFE
{
    public class IndexModel : PageModel
    {
        public IList<Feedback> Feedback { get; set; } = null!;

        public PagedResult<Feedback> dto { get; set; } = null!;

        [BindProperty(SupportsGet = true)]
        public int Index { get; set; } = 1;
        public double Count { get; set; }
        [BindProperty]
        public string? searchValue { get; set; } = null!;
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                int size = 5;

                // Nếu không có giá trị tìm kiếm, chuyển hướng đến Index để hiển thị toàn bộ phản hồi
                if (string.IsNullOrEmpty(searchValue))
                {
                    return RedirectToPage("./Index");
                }

                // Xây dựng URL API để tìm kiếm feedback theo description
                string url = $"https://localhost:7211/api/v1/feedbacks/SearchByDescription?query={searchValue}&pageNumber={Index}&pageSize={size}";

                // Lấy JWT từ cookie để thực hiện xác thực
                string? jwt = Request.Cookies["jwt"];
                if (jwt == null)
                {
                    TempData["errorLogin"] = "You need to login to access this page";
                    return RedirectToPage("../Login");
                }

                // Thiết lập HttpClient với Authorization Header
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

                // Tạo yêu cầu HTTP GET
                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get
                };

                // Gửi yêu cầu và nhận phản hồi
                HttpResponseMessage response = await client.SendAsync(request);

                // Kiểm tra nếu phản hồi thành công
                if (response.IsSuccessStatusCode)
                {
                    // Đọc và chuyển đổi JSON response thành đối tượng PagedResult
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var dto = JsonConvert.DeserializeObject<PagedResult<Feedback>>(jsonResponse);

                    // Gán Feedback với danh sách feedback từ API hoặc danh sách trống nếu không có kết quả
                    Feedback = dto?.Items ?? new List<Feedback>();

                    // Tính toán số trang dựa trên tổng số phản hồi
                    Count = Math.Ceiling((double)(dto?.TotalCount ?? 0) / size);

                    return Page();
                }
                else
                {
                    // Đặt thông báo lỗi nếu API không thành công
                    TempData["errorList"] = "Error retrieving search results";
                    Feedback = new List<Feedback>();  // Gán danh sách trống nếu có lỗi
                }

                return Page();
            }
            catch (Exception)
            {
                TempData["errorList"] = "An error occurred while processing your request. Please try again later";
                Feedback = new List<Feedback>();  // Gán danh sách trống khi gặp lỗi ngoại lệ
                return Page();
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var size = 5;
                string url = "https://localhost:7211/api/v1/feedbacks/PagingFeedbackList1?pageNumber=" + Index + "&pageSize=" + size;

                string? jwt = Request.Cookies["jwt"]?.ToString();
                if (jwt == null)
                {
                    TempData["errorLogin"] = "You need to login to access this page";
                    return RedirectToPage("../Login");
                }

                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(url),
                };
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var role = HttpContext.Session.GetString("Role");
                    if (role == "Manager")
                    {
                        // Lấy thông tin ID của người dùng hiện tại từ token hoặc session
                        var currentUserId = HttpContext.Session.GetString("UserId");

                        // Lấy danh sách phản hồi từ API nếu token hợp lệ
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var dto = JsonConvert.DeserializeObject<PagedResult<Feedback>>(jsonResponse)!;

                        // Deserialize `dto.Data` 
                        var feedbackListJson = JsonConvert.SerializeObject(dto.Items);
                        Feedback = JsonConvert.DeserializeObject<IList<Feedback>>(feedbackListJson)!;

                        // Phân trang cho danh sách phản hồi
                        var countJson = JsonConvert.SerializeObject(dto.TotalCount);
                        var count = JsonConvert.DeserializeObject<int>(countJson);
                        Count = Math.Ceiling((double)count / size);

                        return Page();  // Trả về Razor Page với danh sách phản hồi
                    }
                    else
                    {
                        // Nếu không phải Manager, có thể trả về trang lỗi hoặc chuyển hướng
                        return RedirectToPage("/login");
                    }
                }
                else
                {
                    TempData["errorList"] = "Get List Error";
                    return Page();
                }
            }
            catch (Exception)
            {
                TempData["errorList"] = "An error occurred while processing your request. Please try again later";
                return Page();
            }
        }
    }
}
