using AutoMapper;
using BusinessObject;
using BusinessObject.Model;
using BusinessObject.RequestDTO;
using BusinessObject.ResponseDTO;
using Microsoft.Extensions.Configuration;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.RequestDTO.RequestDTO;

namespace Service.Service
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public BookingService(IUnitOfWork unitOfWork, IConfiguration config, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _mapper = mapper;
        }

        public async Task<ResponseDTO> ChangeBookingStatus(RequestDTO.ChangebookingStatusDTO request, int bookingId)
        {
            try
            {
                // Lấy người dùng hiện tại
                var booking = await _unitOfWork.bookingRepository.GetBookingByIdAsync(bookingId);
                if (booking == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, "Booking not found !");
                }

                // Sử dụng AutoMapper để ánh xạ thông tin từ DTO vào user

                booking.Status = request.Status;

                // Lưu các thay đổi vào cơ sở dữ liệu
                await _unitOfWork.bookingRepository.UpdateAsync(booking);

                return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, "Change Status Succeed");
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }       

        public async Task<ResponseDTO> CreateBooking(BookingRequestDTO bookingRequest)
        {
            try
            {
                double? totalPrice = 0;
                // kiểm tra Service và Stylist có tồn tại không
                foreach (var serviceId in bookingRequest.ServiceId)
                {
                    var service = await _unitOfWork.HairServiceRepository.GetByIdAsync(serviceId);
                    if (service == null)
                    {
                        return new ResponseDTO(400, $"Service with ID {serviceId} does not exist.");
                    }
                    totalPrice += service.Price;
                }

                foreach (var stylistId in bookingRequest.StylistId)
                {
                    var stylist = await _unitOfWork.UserRepository.GetByIdAsync(stylistId);
                    if (stylist == null || stylist.Role != UserRole.Stylist)
                    {
                        return new ResponseDTO(400, $"Stylist with ID {stylistId} does not exist or not a stylist.");
                    }
                }

                var schedule = await _unitOfWork.UserRepository.GetByIdAsync(bookingRequest.ScheduleId);
                if (schedule == null)
                {
                    return new ResponseDTO(400, $"Stylist with ID {bookingRequest.ScheduleId} does not exist or not a stylist.");
                }

                // Tạo một đối tượng Booking mới từ DTO
                var booking = _mapper.Map<Booking>(bookingRequest);
                booking.CreateDate = DateTime.Now;
                booking.Status = BookingStatus.InProgress;
                booking.TotalPrice = totalPrice;
                // Thêm BookingDetails từ ServiceId và StylistId
                booking.BookingDetails = new List<BookingDetail>();
                for (int i = 0; i < bookingRequest.ServiceId.Count; i++)
                {
                    booking.BookingDetails.Add(new BookingDetail
                    {
                        ServiceId = bookingRequest.ServiceId[i],
                        StylistId = bookingRequest.StylistId[i],
                        CreateDate = DateTime.Now,
                        //CreateBy = bookingRequest.CreatedBy
                    });
                }

                // Lưu Booking vào database thông qua UnitOfWork
                var checkUpdate = await _unitOfWork.bookingRepository.CreateBookingAsync(booking);
                if (checkUpdate <= 0)
                {
                    return new ResponseDTO(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                }
                return new ResponseDTO(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, booking);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, ex);
            }
            
        }

    }
}
