using AutoMapper;
using BusinessObject;
using BusinessObject.Model;
using BusinessObject.Paging;
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
using static BusinessObject.VoucherEnum;

namespace Service.Service
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJWTService _jWTService;
        private readonly IScheduleUserService _scheduleUserService;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper, IJWTService jWTService, IScheduleUserService scheduleUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jWTService = jWTService;
            _scheduleUserService = scheduleUserService;
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

                booking.Status = (BookingStatus?)request.Status;

                // Lưu các thay đổi vào cơ sở dữ liệu
                await _unitOfWork.BookingRepository.UpdateAsync(booking);

                return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, "Change Status Succeed");
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<ResponseDTO> AcceptBookingStatus(int bookingId)
        {
            try
            {
                int customerId;
                // Lấy người dùng hiện tại
                var user = await _jWTService.GetCurrentUserAsync();

                var booking = await _unitOfWork.BookingRepository.GetBookingByIdAsync(bookingId);
                if (booking == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, "Booking not found !");
                }

                // Sử dụng AutoMapper để ánh xạ thông tin từ DTO vào user

                booking.Status = booking.Status == BookingStatus.InQueue ? BookingStatus.Accepted : BookingStatus.InQueue;
                booking.UpdateBy = user.UserName;

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
                    if (existingUser != null)
                    {
                        customerId = existingUser.UserId;
                    }
                    else
                    {
                        //Nếu người dùng chưa đăng nhập
                        GuestRegisterRequestDTO registerRequestDTO = new GuestRegisterRequestDTO();
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
                double totalTime = 0;

                // Kiểm tra ServiceId
                if (bookingRequest.ServiceId == null || !bookingRequest.ServiceId.Any())
                {
                    return new ResponseDTO(400, Const.FAIL_READ_MSG);
                }
                // Kiểm tra trùng lặp ServiceId
                if (bookingRequest.ServiceId.Count != bookingRequest.ServiceId.Distinct().Count())
                {
                    return new ResponseDTO(400, "Service IDs cannot be duplicated.");
                }

                foreach (var serviceId in bookingRequest.ServiceId)
                {
                    var service = await _unitOfWork.HairServiceRepository.GetByIdAsync(serviceId);
                    if (service == null)
                    {
                        return new ResponseDTO(400, $"Service with ID {serviceId} does not exist.");
                    }
                    totalPrice += service.Price;
                    totalTime += service.EstimateTime?.TotalMinutes ?? 0;
                }

                // Kiểm tra Voucher nếu có VoucherId
                if (bookingRequest.VoucherId.HasValue)
                {
                    var voucher = await _unitOfWork.VoucherRepository.GetByIdAsync(bookingRequest.VoucherId.Value);
                    if (voucher == null)
                    {
                        return new ResponseDTO(400, $"Voucher with ID {bookingRequest.VoucherId} does not exist.");
                    }

                    // Kiểm tra tính hợp lệ của voucher (ngày, trạng thái, v.v.)
                    if (voucher.Status != VoucherStatusEnum.Active /*|| voucher.StartDate > DateTime.Now || voucher.EndDate < DateTime.Now*/)
                    {
                        return new ResponseDTO(400, "Voucher is not valid.");
                    }

                    // Giảm giá từ tổng giá nếu có DiscountAmount
                    if (voucher.DiscountAmount.HasValue)
                    {
                        totalPrice -= voucher.DiscountAmount.Value;
                        if (totalPrice < 0) totalPrice = 0; // Đảm bảo tổng giá không âm
                    }
                }



                // List to hold created schedules for each stylist
                var createdSchedules = new List<Schedule>();
                if (bookingRequest.StylistId != null)
                {
                    // Kiểm tra StylistId và tạo lịch cho từng stylist
                    for (int i = 0; i < bookingRequest.StylistId.Count; i++)
                    {
                        var stylistId = bookingRequest.StylistId[i];
                        var stylist = await _unitOfWork.UserRepository.GetUserByIdAsync(stylistId);

                        if (stylist == null || stylist.Role != UserRole.Stylist)
                        {
                            return new ResponseDTO(400, $"Stylist with ID {stylistId} does not exist or is not a stylist.");
                        }

                        // Gán UserId cho từng lịch trong danh sách Schedule tương ứng
                        var scheduleRequest = bookingRequest.Schedule[i];
                        scheduleRequest.UserId = stylistId;

                        // Sử dụng service để tạo lịch cho stylist
                        var createdSchedule = await _scheduleUserService.createScheduleUser(scheduleRequest);

                        // Thêm lịch đã tạo vào danh sách (nếu cần thiết)
                        createdSchedules.Add(createdSchedule.Data as Schedule);
                    }
                }

                foreach (var scheduleRequest in bookingRequest.Schedule)
                {
                    scheduleRequest.UserId = null; // Không gán stylistId
                    var createdSchedule = await _scheduleUserService.createScheduleUser(scheduleRequest);
                    createdSchedules.Add(createdSchedule.Data as Schedule);
                }


                // Tạo một đối tượng Booking mới từ DTO
                var booking = _mapper.Map<Booking>(bookingRequest);
                booking.CustomerId = customerId;
                booking.CreateDate = DateTime.Now;
                booking.Status = BookingStatus.InQueue;
                booking.TotalPrice = totalPrice;

                string name;
                if (user == null)
                {
                    name = bookingRequest.UserName;
                }
                else
                {
                    name = user.UserName;
                }
                booking.CreateBy = name;
                // Thêm BookingDetails từ ServiceId và StylistId
                booking.BookingDetails = new List<BookingDetail>();
                for (int y = 0; y < bookingRequest.ServiceId.Count; y++)
                {
                    for (int i = 0; i < createdSchedules.Count; i++)
                    {
                        var schedule = createdSchedules[i];
                        booking.BookingDetails.Add(new BookingDetail
                        {
                            ServiceId = bookingRequest.ServiceId[y],
                            StylistId = bookingRequest.StylistId != null && bookingRequest.StylistId.Count > y
                                        ? bookingRequest.StylistId[y]
                                        : null,
                            ScheduleId = schedule?.ScheduleId,
                            CreateDate = DateTime.Now,
                            CreateBy = name,
                        });
                    }
                }
                // Lưu Booking vào database thông qua UnitOfWork
                var checkUpdate = await _unitOfWork.BookingRepository.CreateBookingAsync(booking);
                if (checkUpdate > 0)
                {
                    if (bookingRequest.StylistId != null)
                    {
                        // Gọi hàm cập nhật status cho ScheduleUser
                        await UpdateScheduleUserStatusAsync(bookingRequest.StylistId, booking.BookingDetails.Select(b => b.ScheduleId).ToList(), ScheduleUserEnum.InQueue, name);

                    }

                    var userInfo = _unitOfWork.UserRepository.GetById(customerId);
                    var checkoutRequest = new CheckoutRequestDTO
                    {
                        TotalPrice = booking.TotalPrice,
                        CreateDate = DateTime.Now,
                        Description = $"{userInfo.UserName} {userInfo.Phone}",
                        FullName = userInfo.UserName,
                        BookingId = booking.BookingId
                    };
                    return new ResponseDTO(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, checkoutRequest);
                }
                return new ResponseDTO(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, ex);
            }

        }

        private async Task UpdateScheduleUserStatusAsync(List<int?> stylistIds, List<int?> scheduleIds, ScheduleUserEnum status, string updatedBy)
        {
            try
            {
                foreach (var stylistId in stylistIds)
                {
                    foreach (var scheduleId in scheduleIds)
                    {
                        // Sử dụng repository để lấy bản ghi ScheduleUser
                        var scheduleUser = await _unitOfWork.ScheduleUserRepository
                            .GetByUserAndScheduleIdAsync(stylistId, scheduleId);

                        if (scheduleUser != null)
                        {
                            // Cập nhật Status và UpdateBy cho từng bản ghi
                            scheduleUser.Status = status;
                            scheduleUser.UpdateDate = DateTime.Now;
                            scheduleUser.UpdateBy = updatedBy;

                            // Lưu thay đổi
                            await _unitOfWork.ScheduleUserRepository.UpdateAsync(scheduleUser);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu cần
                Console.WriteLine($"Error updating ScheduleUser status: {ex.Message}");
            }
        }

        public async Task<IEnumerable<ViewBookingDTO>> GetAllBookingsAsync(int page = 1, int pageSize = 10)
        {
            var bookings = await _unitOfWork.BookingRepository.GetAllAsync();
            var pagedBookings = bookings.Skip((page - 1) * pageSize).Take(pageSize);
            return _mapper.Map<IEnumerable<ViewBookingDTO>>(pagedBookings);
        }

        public async Task<ResponseDTO> GetBookingHistoryOfCurrentUser()
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
                var bookings = await _unitOfWork.BookingRepository.GetBookingHistoryByCustomerIdAsync(user.UserId);
                if (bookings == null || bookings.Count == 0)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "No booking history found.");
                }

                // Ánh xạ kết quả từ Booking sang BookingHistoryDTO bằng ánh xạ thủ công
                List<BookingHistoryDTO> bookingHistoryDto = new List<BookingHistoryDTO>();

                foreach (var booking in bookings)
                {
                    var dto = _mapper.Map<BookingHistoryDTO>(booking);
                    bookingHistoryDto.Add(dto);
                }

                foreach (var booking in bookingHistoryDto)
                {
                    // Lọc dịch vụ chỉ lấy dịch vụ duy nhất (không trùng lặp)
                    booking.Services = booking.Services
                        .DistinctBy(s => s.ServiceId)
                        .ToList();

                    // Lọc lịch trình chỉ lấy lịch trình duy nhất (không trùng lặp)
                    booking.Schedules = booking.Schedules
                        .DistinctBy(sched => sched.ScheduleId)
                        .ToList();

                }

                return new ResponseDTO(Const.SUCCESS_READ_CODE, "Booking history retrieved successfully.", bookingHistoryDto);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<ResponseDTO> GetAllBookingsAsync()
        {
            try
            {
                var listBooking = await _unitOfWork.BookingRepository.GetAllAsync();

                if (listBooking == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "No Bookings found.");
                }
                else
                {
                    var result = _mapper.Map<List<BookingResponseDTO>>(listBooking);
                    return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<PagedResult<Booking>> GetAllBookingPagingAsync(int pageNumber, int pageSize)
        {
            try
            {
                var bookingList = _unitOfWork.BookingRepository.GetAll();
                if (bookingList == null)
                {
                    throw new Exception();
                }
                return await Paging.GetPagedResultAsync(bookingList.AsQueryable(), pageNumber, pageSize);
            }
            catch (Exception)
            {
                return new PagedResult<Booking>();
            }
        }

        public async Task<ResponseDTO> GetBookingByIdAsync(int bookingId)
        {
            try
            {

                var booking = await _unitOfWork.BookingRepository.GetByIdAsync(bookingId);

                // Kiểm tra nếu danh sách rỗng
                if (booking == null)
                {
                    return new ResponseDTO(Const.SUCCESS_CREATE_CODE, "No Booking found with the ID");
                }

                //// Sử dụng AutoMapper để ánh xạ các entity sang DTO
                //var result = _mapper.Map<VoucherDTO>(voucher);

                return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, booking);
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu xảy ra
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<PagedResult<Booking>> GetCustomerPagingByCreatedByAsync(string customerName, int pageNumber, int pageSize)
        {
            try
            {

                // Gọi repository để lấy danh sách người dùng theo tên
                var users = _unitOfWork.BookingRepository.GetCustomerNameByCreatedByAsync(customerName);

                // Kiểm tra nếu danh sách rỗng
                if (users == null)
                {
                    throw new Exception();
                }

                var usersQuery = users.AsQueryable();

                return await Paging.GetPagedResultAsync(usersQuery, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu xảy ra
                return new PagedResult<Booking>();
            }
        }
    }
}
