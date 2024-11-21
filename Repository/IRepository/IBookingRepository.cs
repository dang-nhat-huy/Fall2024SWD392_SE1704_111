using BusinessObject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<Booking> GetBookingByIdAsync(int id);
        Task<int> CreateBookingAsync(Booking entity);

        Task<List<Booking>> GetBookingHistoryByCustomerIdAsync(int customerId);
        IQueryable<Booking> GetCustomerNameByCreatedByAsync(string fullName);
        Task<List<Booking>> GetBookingIncludeByIdAsync(int id);

        Task<List<Booking>> GetBookingHistoryWithNullStylistAsync();

        Task<List<Booking>> GetBookingListAsync();
        Task<List<Booking>> GetBookingListWithStylistNameAsync(string stylistName);
    }
}
