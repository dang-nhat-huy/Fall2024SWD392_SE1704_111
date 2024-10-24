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
        private readonly IUnitOfWork _unitOfWork;

        public BookingDetailService(IBookingDetailRepository bookingDetailRepository)
        {
            _bookingDetailRepository = bookingDetailRepository;
        }

        //public async Task<BookingDetailResponseDTO> GetBookingDetailByIdAsync1(int bookingDetailID)
        //{
        //    var bookingDetail = await _bookingDetailRepository.GetByIdAsync(bookingDetailID);
        //    if (bookingDetail == null)
        //    {
        //        return null; // Hoặc throw exception nếu cần
        //    }

        //    // Ánh xạ dữ liệu từ Entity sang DTO
        //    return new BookingDetailResponseDTO
        //    {
        //        BookingDetailID = bookingDetail.BookingDetailId,
        //        BookingID = bookingDetail.BookingId ?? 0,
        //        StylistID = bookingDetail.StylistId ?? 0,
        //        ServiceID = bookingDetail.ServiceId ?? 0,
        //        CreateDate = bookingDetail.CreateDate,
        //        CreateBy = bookingDetail.CreateBy,
        //        UpdateDate = bookingDetail.UpdateDate,
        //        UpdateBy = bookingDetail.UpdateBy
        //    };
        //}
        public async Task<BookingDetailResponseDTO> GetBookingDetailByIdAsync(int bookingDetailID)
        {
            // Lấy danh sách booking detail từ database dựa trên bookingDetailID
            var bookingDetails = await _unitOfWork.BookingDetailRepository.GetByConditionAsync(bd => bd.BookingDetailId == bookingDetailID); 

            if (bookingDetails == null || !bookingDetails.Any())
            {
                return null;
            }

            // Ánh xạ dữ liệu từ Entity sang DTO, lấy danh sách bookingID và stylistID
            return new BookingDetailResponseDTO
            {
                BookingDetailID = bookingDetailID,
                BookingID = bookingDetails.First().BookingId ?? 0,
                StylistID = bookingDetails.Select(bd => bd.StylistId ?? 0).ToArray(),
                ServiceID = bookingDetails.Select(bd => bd.ServiceId ?? 0).ToArray() , 
                CreateDate = bookingDetails.First().CreateDate, 
                CreateBy = bookingDetails.First().CreateBy,  
                UpdateDate = bookingDetails.First().UpdateDate, 
                UpdateBy = bookingDetails.First().UpdateBy    
            };
        }
    }
}
