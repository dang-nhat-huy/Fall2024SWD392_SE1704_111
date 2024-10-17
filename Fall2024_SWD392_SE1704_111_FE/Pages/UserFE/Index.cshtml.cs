using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Model;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using BusinessObject.ResponseDTO;
using Microsoft.DotNet.MSIdentity.Shared;

namespace Fall2024_SWD392_SE1704_111_FE.Pages.UserFE
{
    public class IndexModel : PageModel
    {
        public IList<UserListDTO> Users { get; set; } = null!;

        public ResponseDTO dto { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                

                string url = "https://localhost:7211/api/v1/users/usersList";

                string? jwt = Request.Cookies["jwt"]!.ToString();
                if(jwt == null)
                {
                    return RedirectToPage("../Login");
                }
                string jsonProduct = JsonConvert.SerializeObject(Users);
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
                    if (role == "Admin")
                    {


                        // Lấy danh sách người dùng từ API nếu token hợp lệ
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var dto = JsonConvert.DeserializeObject<ResponseDTO>(jsonResponse)!;

                        // Deserialize `dto.Data` to `List<UserListDTO>`
                        var usersListJson = JsonConvert.SerializeObject(dto.Data);
                        Users = JsonConvert.DeserializeObject<IList<UserListDTO>>(usersListJson);

                        return Page();  // Trả về Razor Page với danh sách người dùng
                    }
                    else
                    {
                        // Nếu không phải admin, có thể trả về trang lỗi hoặc chuyển hướng
                        return RedirectToPage("/AccessDenied");
                    }
                }

                else
                {
                    TempData["errorList"] = "Get List Error";
                    return Page();
                }
            }
            catch (Exception) {
                TempData["errorList"] = "An error occurred while processing your request. Please try again later";
                return Page();
            }
        }
    }
}
