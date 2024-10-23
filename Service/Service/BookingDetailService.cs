using BusinessObject.ResponseDTO;
using Repository.IRepository;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class BookingDetailService : IBookingDetailService
    {
        private readonly IBookingDetailRepository _bookingDetailRepository;

        public BookingDetailService(IBookingDetailRepository bookingDetailRepository)
        {
            _bookingDetailRepository = bookingDetailRepository;
        }

        public async Task<BookingDetailResponseDTO> GetBookingDetailByIdAsync(int bookingDetailID)
        {
            var bookingDetail = await _bookingDetailRepository.GetByIdAsync(bookingDetailID);
            if (bookingDetail == null)
            {
                return null; // Hoặc throw exception nếu cần
            }

            // Ánh xạ dữ liệu từ Entity sang DTO
            return new BookingDetailResponseDTO
            {
                BookingDetailID = bookingDetail.BookingDetailId,
                BookingID = bookingDetail.BookingId ?? 0,
                StylistID = bookingDetail.StylistId ?? 0,
                ServiceID = bookingDetail.ServiceId ?? 0,
                CreateDate = bookingDetail.CreateDate,
                CreateBy = bookingDetail.CreateBy,
                UpdateDate = bookingDetail.UpdateDate,
                UpdateBy = bookingDetail.UpdateBy
            };
        }
    }
}
