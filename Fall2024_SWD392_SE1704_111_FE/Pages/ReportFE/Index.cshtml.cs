using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Model;
using Newtonsoft.Json;
using System.Security.Principal;

namespace Fall2024_SWD392_SE1704_111_FE.Pages.ReportFE
{
    public class IndexModel : PageModel
    {
        private readonly BusinessObject.Model.HairSalonBookingContext _context;

        public IndexModel(BusinessObject.Model.HairSalonBookingContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int Index { get; set; } = 1;
        public double Count { get; set; }

        public IList<Report> Report { get;set; } = default!;

        private async Task<double> CountMaxPage()
        {
            try
            {
                double count = 1;
                string? jwt = Request.Cookies["jwt"]!.ToString();
                string url = $"https://localhost:7211/api/v1/reports/GetAllReport";
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
                    string responseBody = await response.Content.ReadAsStringAsync();
                    count = JsonConvert.DeserializeObject<double>(responseBody)!;
                }
                else
                {
                    count = 1;
                }
                return count;
            }
            catch (Exception)
            {
                return 1;
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                //var role = HttpContext.Session.GetString("Role");
                string? jwt = Request.Cookies["jwt"];
                //if (role == null || jwt == null)
                //{
                //    TempData["errorLogin"] = "You need to login to access this page";
                //    return RedirectToPage("../Logout");
                //}
                //else if (!role.Equals("Stylist"))
                //{
                //    TempData["error"] = "You don't have permission to access this page";
                //    return RedirectToPage("../ReportFE/Index");
                //}

                jwt = jwt.ToString();
                var top = 10;
                var skip = (Index - 1) * top;
                string url = $"https://localhost:7211/api/v1/reports/GetAllReport?$skip={skip}&$top={top}";
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
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Report = JsonConvert.DeserializeObject<IList<Report>>(responseBody)!;
                }
                else
                {
                    TempData["error"] = "Error Getting Data";
                }

                Count = await CountMaxPage();

                return Page();
            }
            catch(Exception)
            {
                TempData["error"] = "An error occurred while processing your request. Please try again later";
                return Page();
            }
        }
    }
}
