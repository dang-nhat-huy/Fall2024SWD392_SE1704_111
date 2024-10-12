using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            [Required]
            [StringLength(255, ErrorMessage = "Password cannot exceed 255 characters.")]
            public string password { get; set; }
            [Required]
            [RegularExpression(@"^\d{10,15}$", ErrorMessage = "Phone number must be between 10 and 15 digits and only contain numbers.")]
            public string phone { get; set; }
        }

        public class AddToBookingDTO
        {
            public int? CustomerId { get; set; }
            public int? VoucherId { get; set; }
        }
    }
}
