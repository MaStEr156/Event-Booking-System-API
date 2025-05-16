using System.ComponentModel.DataAnnotations;

namespace Event_Booking_System_API.AuthService.DTOs
{
    public class RoleModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}
