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
        private readonly IMapper _mapper;
        private readonly IJWTService _jWTService;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper, IJWTService jWTService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jWTService = jWTService;
        }

        public async Task<ResponseDTO> ChangeBookingStatus(RequestDTO.ChangebookingStatusDTO request, int bookingId)
        {
            try
            {
                // Lấy người dùng hiện tại
                var booking = await _unitOfWork.BookingRepository.GetBookingByIdAsync(bookingId);
                if (booking == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, "Booking not found !");
                }

                // Sử dụng AutoMapper để ánh xạ thông tin từ DTO vào user

                booking.Status = request.Status;

                // Lưu các thay đổi vào cơ sở dữ liệu
                await _unitOfWork.BookingRepository.UpdateAsync(booking);

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
                int customerId;
                // Lấy người dùng hiện tại
                var user = await _jWTService.GetCurrentUserAsync();
                if (user == null)
                {
                    // Kiểm tra xem UserName đã tồn tại hay chưa
                    var existingUser = await _unitOfWork.UserRepository.GetUserByUserNameAsync(bookingRequest.UserName);
                    if(existingUser != null)
                    {
                        customerId = existingUser.UserId;
                    }
                    else
                    {
                        //Nếu người dùng chưa đăng nhập
                        RegisterRequestDTO registerRequestDTO = new RegisterRequestDTO();
                        registerRequestDTO.userName = bookingRequest.UserName;
                        registerRequestDTO.phone = bookingRequest.Phone;
                        var registUser = _mapper.Map<User>(registerRequestDTO);

                        //Tạo  mới người dùng với 2 field UserName và Password
                        var createAccountResponse = await _unitOfWork.UserRepository.CreateAsync(registUser);
                        if (createAccountResponse <= 0)
                        {
                            return new ResponseDTO(Const.FAIL_CREATE_CODE, "Create Account Fail");
                        }
                        customerId = registUser.UserId; // Sử dụng UserId của người dùng mới
                    }
                    
                }
                else
                {
                    // Nếu người dùng đã đăng nhập, sử dụng thông tin của họ
                    customerId = user.UserId;
                }
                double? totalPrice = 0;

                // Kiểm tra ServiceId
                if (bookingRequest.ServiceId == null || !bookingRequest.ServiceId.Any())
                {
                    return new ResponseDTO(400, Const.FAIL_READ_MSG);
                }

                foreach (var serviceId in bookingRequest.ServiceId)
                {
                    var service = await _unitOfWork.HairServiceRepository.GetByIdAsync(serviceId);
                    if (service == null)
                    {
                        return new ResponseDTO(400, $"Service with ID {serviceId} does not exist.");
                    }
                    totalPrice += service.Price;
                }

                // Kiểm tra StylistId
                if (bookingRequest.StylistId == null || !bookingRequest.StylistId.Any())
                {
                    return new ResponseDTO(400, Const.FAIL_READ_MSG);
                }

                foreach (var stylistId in bookingRequest.StylistId)
                {
                    var stylist = await _unitOfWork.UserRepository.GetByIdAsync(stylistId);
                    if (stylist == null || stylist.Role != UserRole.Stylist)
                    {
                        return new ResponseDTO(400, $"Stylist with ID {stylistId} does not exist or is not a stylist.");
                    }
                }

                // kiểm tra schedule có tồn tại không
                var schedule = await _unitOfWork.UserRepository.GetByIdAsync(bookingRequest.ScheduleId);
                if (schedule == null)
                {
                    return new ResponseDTO(400, $"Stylist with ID {bookingRequest.ScheduleId} does not exist.");
                }

                // Tạo một đối tượng Booking mới từ DTO
                var booking = _mapper.Map<Booking>(bookingRequest);
                booking.CustomerId = customerId;
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
                var checkUpdate = await _unitOfWork.BookingRepository.CreateBookingAsync(booking);
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
        public async Task<IEnumerable<ViewBookingDTO>> GetAllBookingsAsync(int page = 1, int pageSize = 10)
        {
            var bookings = await _unitOfWork.BookingRepository.GetAllAsync();
            var pagedBookings = bookings.Skip((page - 1) * pageSize).Take(pageSize);
            return _mapper.Map<IEnumerable<ViewBookingDTO>>(pagedBookings);
        }

    }
}
