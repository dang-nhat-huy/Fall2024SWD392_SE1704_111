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
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Principal;
using System.Drawing;
using System.Data;

namespace Fall2024_SWD392_SE1704_111_FE.Pages.UserFE
{
    public class IndexModel : PageModel
    {
        public IList<User> Users { get; set; } = null!;

        public PagedResult<User> dto { get; set; } = null!;

        [BindProperty(SupportsGet = true)]
        public int Index { get; set; } = 1;
        public double Count { get; set; }
        [BindProperty]
        public string? searchValue { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                
                var size = 5;
                
                string url = "https://localhost:7211/api/v1/users/PagingUserList?pageNumber="+Index+"&pageSize="+size;

                string? jwt = Request.Cookies["jwt"]!.ToString();
                if (jwt == null)
                {
                    TempData["errorLogin"] = "You need to login to access this page";
                    return RedirectToPage("../Login");
                }
                string jsonProduct = JsonConvert.SerializeObject(Users);
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

                //// Check if a search query was provided
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    url = $"https://localhost:7211/api/v1/users/searchAccountByName/{searchValue}";
                //}

                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(url),
                };
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var role = HttpContext.Session.GetString("Role");
                    if (role == "Admin" || role == "Manager")
                    {
                        // Lấy thông tin ID của người dùng hiện tại từ token hoặc session
                        var currentUserId = HttpContext.Session.GetString("UserId");

                        // Lấy danh sách người dùng từ API nếu token hợp lệ
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var dto = JsonConvert.DeserializeObject<PagedResult<User>>(jsonResponse)!;

                        // Deserialize `dto.Data` to `User`
                        var usersListJson = JsonConvert.SerializeObject(dto.Items);
                        Users = JsonConvert.DeserializeObject<IList<User>>(usersListJson)!;

                        // Lọc danh sách để không bao gồm người dùng đang đăng nhập
                        Users = Users.Where(u => u.UserId.ToString() != currentUserId).ToList();

                        if(role == "Manager")
                        {
                            Users = Users.Where(u => u.UserId.ToString() != currentUserId && u.Role.ToString() != "Admin").ToList();
                        }

                        //phân trang cho list
                        var countJson = JsonConvert.SerializeObject(dto.TotalCount);
                        var count = JsonConvert.DeserializeObject<int>(countJson);
                        Count = Math.Ceiling((double)count/size);

                        //Count = await CountMaxPage();
                        return Page();  // Trả về Razor Page với danh sách người dùng
                    }
                    else
                    {
                        // Nếu không phải admin, có thể trả về trang lỗi hoặc chuyển hướng
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

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var size = 5;
                if (searchValue == null)
                {
                    //TempData["error"] = "You must input to search";
                    return RedirectToPage("../UserFE/Index");
                }

                ViewData["SearchOption"] = new SelectList(new[]
                    {
                    new { Value = "UserName", Text = "Username" }
                }, "Value", "Text");

                string? jwt = Request.Cookies["jwt"]!.ToString();


                string url = $"https://localhost:7211/api/v1/users/GetUserByUsernamePaging/" +searchValue;
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

                    // Lấy danh sách người dùng từ API nếu token hợp lệ
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var dto = JsonConvert.DeserializeObject<PagedResult<User>>(jsonResponse)!;

                    // Deserialize `dto.Data` to `User`
                    var usersListJson = JsonConvert.SerializeObject(dto.Items);
                    Users = JsonConvert.DeserializeObject<IList<User>>(usersListJson)!;

                    var role = HttpContext.Session.GetString("Role");

                    // Lấy thông tin ID của người dùng hiện tại từ token hoặc session
                    var currentUserId = HttpContext.Session.GetString("UserId");

                    // Lọc danh sách để không bao gồm người dùng đang đăng nhập
                    Users = Users.Where(u => u.UserId.ToString() != currentUserId).ToList();

                    if (role == "Manager")
                    {
                        Users = Users.Where(u => u.UserId.ToString() != currentUserId && u.Role.ToString() != "Admin").ToList();
                    }

                    //phân trang cho list
                    var countJson = JsonConvert.SerializeObject(dto.TotalCount);
                    var count = JsonConvert.DeserializeObject<int>(countJson);
                    Count = Math.Ceiling((double)count / size);

                    return Page();  // Trả về Razor Page với danh sách người dùng
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
    }
}
