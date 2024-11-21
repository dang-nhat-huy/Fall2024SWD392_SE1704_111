using BusinessObject.Model;
using BusinessObject.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.RequestDTO.RequestDTO;

namespace Service.IService
{
    public interface IBookingService
    {
        //Task<ResponseDTO> AddBooking(AddToBookingDTO request);

        Task<ResponseDTO> ChangeBookingStatus(ChangebookingStatusDTO request, int bookingId);

        Task<ResponseDTO> CreateBooking(BookingRequestDTO bookingRequest);

        Task<ResponseDTO> GetBookingHistoryOfCurrentUser();
        Task<ResponseDTO> GetAllBookingsAsync();
        Task<PagedResult<Booking>> GetAllBookingPagingAsync(int pageNumber, int pageSize);
        Task<ResponseDTO> GetBookingByIdAsync(int bookingId);
        Task<ResponseDTO> AcceptBookingStatus(int bookingId);
        //Task<ResponseDTO> GetCustomerNameByCreatedByAsync(string fullName);
        Task<PagedResult<Booking>> GetCustomerPagingByCreatedByAsync(string customerName, int pageNumber, int pageSize);

        Task<ResponseDTO> GetBookingHistoryOfNullStylist();
    }
}
