using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Model;

namespace Fall2024_SWD392_SE1704_111_FE.Pages.HairServiceFE
{
    public class DetailsModel : PageModel
    {
        private readonly BusinessObject.Model.HairSalonBookingContext _context;

        public DetailsModel(BusinessObject.Model.HairSalonBookingContext context)
        {
            _context = context;
        }

        public HairService HairService { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.HairServices == null)
            {
                return NotFound();
            }

            var hairService = await _context.HairServices.FirstOrDefaultAsync(m => m.ServiceId == id);
            if (hairService == null)
            {
                return NotFound();
            }
            else
            {
                HairService = hairService;
            }
            return Page();
        }
    }
}
