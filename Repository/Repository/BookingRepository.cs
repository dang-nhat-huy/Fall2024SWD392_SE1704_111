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
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository() { }

        public BookingRepository(HairSalonBookingContext context) => _context = context;

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings.FirstOrDefaultAsync(u => u.BookingId == id);
        }

        public async Task<List<Booking>> GetBookingIncludeByIdAsync(int id)
        {
            return await _context.Bookings
                .Where(b => b.BookingId == id)
                .Include(b => b.BookingDetails)
                .ToListAsync();
        }

        public async Task<int> CreateBookingAsync(Booking entity)
        {

            _context.Add(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Booking>> GetBookingHistoryByCustomerIdAsync(int customerId)
        {
            return await _context.Bookings
                .Where(b => b.CustomerId == customerId)
                .Include(b => b.BookingDetails)
                    .ThenInclude(bd => bd.Service)
                .Include(b => b.BookingDetails)  
                    .ThenInclude(bd => bd.Stylist)
                .Include(b => b.BookingDetails)
                .ThenInclude(bd => bd.Schedule)
                .ToListAsync();
        }

        public IQueryable<Booking> GetCustomerNameByCreatedByAsync(string fullName)
        {
            var customerList = _context.Bookings
                .Where(u => u.CreateBy.ToLower().StartsWith(fullName.ToLower())); // Trả về danh sách

            return customerList;


        }
    }
}
