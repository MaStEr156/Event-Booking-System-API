using System.ComponentModel.DataAnnotations;

namespace Event_Booking_System_API.EventService.DTOs
{
    public class EventRequest
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public string CategoryId { get; set; }

        [Required]
        public DateTime EventDate { get; set; }
        [Required]
        public string Venue { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public IFormFile Image { get; set; }

    }
}
