using Newtonsoft.Json;
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
            [Required]
            public string userName { get; set; }
            [Required]
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
            [Required(ErrorMessage = "Full name is required.")]
            public string? FullName { get; set; }
            [Required(ErrorMessage = "Email is required.")]
            [EmailAddress(ErrorMessage = "Invalid email format.")]
            public string? Email { get; set; }
            [Required(ErrorMessage = "Gender is required.")]
            public int? Gender { get; set; }
            [Required(ErrorMessage = "Address is required.")]
            public string? Address { get; set; }
            [Required(ErrorMessage = "Date of birth is required.")]
            public DateTime? DateOfBirth { get; set; }
        }

        public class GuestRegisterRequestDTO
        {
            [Required]
            [StringLength(255, ErrorMessage = "Username cannot exceed 255 characters.")]
            public string userName { get; set; }
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
            public List<createScheduleUser?> Schedule { get; set; }
            [Required]
            public List<int> ServiceId { get; set; }
            public List<int?> StylistId { get; set; }

        }

        public class CheckoutRequestDTO
        {
            public double? TotalPrice { get; set; } // Tổng giá của booking
            public DateTime CreateDate { get; set; } // Ngày tạo booking
            public string Description { get; set; } // Mô tả booking
            public string FullName { get; set; } // Tên khách hàng
            public int BookingId { get; set; } // ID của booking
        }

        public class AssignStylistRequestDTO
        {
            public int StylistId { get; set; }
            public int BookingId { get; set; } // ID của booking
            public int ScheduleUserId { get; set; }
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
            public BookingUpdateStatus Status { get; set; }
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
            [Required(ErrorMessage = "StartTime is required.")]
            public TimeSpan? StartTime { get; set; }
            [Required(ErrorMessage = "EndTime is required.")]
            [EndTimeGreaterThanStartTime("StartTime", ErrorMessage = "EndTime must be greater than StartTime.")]
            public TimeSpan? EndTime { get; set; }
            [Required(ErrorMessage = "StartDate is required.")]
            public DateTime? StartDate { get; set; }
            [Required(ErrorMessage = "EndDate is required.")]
            [EndDateValidation("StartDate", ErrorMessage = "EndDate must be greater than StartDate.")]
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

        public class CreateAccountDTO
        {
            [Required]
            [StringLength(255, ErrorMessage = "Username cannot exceed 255 characters.")]
            public string userName { get; set; }
            [StringLength(255, ErrorMessage = "Password cannot exceed 255 characters.")]
            public string? password { get; set; }
            [Required]
            [MinLength(8), MaxLength(20)]
            [RegularExpression(@"^\d+$", ErrorMessage = "Phone Is Number Only")]
            public string? Phone { get; set; }
            [Required]
            public UserRole? RoleId { get; set; }
        }

        public class UpdateAccountDTO
        {
            [Required]
            [StringLength(255, ErrorMessage = "Username cannot exceed 255 characters.")]
            public string userName { get; set; }
            [StringLength(255, ErrorMessage = "Password cannot exceed 255 characters.")]
            public string? password { get; set; }
            [Required]
            [MinLength(8), MaxLength(20)]
            [RegularExpression(@"^\d+$", ErrorMessage = "Phone Is Number Only")]
            public string? Phone { get; set; }
            [Required]
            public UserRole? RoleId { get; set; }
        }

        

        public class VnPaymentRequestModel
        {
            public int BookingId { get; set; }
            public string FullName { get; set; }
            public string Description { get; set; }
            public double? TotalPrice { get; set; }
            public DateTime CreateDate { get; set; }
        }

        public class UpdateVoucherDTO
        {
            [Range(0, double.MaxValue, ErrorMessage = "Discount amount must be a positive value.")]
            public double? DiscountAmount { get; set; }
            [DataType(DataType.Date)]
            [DateInFuture(ErrorMessage = "Start date must not be in the past.")]
            public DateTime? StartDate { get; set; }
            [DataType(DataType.Date)]
            [DateInFuture(ErrorMessage = "End date must not be in the past.")]
            [EndDateValidation("StartDate", ErrorMessage = "End date must be greater than start date.")]
            public DateTime? EndDate { get; set; }
        }

        public class CreateVoucherDTO
        {
            [Range(0, double.MaxValue, ErrorMessage = "Discount amount must be a positive value.")]
            public double? DiscountAmount { get; set; }
            [DataType(DataType.Date)]
            [DateInFuture(ErrorMessage = "Start date must not be in the past.")]
            public DateTime? StartDate { get; set; }
            [DataType(DataType.Date)]
            [DateInFuture(ErrorMessage = "End date must not be in the past.")]
            [EndDateValidation("StartDate", ErrorMessage = "End date must be greater than start date.")]
            public DateTime? EndDate { get; set; }
        }

        public class DateInFuture : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value != null)
                {
                    var date = (DateTime)value;

                    // So sánh ngày không nhỏ hơn ngày hiện tại (chấp nhận ngày hôm nay)
                    if (date.Date < DateTime.Now.Date)
                    {
                        return new ValidationResult(ErrorMessage);
                    }
                }
                return ValidationResult.Success;
            }
        }


        public class EndDateValidation : ValidationAttribute
        {
            private readonly string _comparisonProperty;

            public EndDateValidation(string comparisonProperty)
            {
                _comparisonProperty = comparisonProperty;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                // Lấy giá trị của StartDate từ đối tượng đang được kiểm tra
                var startDateValue = validationContext.ObjectInstance.GetType()
                                    .GetProperty(_comparisonProperty)
                                    ?.GetValue(validationContext.ObjectInstance, null);

                if (value != null && startDateValue != null)
                {
                    var endDate = (DateTime)value;
                    var startDate = (DateTime)startDateValue;

                    if (endDate <= startDate)
                    {
                        return new ValidationResult(ErrorMessage);
                    }
                }
                return ValidationResult.Success;
            }
        }
        public class FeedbackRequestDTO
        {
            public int? UserId { get; set; }
            public string? Description { get; set; }
            [DateInFuture(ErrorMessage = "Create date must not be in the past.")]
            public DateTime? CreateDate { get; set; }
            public string? CreateBy { get; set; }

        }
        public class ChangefeedbackStatusDTO
        {
            public FeedbackStatusEnum Status { get; set; }
        }
        public class RemoveFeedbackDTO
        {
            public int? Status { get; set; }
        }

        public class UpdateFeedbackDTO
        {
            [MinLength(5, ErrorMessage = "The description must be at least 5 characters long.")]
            public string? Description { get; set; }

            [DataType(DataType.Date)]
            public DateTime? CreateDate { get; set; }
            
        }
        public class CreateFeedbackDTO
        {
            [MinLength(5, ErrorMessage = "The description must be at least 5 characters long.")]
            public string? Description { get; set; }
           
            [DataType(DataType.Date)]
            public DateTime? CreateDate { get; set; }
        }

        public class ResetPasswordRequest
        {
            [Required]
            public string? UserName { get; set; }
            [Required]
            public string? Password { get; set; }
        }
        public class EndTimeGreaterThanStartTime : ValidationAttribute
        {
            private readonly string _comparisonProperty;

            public EndTimeGreaterThanStartTime(string comparisonProperty)
            {
                _comparisonProperty = comparisonProperty;
            }

            protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
            {
                var startTimeValue = validationContext.ObjectInstance.GetType()
                                       .GetProperty(_comparisonProperty)
                                       ?.GetValue(validationContext.ObjectInstance, null);

                if (value != null && startTimeValue != null)
                {
                    var endTime = (TimeSpan)value;
                    var startTime = (TimeSpan)startTimeValue;

                    if (endTime <= startTime)
                    {
                        return new ValidationResult(ErrorMessage ?? "EndTime must be greater than StartTime.");
                    }
                }

                return ValidationResult.Success;
            }
        }

        public class TimeSpanConverter : JsonConverter<TimeSpan?>
        {
            public override TimeSpan? ReadJson(JsonReader reader, Type objectType, TimeSpan? existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.String)
                {
                    if (TimeSpan.TryParse(reader.Value.ToString(), out var result))
                    {
                        return result;
                    }
                }
                return null;
            }

            public override void WriteJson(JsonWriter writer, TimeSpan? value, JsonSerializer serializer)
            {
                writer.WriteValue(value?.ToString(@"hh\:mm\:ss"));
            }
        }

        //public class TimeInFuture : ValidationAttribute
        //{
        //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //    {
        //        if (value != null)
        //        {
        //            var inputTime = (TimeSpan)value;
        //            var currentTime = DateTime.Now.TimeOfDay;

        //            // Kiểm tra thời gian không được nhỏ hơn thời gian hiện tại
        //            if (inputTime < currentTime)
        //            {
        //                return new ValidationResult(ErrorMessage ?? "Thời gian không được ở quá khứ.");
        //            }
        //        }
        //        return ValidationResult.Success;
        //    }
        //}

        public class TimeInFuture : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value is not TimeSpan inputTime)
                    return ValidationResult.Success;

                // Lấy đối tượng từ context để kiểm tra StartDate
                var startDateProperty = validationContext.ObjectType.GetProperty("StartDate");
                if (startDateProperty == null)
                    throw new ArgumentException("StartDate property not found.");

                var startDateValue = startDateProperty.GetValue(validationContext.ObjectInstance) as DateTime?;

                if (startDateValue == null)
                    return new ValidationResult("Start date is required for time validation.");

                // Ghép TimeSpan vào StartDate để tính toán
                var inputDateTime = startDateValue.Value.Date.Add(inputTime);

                // Nếu StartDate là hôm nay, kiểm tra thời gian phải là hiện tại hoặc tương lai
                if (startDateValue.Value.Date == DateTime.Now.Date)
                {
                    if (inputDateTime < DateTime.Now)
                    {
                        return new ValidationResult(ErrorMessage ?? "Time must not be in the past for today's date.");
                    }
                }

                // Nếu StartDate là tương lai, thời gian nào cũng hợp lệ
                return ValidationResult.Success;
            }
        }




        public class createScheduleUser
        {
            [JsonConverter(typeof(TimeSpanConverter))]
            public TimeSpan? StartTime { get; set; }

            //[JsonConverter(typeof(TimeSpanConverter))]
            public TimeSpan? EndTime { get; set; }


            [DataType(DataType.Date)]
            [DateInFuture(ErrorMessage = "Start date must not be in the past.")]
            public DateTime? StartDate { get; set; }

            //[DataType(DataType.Date)]
            //[DateInFuture(ErrorMessage = "End date must not be in the past.")]
            public DateTime? EndDate { get; set; }
            public int? UserId { get; set; }
        }

        public class viewScheduleOfStylist
        {
            public int ScheduleUserId { get; set; }
            public string? FullName { get; set; }
            [JsonConverter(typeof(TimeSpanConverter))]
            public TimeSpan? StartTime { get; set; }

            [JsonConverter(typeof(TimeSpanConverter))]
            public TimeSpan? EndTime { get; set; }

            [DataType(DataType.Date)]
            [DateInFuture(ErrorMessage = "Start date must not be in the past.")]
            public DateTime? StartDate { get; set; }

            [DataType(DataType.Date)]
            [DateInFuture(ErrorMessage = "End date must not be in the past.")]
            public DateTime? EndDate { get; set; }
            public ScheduleEnum? Status { get; set; }
        }

        public class getStartDateAndStartTime
        {
            [JsonConverter(typeof(TimeSpanConverter))]
            public TimeSpan StartTime { get; set; }
            [DataType(DataType.Date)]
            [DateInFuture(ErrorMessage = "Start date must not be in the past.")]
            public DateTime StartDate { get; set; }
        }
    }
}
