using System.ComponentModel.DataAnnotations;

namespace Event_Booking_System_API.CategoryService.DTOs
{
    public class CategoryRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
} 