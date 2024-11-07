using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using BusinessObject.Model;
using BusinessObject.ResponseDTO;
using static BusinessObject.RequestDTO.RequestDTO;

namespace Fall2024_SWD392_SE1704_111_FE.Pages.ScheduleFE
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public Schedule Schedule { get; set; } = default!;
        [BindProperty]
        public UpdateScheduleDTO UpdateDto { get; set; } = default!;

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
                if (role != "Manager")
                {
                    TempData["error"] = "You are not authorized to access this page";
                    return RedirectToPage("../Logout");
                }

                string url = "https://localhost:7211/api/v1/schedule/GetScheduleById/" + id;
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var dto = JsonConvert.DeserializeObject<ResponseDTO>(jsonResponse);

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

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["error"] = "Invalid Input";
                    return Page();
                }

                string? jwt = Request.Cookies["jwt"];
                if (string.IsNullOrEmpty(jwt))
                {
                    TempData["errorLogin"] = "You need to login to access this page";
                    return RedirectToPage("../Login");
                }

                // Serialize `UpdateScheduleDTO` to JSON
                string jsonRequest = JsonConvert.SerializeObject(UpdateDto);
                string url = $"https://localhost:7211/api/v1/schedule/UpdateSchedule/{Schedule.ScheduleId}";

                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Post,
                    Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json")
                };

                HttpResponseMessage response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "Schedule updated successfully";
                    return RedirectToPage("./Index");
                }
                else
                {
                    TempData["error"] = "Error Updating Schedule";
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
