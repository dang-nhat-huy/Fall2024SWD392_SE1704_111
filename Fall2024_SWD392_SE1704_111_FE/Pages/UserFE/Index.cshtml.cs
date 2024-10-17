using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Model;

namespace Fall2024_SWD392_SE1704_111_FE.Pages.UserFE
{
    public class IndexModel : PageModel
    {
        private readonly BusinessObject.Model.HairSalonBookingContext _context;

        public IndexModel(BusinessObject.Model.HairSalonBookingContext context)
        {
            _context = context;
        }

        public IList<User> User { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Users != null)
            {
                User = await _context.Users.ToListAsync();
            }
        }
    }
}
