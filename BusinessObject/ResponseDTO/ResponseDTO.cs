using BusinessObject.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.ReportEnum;
using static BusinessObject.ServiceEnum;

namespace BusinessObject.ResponseDTO
{
    public class ResponseDTO
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
        public ResponseDTO(int status, string? message, object? data = null)
        {
            Status = status;
            Message = message;
            Data = data;
        }


    }

    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }

    public class VnPayResponseModel
    {
        public string VnPayResponseCode { get; set; }
        public string Message { get; set; }
        public string TransactionId { get; set; }
        public string BookingId { get; set; }
    }

    public class PaymentType
    {
        public static string VNPAY = "VnPay";
    }

    public class ServicesDTO
    {
        public int ServiceId { get; set; }
        public string? ImageLink { get; set; }
        public string? ServiceName { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public TimeSpan? EstimateTime { get; set; }
        public ServiceStatusEnum? Status { get; set; }
    }

    public class StylistResponseDTO
    {
        public int StylistId { get; set; }
        public string StylistName { get; set; }
    }

    public class ScheduleDTO
    {
        public int ScheduleId { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Status { get; set; }
    }

    public class ScheduleUserDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string? Phone { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    public class GetAllScheduleDTO
    {
        public int ScheduleId { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ScheduleEnum? Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }
    }

    public class UserProfileDTO
    {
        public int UserProfileId { get; set; }
        public int UserId { get; set; }
        public string? ImageLink { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int? Gender { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? RegistrationDate { get; set; }
    }

    public class UserProfileUpdatedDTO
    {
        public int UserProfileId { get; set; }
        public int UserId { get; set; }
        public string? ImageLink { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public int? Gender { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string? Phone { get; set; }
    }

    public class LoginResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Phone { get; set; }
        public UserStatus Status { get; set; }
        public UserRole Role { get; set; }
    }

    public class ViewBookingDTO
    {
        public int BookingId { get; set; }
        public double TotalPrice { get; set; }
        public User Customer { get; set; } = null!;
        public Voucher? Voucher { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
    }

    public class BookingHistoryDTO
    {
        public int BookingId { get; set; }
        // Danh sách các dịch vụ kèm tên nhân viên thực hiện
        public List<ServiceDetailDTO> Services { get; set; } = new List<ServiceDetailDTO>();
        public List<ScheduledDetailDTO> Schedules { get; set; } = new List<ScheduledDetailDTO>();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double TotalPrice { get; set; }
        public BookingStatus? Status { get; set; }

    }

    public class BookingOfStylistDTO
    {
        public int BookingId { get; set; }
        // Danh sách các dịch vụ kèm tên nhân viên thực hiện
        public ServiceDetailDTO Services { get; set; } = new ServiceDetailDTO();
        public ScheduledDetailDTO Schedules { get; set; } = new ScheduledDetailDTO();
        public double TotalPrice { get; set; }
        public BookingStatus? Status { get; set; }
        public string? CreateBy { get; set; }

    }

    public class ScheduleCurrentUserDTO
    {
        public int ScheduleUserId { get; set; }
        public ScheduleUserEnum? Status { get; set; }
        public ScheduledDetailDTO Schedule { get; set; } = new ScheduledDetailDTO();

    }

    public class ServiceDetailDTO
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string StylistName { get; set; }

        public double? Price { get; set; }
        public TimeSpan? EstimateTime { get; set; }

    }

    public class ScheduledDetailDTO
    {
        public int ScheduleId { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    public class UserListDTO
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public UserStatus? Status { get; set; }
        public UserRole? Role { get; set; }
    }


    public class VoucherDTO
    {
        public int VoucherId { get; set; }
        public double? DiscountAmount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Status { get; set; }

    }

    public class ReportDTO
    {
        public int ReportId { get; set; }
        public string? ReportName { get; set; }
        public string? ReportLink { get; set; }
        public ReportStatusEnum? Status { get; set; }
        public string? CreateBy { get; set; }
        public string? UpdateBy { get; set; }
    }
    public class BookingDetailResponseDTO
    {
        public int BookingDetailID { get; set; }
        public int BookingID { get; set; }
        [Required]
        public List<int>? ServiceId { get; set; }
        [Required]
        public List<int>? StylistId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
       
    }
    public class BookingResponseDTO
    {
        public int BookingId { get; set; }
        public double? TotalPrice { get; set; }
        public int? VoucherId { get; set; }
        public int? ManagerId { get; set; }
        public int? CustomerId { get; set; }
        public int? StaffId { get; set; }
        public List<int>? ScheduleId { get; set; }
        public BookingStatus? Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }


    }
    public class FeedbackResponseDTO
    {
        public int FeedbackId { get; set; }
        public int? UserId { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }

        public virtual User? User { get; set; }
    }

    public class StylistOfScheduleUserResponseDTO
    {
        public int StylistId { get; set; }
        public string StylistName { get; set; }
        public UserStatus? Status { get; set; }
    }

}

