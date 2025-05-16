using DB_Layer.Entities;
using DB_Layer.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DB_Layer.Repositories.EventRepo
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _context;
        public EventRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Event?> GetByIdAsync(string id)
        {
            var eventEntity = await _context.Events
                .Include(e => e.Bookings)
                .Include(e => e.Category)
                .Where(e => e.Id == id && !e.IsDeleted)
                .FirstOrDefaultAsync();

            if (eventEntity == null)
            {
                return null;
            }

            return eventEntity;
        }

        public async Task<IEnumerable<Event>?> GetAllAsync(int pageNumber, int pageSize)
        {
            var events = await _context.Events
                .Include(e => e.Bookings)
                .Include(e => e.Category)
                .Where(e => !e.IsDeleted)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            if (events == null)
                return null;
            return events;
        }

        public async Task AddAsync(Event entity)
        {
            await _context.Events.AddAsync(entity);
        }

        public async Task UpdateAsync(Event entity)
        {
            _context.Events.Update(entity);
        }

        public async Task DeleteAsync(string id)
        {
            var eventEntity = await _context.Events.FindAsync(id);
            if (eventEntity == null)
            {
                throw new KeyNotFoundException($"Event with ID {id} not found.");
            }

            _context.Events.Remove(eventEntity);
        }

        public async Task SoftDeleteAsync(string id)
        {
            var eventEntity = await _context.Events.FindAsync(id);
            if (eventEntity == null)
            {
                throw new KeyNotFoundException($"Event with ID {id} not found.");
            }

            eventEntity.IsDeleted = true;
            _context.Events.Update(eventEntity);
        }

        public async Task<IEnumerable<Event>> GetEventsByCategoryId(string categoryId, int pageNumber, int pageSize)
        {
            var events = await _context.Events
                .Include(e => e.Bookings)
                .Include(e => e.Category)
                .Where(e => e.CategoryId == categoryId && !e.IsDeleted)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return events;
        }
    }
}
