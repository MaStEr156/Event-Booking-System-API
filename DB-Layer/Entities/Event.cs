using DB_Layer.Helpers.Enums;
using System.ComponentModel.DataAnnotations;

namespace DB_Layer.Entities
{
    public class Event
    {
        [Required]
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]  
        public DateTime EventDate { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public string Venue { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<Booking>? Bookings { get; set; } = new List<Booking>();
    }
}
