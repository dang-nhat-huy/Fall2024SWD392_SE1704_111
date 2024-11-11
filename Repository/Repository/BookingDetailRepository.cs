using BusinessObject.Model;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class BookingDetailRepository : GenericRepository<BookingDetail>, IBookingDetailRepository
    {
        public BookingDetailRepository() { }

        public BookingDetailRepository(HairSalonBookingContext context) => _context = context;

        public async Task<List<BookingDetail>> GetBookingByStylistIdAsync(int stylistId)
        {
            return await _context.BookingDetails
                .Where(b => b.StylistId == stylistId)
                .Include("Service")
                .Include("Schedule")
                .Include("Booking")
                .ToListAsync();
        }
    }
}
