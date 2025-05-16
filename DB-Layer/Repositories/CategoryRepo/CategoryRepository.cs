using DB_Layer.Entities;
using DB_Layer.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DB_Layer.Repositories.CategoryRepo
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Category?> GetByIdAsync(string id)
        {
            var categoryEntity = await _context.Categories
                .Include(c => c.Events)
                .Where(c => c.Id == id && !c.IsDeleted)
                .FirstOrDefaultAsync();
            if (categoryEntity == null)
            {
                return null;
            }
            return categoryEntity;
        }
        public async Task<IEnumerable<Category>?> GetAllAsync(int pageNumber, int pageSize)
        {
            var categories = await _context.Categories
                .Include(c => c.Events)
                .Where(c => !c.IsDeleted)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            if (categories == null)
                return null;
            return categories;
        }
        public async Task AddAsync(Category entity)
        {
            await _context.Categories.AddAsync(entity);
        }
        public async Task UpdateAsync(Category entity)
        {
            _context.Categories.Update(entity);
        }
        public async Task DeleteAsync(string id)
        {
            var categoryEntity = await _context.Categories.FindAsync(id);
            if (categoryEntity == null)
            {
                throw new KeyNotFoundException($"Category with ID {id} not found.");
            }
            _context.Categories.Remove(categoryEntity);
        }

        public async Task SoftDeleteAsync(string id)
        {
            var categoryEntity = await _context.Categories.FindAsync(id);
            if (categoryEntity == null)
            {
                throw new KeyNotFoundException($"Category with ID {id} not found.");
            }
            categoryEntity.IsDeleted = true;
            _context.Categories.Update(categoryEntity);
        }
    }
}
