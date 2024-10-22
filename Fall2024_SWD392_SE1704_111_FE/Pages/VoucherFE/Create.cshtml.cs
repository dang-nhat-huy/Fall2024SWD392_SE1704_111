using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.Model;

namespace Fall2024_SWD392_SE1704_111_FE.Pages.VoucherFE
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
            return Page();
        }

        [BindProperty]
        public Voucher Voucher { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Vouchers == null || Voucher == null)
            {
                return Page();
            }

            _context.Vouchers.Add(Voucher);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
