using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Model;

namespace Fall2024_SWD392_SE1704_111_FE.Pages.ReportFE
{
    public class IndexModel : PageModel
    {
        private readonly BusinessObject.Model.HairSalonBookingContext _context;

        public IndexModel(BusinessObject.Model.HairSalonBookingContext context)
        {
            _context = context;
        }

        public IList<Report> Report { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Reports != null)
            {
                Report = await _context.Reports
                .Include(r => r.Booking)
                .Include(r => r.Stylist).ToListAsync();
            }
        }
    }
}
