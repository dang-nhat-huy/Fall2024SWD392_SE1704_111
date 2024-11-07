using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.Model;
using BusinessObject.ResponseDTO;
using Newtonsoft.Json;

namespace Fall2024_SWD392_SE1704_111_FE.Pages.ScheduleFE
{
    public class IndexModel : PageModel
    {
        public IList<Schedule> Schedules { get; set; } = null!;

        public PagedResult<Schedule> dto { get; set; } = null!;

        [BindProperty(SupportsGet = true)]
        public int Index { get; set; } = 1;

        public double Count { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var size = 5;

                string url = "https://localhost:7211/api/v1/schedule/PagingScheduleList_1?pageNumber=" + Index + "&pageSize=" + size;

                string? jwt = Request.Cookies["jwt"]!.ToString();
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
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var dto = JsonConvert.DeserializeObject<PagedResult<Schedule>>(jsonResponse)!;

                    // Deserialize `dto.Items`
                    var schedulesListJson = JsonConvert.SerializeObject(dto.Items);
                    Schedules = JsonConvert.DeserializeObject<IList<Schedule>>(schedulesListJson)!;

                    // Handle pagination
                    var countJson = JsonConvert.SerializeObject(dto.TotalCount);
                    var count = JsonConvert.DeserializeObject<int>(countJson);
                    Count = Math.Ceiling((double)count / size);

                    return Page();
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
    }
}
