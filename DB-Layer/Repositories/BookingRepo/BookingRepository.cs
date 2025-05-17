using DB_Layer.Entities;
using DB_Layer.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DB_Layer.Repositories.BookingRepo
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;
        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Booking?> GetByIdAsync(string id)
        {
            var bookingEntity = await _context.Bookings
                .Include(b => b.Event)
                .Where(b => b.Id == id && !b.IsDeleted)
                .FirstOrDefaultAsync();
            if (bookingEntity == null)
            {
                return null;
            }
            return bookingEntity;
        }

        public async Task<IEnumerable<Booking>?> GetAllAsync(int pageNumber, int pageSize)
        {
            var bookings = await _context.Bookings
                .Include(b => b.Event)
                .Where(b => !b.IsDeleted)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            if (bookings == null)
                return null;
            return bookings;
        }

        public async Task AddAsync(Booking entity)
        {
            await _context.Bookings.AddAsync(entity);
        }

        public async Task UpdateAsync(Booking entity)
        {
            _context.Bookings.Update(entity);
        }

        public async Task DeleteAsync(string id)
        {
            var bookingEntity = await _context.Bookings.FindAsync(id);
            if (bookingEntity == null)
            {
                throw new KeyNotFoundException($"Booking with ID {id} not found.");
            }
            _context.Bookings.Remove(bookingEntity);

        }
        public async Task SoftDeleteAsync(string id)
        {
            var bookingEntity = await _context.Bookings.FindAsync(id);
            if (bookingEntity == null)
            {
                throw new KeyNotFoundException($"Booking with ID {id} not found.");
            }
            bookingEntity.IsDeleted = true;
            _context.Bookings.Update(bookingEntity);
        }

        public async Task<IEnumerable<Booking>?> GetBookingsByUserIdAsync(string userId)
        {
            var bookings = await _context.Bookings
                .Include(b => b.Event)
                .Where(b => b.UserId == userId && !b.IsDeleted)
                .ToListAsync();
            
            if (bookings == null || !bookings.Any())
                return null;
            return bookings;
        }

        public async Task<bool> HasUserBookedEventAsync(string userId, string eventId)
        {
            return await _context.Bookings
                .AnyAsync(b => b.UserId == userId && 
                             b.EventId == eventId && 
                             !b.IsDeleted);
        }
    }
}
