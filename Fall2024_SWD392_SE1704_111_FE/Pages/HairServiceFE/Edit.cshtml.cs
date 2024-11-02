using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Model;
using BusinessObject.ResponseDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using static BusinessObject.RequestDTO.RequestDTO;

namespace Fall2024_SWD392_SE1704_111_FE.Pages.HairServiceFE
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public HairService HairService { get; set; } = default!;
        [BindProperty]
        public UpdateServiceDTO updateServiceDto { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                string? jwt = Request.Cookies["jwt"];
                if (string.IsNullOrEmpty(jwt))
                {
                    TempData["errorLogin"] = "You need to login to access this page";
                    return RedirectToPage("../Login");
                }

                string url = $"https://localhost:7211/api/v1/hairservice/GetHairServiceById/{id}";
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var dto = JsonConvert.DeserializeObject<ResponseDTO>(jsonResponse)!;

                    // Deserialize `dto.Data` to `HairService`
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
                TempData["error"] = "An error occurred while processing your request. Please try again later.";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var serviceId = HairService.ServiceId;

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

                string jsonRequest = JsonConvert.SerializeObject(updateServiceDto);
                string url = $"https://localhost:7211/api/v1/hairservice/updateHairService/{serviceId}";

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
                    TempData["success"] = "Update Successfully";
                    return RedirectToPage("./Index");
                }
                else
                {
                    TempData["error"] = "Error Updating Data";
                }
                return Page();
            }
            catch (Exception)
            {
                TempData["error"] = "An error occurred while processing your request. Please try again later.";
                return Page();
            }
        }
    }
}
