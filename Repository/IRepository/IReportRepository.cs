using BusinessObject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IReportRepository : IGenericRepository<Report>
    {
        Task<User?> GetUserByCurrentId(int userId);

        Task<Booking?> GetBookingById(int bookingId);

        Task<Report?> GetReportById(int reportId);
    }
}
