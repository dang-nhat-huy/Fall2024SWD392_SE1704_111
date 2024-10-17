using BusinessObject;
using BusinessObject.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Security.Claims;

namespace RazorPage.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public User User { get; set; } = null!;
        public void OnGet()
        {
            //var role = HttpContext.Session.GetString("Role");
            //string? jwt = Request.Cookies["jwt"];
            //if (role == null || jwt == null)
            //{
            //    return Page();
            //}
            //if (role.Equals("Admin"))
            //{
            //    return RedirectToPage("./UserFE/Index");
            //}
            //else
            //{
            //    return RedirectToPage("./UserFE/Index");
            //}
            
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var username = User.UserName;
                var password = User.Password;
                string url = $"https://localhost:7211/api/v1/users/Login?username={username}&password={password}";

                var client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(url)
                };
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string jwt = await response.Content.ReadAsStringAsync();
                    var jwtHandler = new JwtSecurityTokenHandler();
                    var token = jwtHandler.ReadToken(jwt) as JwtSecurityToken;
                    var role = token!.Claims.FirstOrDefault(c => c.Type == "role")!.Value;

                    //// Set the cookie
                    //var cookieOptions = new CookieOptions
                    //{
                    //    Expires = DateTime.UtcNow.AddHours(1),
                    //    HttpOnly = true,
                    //    Path = "/"
                    //};
                    //Response.Cookies.Append("jwt", jwt, cookieOptions);

                    //Set Session
                    HttpContext.Session.SetString("Role", role);

                    if (role.Equals("Admin"))
                    {
                        return RedirectToPage("./UserFE/Index");
                    }
                    else
                    {
                        return RedirectToPage("./UserFE/Index");
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
