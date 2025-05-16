using System.ComponentModel.DataAnnotations;

namespace Event_Booking_System_API.AuthService.DTOs
{
    public class UpdateProfileModel
    {
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(50, MinimumLength = 6)]
        public string CurrentPassword { get; set; }

        [StringLength(50, MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
    }
}
