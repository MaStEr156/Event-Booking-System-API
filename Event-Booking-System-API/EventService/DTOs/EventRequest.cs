using System.ComponentModel.DataAnnotations;

namespace Event_Booking_System_API.EventService.DTOs
{
    public class EventRequest
    {
        [MaxLength(100)]
        public string? Title { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public string? CategoryId { get; set; }

        public DateTime EventDate { get; set; }
        public string? Venue { get; set; }

        public decimal? Price { get; set; }

        public IFormFile? Image { get; set; }

    }
}
