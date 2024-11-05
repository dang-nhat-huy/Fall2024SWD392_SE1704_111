using BusinessObject;
using BusinessObject.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using static BusinessObject.RequestDTO.RequestDTO;

namespace RazorPage.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public User User { get; set; } = null!;
        public void OnGet()
        {
            
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var username = User.UserName;
                var password = User.Password;

                // Tạo DTO với thông tin đăng nhập
                var loginDto = new LoginRequestDTO
                {
                    userName = username,
                    password = password
                };

                string url = "https://localhost:7211/api/v1/users/Login";

                var client = new HttpClient();

                // Chuyển đổi DTO thành JSON và đưa vào body
                var content = new StringContent(
                    JsonConvert.SerializeObject(loginDto), // Serialize DTO thành JSON
                    Encoding.UTF8,
                    "application/json" // Đặt Content-Type là application/json
                );

                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(url),
                    Content = content // Truyền body là DTO JSON
                };
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string jwt = await response.Content.ReadAsStringAsync();
                    var jwtHandler = new JwtSecurityTokenHandler();
                    var token = jwtHandler.ReadToken(jwt) as JwtSecurityToken;
                    var role = token!.Claims.FirstOrDefault(c => c.Type == "role")!.Value;
                    var userId = token!.Claims.FirstOrDefault(c => c.Type == "nameid")!.Value;
                    //var currentUsername = token.Claims.FirstOrDefault(c => c.Type == "username")?.ToString;

                    // Set the cookie
                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddHours(1),
                        HttpOnly = true,
                        Path = "/"
                    };
                    Response.Cookies.Append("jwt", jwt, cookieOptions);

                    //Set Session
                    HttpContext.Session.SetString("Role", role);
                    HttpContext.Session.SetString("UserId", userId);
                    HttpContext.Session.SetString("CurrentUsername", username);

                    if (role.Equals("Admin"))
                    {
                        return RedirectToPage("./UserFE/Index");
                    }
                    else if (role.Equals("Manager"))
                    {
                        return RedirectToPage("./UserFE/Index");
                    }
                    else if (role.Equals("Staff"))
                    {
                        return RedirectToPage("./BookingFE/Index");
                    }
                    else
                    {
                        TempData["errorLogin"] = "Unauthorize for this user!";
                        return Page();
                    }
                }
                else
                {
                    TempData["errorLogin"] = "Wrong User Or Password";
                    return Page();
                }
            }
            catch (Exception)
            {
                TempData["errorLogin"] = "An error occurred while processing your request. Please try again later";
                return Page();
            }
        }
    }
}
