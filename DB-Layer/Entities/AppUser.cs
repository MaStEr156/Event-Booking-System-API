using Microsoft.AspNetCore.Identity;
using Ollama_DB_layer.Entities;
using System.ComponentModel.DataAnnotations;

namespace DB_Layer.Entities
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string? ProfilePictureUrl { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<Booking>? Bookings { get; set; } = new List<Booking>();
        public List<RefreshToken>? RefreshTokens { get; set; }
    }
}
