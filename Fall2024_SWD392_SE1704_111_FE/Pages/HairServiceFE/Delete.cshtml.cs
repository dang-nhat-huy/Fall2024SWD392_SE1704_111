using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using BusinessObject.Model;
using BusinessObject.ResponseDTO;

namespace Fall2024_SWD392_SE1704_111_FE.Pages.HairServiceFE
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public HairService HairService { get; set; } = default!;

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
                string url = "https://localhost:7211/api/v1/hairservice/GetHairServiceById/" + id;
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

                    // Deserialize dto.Data to HairService
                    var hairServiceJson = JsonConvert.SerializeObject(dto.Data);
                    HairService = JsonConvert.DeserializeObject<HairService>(hairServiceJson)!;
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
                var serviceId = HairService.ServiceId;
                string? jwt = Request.Cookies["jwt"]!.ToString();
                string url = "https://localhost:7211/api/v1/hairservice/changeHairServiceStatus/" + serviceId;
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
 