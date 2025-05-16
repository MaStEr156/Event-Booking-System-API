using System.ComponentModel.DataAnnotations;

namespace Event_Booking_System_API.AuthService.DTOs
{
    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
