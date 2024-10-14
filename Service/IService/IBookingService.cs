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
    }
}
