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

namespace Fall2024_SWD392_SE1704_111_FE.Pages.HairServiceFE
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public HairService HairService { get; set; } = default!;
        [BindProperty]
        public CreateServiceDTO createDto { get; set; } = default!;
        public ResponseDTO dto { get; set; } = null!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                // Validate model state
                if (!ModelState.IsValid)
                {
                    TempData["error"] = "Invalid Input";
                    return Page();
                }

                // Get JWT token from cookie
                string? jwt = Request.Cookies["jwt"];
                if (string.IsNullOrEmpty(jwt))
                {
                    TempData["errorLogin"] = "You need to login to access this page";
                    return RedirectToPage("../Login");
                }

                // Serialize DTO to JSON
                string jsonRequest = JsonConvert.SerializeObject(createDto);
                string url = "https://localhost:7211/api/v1/hairservice/CreateHairService"; // API endpoint

                // Create HttpClient and set JWT authorization header
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

                // Create HTTP POST request with JSON content
                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Post,
                    Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json")
                };

                // Send the request to the API
                HttpResponseMessage response = await client.SendAsync(request);

                // Check the response from the API
                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "Hair service created successfully";
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
                    // Get error message from the API response
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    TempData["error"] = $"Error: {errorResponse}";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions during the request
                TempData["error"] = $"An error occurred: {ex.Message}";
                return Page();
            }
        }
    }
}
