using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Model;
using BusinessObject.ResponseDTO;
using Newtonsoft.Json;
using static BusinessObject.RequestDTO.RequestDTO;
using System.Text;

namespace Fall2024_SWD392_SE1704_111_FE.Pages.UserFE
{
    public class EditModel : PageModel
    {

        [BindProperty]
        public User User { get; set; } = default!;
        [BindProperty]
        public UpdateAccountDTO updateDto { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var role = HttpContext.Session.GetString("Role");
                string? jwt = Request.Cookies["jwt"];
                if (role == null || jwt == null)
                {
                    TempData["errorLogin"] = "You need to login to access this page";
                    return RedirectToPage("../Logout");
                }
                if (role != null && !role.Equals("Admin"))
                {
                    TempData["error"] = "You are not authorized to access this page";
                    return RedirectToPage("../logout");
                }

                jwt = jwt.ToString();
                string url = "https://localhost:7211/api/v1/users/GetUserById/" + id;
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");
                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get
                };
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var dto = JsonConvert.DeserializeObject<ResponseDTO>(jsonResponse)!;

                    // Deserialize `dto.Data` to `List<UserListDTO>`
                    var usersListJson = JsonConvert.SerializeObject(dto.Data);
                    User = JsonConvert.DeserializeObject<User>(usersListJson);
                }
                else
                {
                    TempData["error"] = "Error Getting Data";
                }
                return Page();
            }
            catch (Exception)
            {
                TempData["error"] = "An error occurred while processing your request. Please try again later";
                return Page();
            }
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.UserId;

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
                string jsonRequest = JsonConvert.SerializeObject(updateDto);
                string url = "https://localhost:7211/api/v1/users/updateAccount/" + userId; // Đường dẫn đến API

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

                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "Change Successfully";
                    return RedirectToPage("./Index");
                }
                else
                {
                    TempData["error"] = "Error Getting Data";
                }
                return Page();
            }
            catch (Exception)
            {
                TempData["error"] = "An error occurred while processing your request. Please try again later";
                return Page();
            }
        }

        //private bool UserExists(int id)
        //{
        //    return Page();
        //}
    }
}
