using System;
using System.Collections.Generic;
using System.Net.Http;
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

        [BindProperty]
        public DateTime? searchValue { get; set; } // Nullable DateTime to handle empty dates

        // POST method to handle the search functionality
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var size = 5;

                // If no search value, redirect to the default index page
                if (!searchValue.HasValue)
                {
                    return RedirectToPage("./Index");
                }

                // Build the URL for the API to search schedules by StartDate
                string url = $"https://localhost:7211/api/v1/schedule/SearchByStartDate?query={searchValue.Value:yyyy-MM-dd}&pageNumber={Index}&pageSize={size}";

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

                    // Deserialize the schedule items
                    Schedules = dto.Items;

                    // Handle pagination
                    Count = Math.Ceiling((double)dto.TotalCount / size);

                    return Page();
                }
                else
                {
                    TempData["errorList"] = "Error retrieving search results";
                    Schedules = new List<Schedule>();  // Empty list on error
                    return Page();
                }
            }
            catch (Exception)
            {
                TempData["errorList"] = "An error occurred while processing your request. Please try again later";
                Schedules = new List<Schedule>();  // Empty list on exception
                return Page();
            }
        }


        // GET method to display paginated schedule list
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

                    // Deserialize schedule list
                    Schedules = dto.Items;

                    // Handle pagination
                    Count = Math.Ceiling((double)dto.TotalCount / size);

                    return Page();
                }
                else
                {
                    TempData["errorList"] = "Error retrieving schedules";
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
