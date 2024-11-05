using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.Model;

namespace Fall2024_SWD392_SE1704_111_FE.Pages.BookingFE
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
        ViewData["CustomerId"] = new SelectList(_context.Users, "UserId", "UserId");
        ViewData["ManagerId"] = new SelectList(_context.Users, "UserId", "UserId");
        ViewData["StaffId"] = new SelectList(_context.Users, "UserId", "UserId");
        ViewData["VoucherId"] = new SelectList(_context.Vouchers, "VoucherId", "VoucherId");
            return Page();
        }

        [BindProperty]
        public Booking Booking { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Bookings == null || Booking == null)
            {
                return Page();
            }

            _context.Bookings.Add(Booking);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
