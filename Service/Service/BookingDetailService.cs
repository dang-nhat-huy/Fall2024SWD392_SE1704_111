using BusinessObject.ResponseDTO;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.ResponseDTO.ReportDTO;

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
            //var bookingDetail = await _bookingDetailRepository.GetByIdAsync(bookingDetailID);
            //if (bookingDetail == null)
            //{
            //    return null; // Hoặc throw exception nếu cần
            //}

            //// Ánh xạ dữ liệu từ Entity sang DTO
            //return new BookingDetailResponseDTO
            //{
            //    BookingDetailID = bookingDetail.BookingDetailId,
            //    BookingID = bookingDetail.BookingId ?? 0,
            //    StylistID = bookingDetail.StylistId ?? 0,
            //    ServiceID = bookingDetail.ServiceId ?? 0,
            //    CreateDate = bookingDetail.CreateDate,
            //    CreateBy = bookingDetail.CreateBy,
            //    UpdateDate = bookingDetail.UpdateDate,
            //    UpdateBy = bookingDetail.UpdateBy
            //};
            try
            {
                // Lấy Booking Detail bao gồm Service và Stylist
                var bookingDetail = await _bookingDetailRepository
                    .GetAllWithTwoInclude("Service", "Stylist") // Thay thế bằng phương thức tương ứng trong repository của bạn
                    .FirstOrDefaultAsync(bd => bd.BookingDetailId == bookingDetailID);

                if (bookingDetail == null)
                {
                    return null; // Hoặc throw exception nếu cần
                }

                // Khởi tạo DTO
                var bookingDetailDTO = new BookingDetailResponseDTO
                {
                    BookingDetailID = bookingDetail.BookingDetailId,
                    BookingID = bookingDetail.BookingId ?? 0,
                    CreateDate = bookingDetail.CreateDate,
                    CreateBy = bookingDetail.CreateBy,
                    UpdateDate = bookingDetail.UpdateDate,
                    UpdateBy = bookingDetail.UpdateBy,

                    // Gán giá trị cho danh sách StylistId 
                    StylistId = new List<int>(), // Khởi tạo danh sách rỗng

                    // Gán giá trị cho danh sách ServiceId 
                    ServiceId = new List<int>() // Khởi tạo danh sách rỗng
                };

                // Thêm StylistId vào danh sách nếu có
                if (bookingDetail.StylistId.HasValue)
                {
                    bookingDetailDTO.StylistId.Add(bookingDetail.StylistId.Value);
                }

                // Thêm ServiceId vào danh sách nếu có
                if (bookingDetail.ServiceId.HasValue)
                {
                    bookingDetailDTO.ServiceId.Add(bookingDetail.ServiceId.Value);
                }

                return bookingDetailDTO;
            }
            catch (Exception ex)
            {
                // Xử lý exception (ví dụ: log lỗi, throw exception)
                return null;
            }
        }
    }
}
