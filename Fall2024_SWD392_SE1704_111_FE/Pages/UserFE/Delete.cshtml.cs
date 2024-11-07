using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Model;
using Newtonsoft.Json;
using BusinessObject.ResponseDTO;
using System.Text;
using BusinessObject;

namespace Fall2024_SWD392_SE1704_111_FE.Pages.UserFE
{
    
    public class DeleteModel : PageModel
    {

        [BindProperty]
      public User User { get; set; } = default!;

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
                if (role != null && !role.Equals("Admin") && !role.Equals("Manager"))
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

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var userId = User.UserId;
                string? jwt = Request.Cookies["jwt"]!.ToString();
                string url = "https://localhost:7211/api/v1/users/changeStatus/" + userId;
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Post
                };
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Change Status Successfully";
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
    }
}
