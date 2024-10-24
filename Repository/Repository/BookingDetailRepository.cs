
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

        public async Task<BookingDetail?> GetBookingDetailAsync(int id)
        {
            return await _context.BookingDetails.FirstOrDefaultAsync(u => u.BookingId == id);
        }

        public async Task<BookingDetail?> GetBookingDetailByIdAsync(int id)
        {
            return await _context.BookingDetails.FirstOrDefaultAsync(u => u.BookingId == id);
        }


    }
}