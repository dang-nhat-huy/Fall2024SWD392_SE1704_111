using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.Model;
using BusinessObject.ResponseDTO;
using static BusinessObject.RequestDTO.RequestDTO;
using Newtonsoft.Json;
using System.Text;

namespace Fall2024_SWD392_SE1704_111_FE.Pages.FeedbackFE
{
    public class CreateModel : PageModel
    {
       
        [BindProperty]
        public Feedback Feedback { get; set; } = default!;
        [BindProperty]
        public CreateFeedbackDTO createDto { get; set; } = default!;
        public ResponseDTO dto { get; set; } = null!;

       

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                // Kiểm tra ModelState có hợp lệ không
                if (!ModelState.IsValid)
                {
                    TempData["error"] = "Invalid Input";
                    return Page();
                }

                // Lấy JWT token từ cookie
                string? jwt = Request.Cookies["jwt"];
                if (string.IsNullOrEmpty(jwt))
                {
                    TempData["errorLogin"] = "You need to login to access this page";
                    return RedirectToPage("../Login");
                }

                // Serialize CreateAccountDTO thành JSON
                string jsonRequest = JsonConvert.SerializeObject(createDto);
                string url = "https://localhost:7211/api/v1/feedbacks/createFeedback"; // Đường dẫn đến API

                // Tạo HttpClient để gửi yêu cầu
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

                // Tạo yêu cầu HTTP POST với JSON content
                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Post,
                    Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json")
                };

                // Gửi yêu cầu đến API
                HttpResponseMessage response = await client.SendAsync(request);

                // Kiểm tra phản hồi từ API
                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "Feedback created successfully";
                    var role = HttpContext.Session.GetString("Role");
                    if (role == "Manager")
                    {
                        return RedirectToPage("./Index");
                    }
                    else
                    {
                        return RedirectToPage("/login");
                    }
                }
                else
                {
                    // Lấy nội dung lỗi từ API nếu có
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    TempData["error"] = $"Error: {errorResponse}";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi trong quá trình gửi yêu cầu
                TempData["error"] = $"An error occurred: {ex.Message}";
                return Page();
            }
        }
    }
}
