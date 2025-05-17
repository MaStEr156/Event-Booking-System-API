using DB_Layer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB_Layer.Persistence.EntityConfigurations
{
    public class BookingConfig : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder
              .HasOne(b => b.User)
              .WithMany(u => u.Bookings)
              .HasForeignKey(b => b.UserId)
              .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(b => b.Event)
                .WithMany(e => e.Bookings)
                .HasForeignKey(b => b.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
