using DB_Layer.Entities;

namespace DB_Layer.Repositories.BookingRepo
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<IEnumerable<Booking>?> GetBookingsByUserIdAsync(string userId);
        Task<bool> HasUserBookedEventAsync(string userId, string eventId);
    }
}
