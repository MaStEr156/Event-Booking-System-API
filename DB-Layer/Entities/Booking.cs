using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB_Layer.Entities
{
    public class Booking
    {
        [Required]
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

        [Required]
        [ForeignKey("Event")]
        public string EventId { get; set; }
        public Event Event { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
