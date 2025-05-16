using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ollama_DB_layer.Entities
{
    [Owned]
    public class RefreshToken
    {
        
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
        public DateTime CreatedOn { get; set; }
        public DateTime? RevokedOn { get; set; }
        public bool IsActive => RevokedOn == null && !IsExpired;

        [Required]
        public bool IsDeleted { get; set; } = false;
    }
}
