using DB_Layer.Entities;

namespace DB_Layer.Repositories.EventRepo
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<IEnumerable<Event>> GetEventsByCategoryId (string categoryId, int pageNumber, int pageSize);

    }
}
