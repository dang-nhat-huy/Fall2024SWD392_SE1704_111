using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.Model;
using BusinessObject.ResponseDTO;
using Newtonsoft.Json;

namespace Fall2024_SWD392_SE1704_111_FE.Pages.HairServiceFE
{
    public class IndexModel : PageModel
    {
        public IList<HairService> HairService { get; set; } = null!;

        public PagedResult<HairService> dto { get; set; } = null!;

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
                string url = "https://localhost:7211/api/v1/hairservice/PagingHairServiceList?pageNumber=" + Index + "&pageSize=" + size;

                string? jwt = Request.Cookies["jwt"]?.ToString();
                if (jwt == null)
                {
                    TempData["errorLogin"] = "You need to login to access this page";
                    return RedirectToPage("../Login");
                }

                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(url),
                };
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var role = HttpContext.Session.GetString("Role");
                    if (role == "Manager")
                    {
                        // L?y thông tin ID c?a ng??i dùng hi?n t?i t? token ho?c session
                        var currentUserId = HttpContext.Session.GetString("UserId");

                        // L?y danh sách d?ch v? t? API n?u token h?p l?
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var dto = JsonConvert.DeserializeObject<PagedResult<HairService>>(jsonResponse)!;

                        // Deserialize `dto.Items` 
                        var servicesListJson = JsonConvert.SerializeObject(dto.Items);
                        HairService = JsonConvert.DeserializeObject<IList<HairService>>(servicesListJson)!;

                        // Phân trang cho danh sách
                        var countJson = JsonConvert.SerializeObject(dto.TotalCount);
                        var count = JsonConvert.DeserializeObject<int>(countJson);
                        Count = Math.Ceiling((double)count / size);

                        return Page();  // Tr? v? Razor Page v?i danh sách d?ch v?
                    }
                    else
                    {
                        // N?u không ph?i admin, chuy?n h??ng t?i trang ??ng nh?p ho?c l?i
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
                TempData["errorList"] = "An error occurred while processing your request. Please try again later.";
                return Page();
            }
        }
    }
}
