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

namespace Fall2024_SWD392_SE1704_111_FE.Pages.VoucherFE
{
    public class IndexModel : PageModel
    {
        public IList<Voucher> Voucher { get; set; } = null!;

        public PagedResult<Voucher> dto { get; set; } = null!;

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

                string url = "https://localhost:7211/api/v1/voucher/PagingVoucherList?pageNumber=" + Index + "&pageSize=" + size;

                string? jwt = Request.Cookies["jwt"]!.ToString();
                if (jwt == null)
                {
                    TempData["errorLogin"] = "You need to login to access this page";
                    return RedirectToPage("../Login");
                }
                string jsonProduct = JsonConvert.SerializeObject(Voucher);
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
                        // Lấy thông tin ID của người dùng hiện tại từ token hoặc session
                        var currentUserId = HttpContext.Session.GetString("UserId");

                        // Lấy danh sách người dùng từ API nếu token hợp lệ
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var dto = JsonConvert.DeserializeObject<PagedResult<Voucher>>(jsonResponse)!;

                        // Deserialize `dto.Data` 
                        var vouchersListJson = JsonConvert.SerializeObject(dto.Items);
                        Voucher = JsonConvert.DeserializeObject<IList<Voucher>>(vouchersListJson)!;

                        //phân trang cho list
                        var countJson = JsonConvert.SerializeObject(dto.TotalCount);
                        var count = JsonConvert.DeserializeObject<int>(countJson);
                        Count = Math.Ceiling((double)count / size);

                        return Page();  // Trả về Razor Page với danh sách người dùng
                    }
                    else
                    {
                        // Nếu không phải admin, có thể trả về trang lỗi hoặc chuyển hướng
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
                TempData["errorList"] = "An error occurred while processing your request. Please try again later";
                return Page();
            }
        }
    }
}
