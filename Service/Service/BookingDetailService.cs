using AutoMapper;
using BusinessObject;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJWTService _jWTService;

        public BookingDetailService(IBookingDetailRepository bookingDetailRepository, IUnitOfWork unitOfWork, IMapper mapper, IJWTService jWTService)
        {
            _bookingDetailRepository = bookingDetailRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jWTService = jWTService;
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

        public async Task<ResponseDTO> GetBookingOfCurrentStylist()
        {
            try
            {
                // Lấy người dùng hiện tại
                var user = await _jWTService.GetCurrentUserAsync();
                if (user == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "User not found.");
                }

                // Lấy danh sách booking của user hiện tại từ repository
                var bookings = await _unitOfWork.BookingDetailRepository.GetBookingByStylistIdAsync(user.UserId);
                if (bookings == null || bookings.Count == 0)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "No booking found.");
                }

                // Ánh xạ kết quả từ Booking sang BookingHistoryDTO bằng ánh xạ thủ công
                List<BookingOfStylistDTO> bookingDto = new List<BookingOfStylistDTO>();

                foreach (var booking in bookings)
                {
                    var dto = _mapper.Map<BookingOfStylistDTO>(booking);
                    bookingDto.Add(dto);
                }

                // Xử lý loại bỏ phần tử trùng lặp theo BookingId
                var distinctBookings = bookingDto
                    .GroupBy(b => b.BookingId)
                    .Select(g => g.First()) // Lấy phần tử đầu tiên của mỗi nhóm (trùng lặp)
                    .ToList();

                return new ResponseDTO(Const.SUCCESS_READ_CODE, "Booking retrieved successfully.", distinctBookings);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
    }
}
