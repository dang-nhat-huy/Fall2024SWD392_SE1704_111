using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.ReportEnum;
using static BusinessObject.UserProfileEnum;

namespace BusinessObject.RequestDTO
{
    public class RequestDTO
    {
        public class LoginRequestDTO
        {
            public string userName { get; set; }
            public string password { get; set; }
        }

        public class RegisterRequestDTO
        {
            [Required]
            [StringLength(255, ErrorMessage = "Username cannot exceed 255 characters.")]
            public string userName { get; set; }
            [StringLength(255, ErrorMessage = "Password cannot exceed 255 characters.")]
            public string? password { get; set; }
            [Required]
            [RegularExpression(@"^\d{10,15}$", ErrorMessage = "Phone number must be between 10 and 15 digits and only contain numbers.")]
            public string phone { get; set; }
        }

        public class BookingRequestDTO
        {
            public string? UserName { get; set; }
            public string? Phone { get; set; }
            public int? VoucherId { get; set; }
            [Required]
            public int ScheduleId { get; set; }
            [Required]
            public List<int>? ServiceId { get; set; }
            [Required]
            public List<int>? StylistId { get; set; }

        }

        public class UpdateUserProfileDTO
        {
            [Url(ErrorMessage = "Invalid URL format.")]
            public string? ImageLink { get; set; }

            [StringLength(100, ErrorMessage = "Full Name cannot be longer than 100 characters.")]
            public string? FullName { get; set; }

            [EmailAddress(ErrorMessage = "Invalid email format.")]
            public string? Email { get; set; }

            public GenderEnum Gender { get; set; }

            [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
            public string? Address { get; set; }

            public DateTime? DateOfBirth { get; set; }

            [RegularExpression(@"^\d{10,15}$", ErrorMessage = "Phone number must be between 10 and 15 digits and only contain numbers.")]
            public string? Phone { get; set; }
        }

        public class ChangeStatusAccountDTO
        {
            public UserStatus? Status { get; set; }
        }

        public class CreateReportDTO
        {
            public int? BookingId { get; set; }
            [StringLength(100, ErrorMessage = "Full Name cannot be longer than 100 characters.")]
            public string? ReportName { get; set; }
            [Url(ErrorMessage = "Invalid URL format.")]
            public string? ReportLink { get; set; }
        }

        public class UpdateReportDTO
        {
            [StringLength(100, ErrorMessage = "Full Name cannot be longer than 100 characters.")]
            public string? ReportName { get; set; }
            [Url(ErrorMessage = "Invalid URL format.")]
            public string? ReportLink { get; set; }
        }

        public class RemoveReportDTO
        {
            public ReportStatusEnum? Status { get; set; }
        }

        public class ChangebookingStatusDTO
        {
            public BookingStatus Status { get; set; }
        }

        public class SearchAccountByNameDTO
        {
            [StringLength(100, ErrorMessage = "Full Name cannot be longer than 100 characters.")]
            public string? UserName { get; set; }
        }

        public class CreateServiceDTO
        {
            [Url(ErrorMessage = "Invalid URL format.")]
            public string? ImageLink { get; set; }
            [StringLength(100, ErrorMessage = "Full Name cannot be longer than 100 characters.")]
            public string? ServiceName { get; set; }
            public string? description { get; set; }
            public int? price { get; set; }
            public TimeSpan? estimateTime { get; set; }
           
        }
        public class UpdateServiceDTO
        {
            [Url(ErrorMessage = "Invalid URL format.")]
            public string? ImageLink { get; set; }
            [StringLength(100, ErrorMessage = "Full Name cannot be longer than 100 characters.")]
            public string? ServiceName { get; set; }
            public string? description { get; set; }
            public int? price { get; set; }
            public TimeSpan? estimateTime { get; set; }
        
        }

        public class RemoveServiceDTO
        {
            public int? Status { get; set; }
        }

        public class CreateScheduleDTO
        {
            public TimeSpan? StartTime { get; set; }
            public TimeSpan? EndTime { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
        }
        public class UpdateScheduleDTO
        {
            public TimeSpan? StartTime { get; set; }
            public TimeSpan? EndTime { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
        }

        public class RemoveScheduleDTO
        {
           public int? Status { get; set; }
        }
    }
}
