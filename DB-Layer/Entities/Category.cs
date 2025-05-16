using System.ComponentModel.DataAnnotations;

namespace DB_Layer.Entities
{
    public class Category
    {
        [Key]
        [Required]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<Event>? Events { get; set; } = new List<Event>();
    }
}
