using DB_Layer.Persistence;
using DB_Layer.Repositories.BookingRepo;
using DB_Layer.Repositories.CategoryRepo;
using DB_Layer.Repositories.EventRepo;

namespace DB_Layer.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IEventRepository EventRepository { get; }
        public IBookingRepository BookingRepository { get; }
        public ICategoryRepository CategoryRepository { get; }

        public UnitOfWork(AppDbContext context, IEventRepository eventRepository, IBookingRepository bookingRepository, ICategoryRepository categoryRepository)
        {
            _context = context;
            EventRepository = eventRepository;
            BookingRepository = bookingRepository;
            CategoryRepository = categoryRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
