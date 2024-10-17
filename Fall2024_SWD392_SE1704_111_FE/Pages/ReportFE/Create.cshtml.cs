using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.Model;

namespace Fall2024_SWD392_SE1704_111_FE.Pages.ReportFE
{
    public class CreateModel : PageModel
    {
        private readonly BusinessObject.Model.HairSalonBookingContext _context;

        public CreateModel(BusinessObject.Model.HairSalonBookingContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["BookingId"] = new SelectList(_context.Bookings, "BookingId", "BookingId");
        ViewData["StylistId"] = new SelectList(_context.Users, "UserId", "UserId");
            return Page();
        }

        [BindProperty]
        public Report Report { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Reports == null || Report == null)
            {
                return Page();
            }

            _context.Reports.Add(Report);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
