using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Model;
using BusinessObject.ResponseDTO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using static BusinessObject.RequestDTO.RequestDTO;
using Microsoft.OpenApi.Extensions;

namespace Fall2024_SWD392_SE1704_111_FE.Pages.BookingFE
{
    public class DeleteModel : PageModel
    {

        [BindProperty]
        public Booking Booking { get; set; } = default!;
        [BindProperty]
        public int Status { get; set; } = 1;
        public ChangebookingStatusDTO BookingStatus { get; set; } = new ChangebookingStatusDTO();
        public async Task<IActionResult> OnGetAsync(int? id)
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
                if (role != null && !role.Equals("Manager") && !role.Equals("Staff"))
                {
                    TempData["error"] = "You are not authorized to access this page";
                    return RedirectToPage("../logout");
                }

                jwt = jwt.ToString();
                string url = "https://localhost:7211/api/v1/booking/GetBookingById/" + id;
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

                    // Deserialize `dto.Data` to `List<UserListDTO>`
                    var usersListJson = JsonConvert.SerializeObject(dto.Data);
                    Booking = JsonConvert.DeserializeObject<Booking>(usersListJson)!;

                    ViewData["BookingStatus"] = new SelectList(
                        Enum.GetValues(typeof(BookingUpdateStatus))
                            .Cast<BookingUpdateStatus>()
                            .Select(status => new { Value = (int)status, Text = status.ToString() }),
                        "Value",
                        "Text"
                    );
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
                var bookingId = Booking.BookingId;
                string? jwt = Request.Cookies["jwt"]!.ToString();
                BookingStatus.Status = (BookingUpdateStatus) Status;
                string jsonStatus = JsonConvert.SerializeObject(BookingStatus);
                string url = "https://localhost:7211/api/v1/booking/changeBookingStatus/" + bookingId;
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Post,
                    Content = new StringContent(jsonStatus, System.Text.Encoding.UTF8, "application/json")
                };
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "Change Successfully";
                    return RedirectToPage("./Index");
                }
                else
                {
                    ViewData["BookingStatus"] = new SelectList(
                        Enum.GetValues(typeof(BookingUpdateStatus))
                            .Cast<BookingUpdateStatus>()
                            .Select(status => new { Value = (int)status, Text = status.ToString() }),
                        "Value",
                        "Text"
                    );
                    TempData["error"] = "Error Getting Data";
                }
                return Page();
            }
            catch (Exception)
            {
                ViewData["BookingStatus"] = new SelectList(
                        Enum.GetValues(typeof(BookingUpdateStatus))
                            .Cast<BookingUpdateStatus>()
                            .Select(status => new { Value = (int)status, Text = status.ToString() }),
                        "Value",
                        "Text"
                    );
                TempData["error"] = "An error occurred while processing your request. Please try again later";
                return Page();
            }
        }
    }
}
