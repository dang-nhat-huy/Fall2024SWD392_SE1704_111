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
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Schedule Schedule { get; set; } = default!;
        [BindProperty]
        public CreateScheduleDTO CreateDto { get; set; } = default!;
        public ResponseDTO ResponseDto { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                // Validate the model state
                if (!ModelState.IsValid)
                {
                    TempData["error"] = "Invalid Input";
                    return Page();
                }

                // Get JWT token from cookies
                string? jwt = Request.Cookies["jwt"];
                if (string.IsNullOrEmpty(jwt))
                {
                    TempData["errorLogin"] = "You need to login to access this page";
                    return RedirectToPage("../Login");
                }

                // Serialize CreateScheduleDTO to JSON
                string jsonRequest = JsonConvert.SerializeObject(CreateDto);
                string url = "https://localhost:7211/api/v1/schedule/CreateSchedule"; // API endpoint

                // Create HttpClient to send request
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

                // Create HTTP POST request with JSON content
                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Post,
                    Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json")
                };

                // Send request to API
                HttpResponseMessage response = await client.SendAsync(request);

                // Check API response
                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Schedule created successfully";
                    var role = HttpContext.Session.GetString("Role");
                    if (role == "Manager")
                    {
                        return RedirectToPage("./Index");
                    }
                    else
                    {
                        return RedirectToPage("/Login");
                    }
                }
                else
                {
                    // Extract and display error message from API
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    TempData["error"] = $"Error: {errorResponse}";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur
                TempData["error"] = $"An error occurred: {ex.Message}";
                return Page();
            }
        }
    }
}
