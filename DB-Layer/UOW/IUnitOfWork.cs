using DB_Layer.Repositories.BookingRepo;
using DB_Layer.Repositories.CategoryRepo;
using DB_Layer.Repositories.EventRepo;

namespace DB_Layer.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        IEventRepository EventRepository { get; }
        IBookingRepository BookingRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
