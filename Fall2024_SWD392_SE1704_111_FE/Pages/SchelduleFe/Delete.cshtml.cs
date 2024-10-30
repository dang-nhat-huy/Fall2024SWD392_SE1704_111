using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using BusinessObject.Model;
using BusinessObject.ResponseDTO;

namespace Fall2024_SWD392_SE1704_111_FE.Pages.ScheduleFE
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public Schedule Schedule { get; set; } = default!;

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
                    return RedirectToPage("../Logout");
                }

                jwt = jwt.ToString();
                string url = "https://localhost:7211/api/v1/schedule/GetScheduleById/" + id;
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

                    // Deserialize `dto.Data` to `Schedule`
                    var scheduleJson = JsonConvert.SerializeObject(dto.Data);
                    Schedule = JsonConvert.DeserializeObject<Schedule>(scheduleJson);
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

        public async Task<IActionResult> OnPostAsync(int id)
        {
            try
            {
                var scheduleId = Schedule.ScheduleId;
                string? jwt = Request.Cookies["jwt"]!.ToString();
                string url = "https://localhost:7211/api/v1/schedule/ChangeScheduleStatus/" + scheduleId;
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
                    TempData["success"] = "Change Successfully";
                    return RedirectToPage("./Index");
                }
                else
                {
                    TempData["error"] = "Error Changing Schedule Status";
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
