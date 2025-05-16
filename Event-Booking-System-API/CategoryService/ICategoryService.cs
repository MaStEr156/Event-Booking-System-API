using Event_Booking_System_API.CategoryService.DTOs;

namespace Event_Booking_System_API.CategoryService
{
    public interface ICategoryService
    {
        Task<(IEnumerable<CategoryResponse>?, string?)> GetAllCategoriesAsync(int pageNumber, int pageSize);
        Task<(GetCategoryResponse?, string?)> GetCategoryByIdAsync(string id);
        Task<(AddCategoryResponse?, string?)> AddCategoryAsync(CategoryRequest categoryDto);
        Task<string?> UpdateCategoryAsync(string id, CategoryRequest categoryDto);
        Task<string?> DeleteCategoryAsync(string id);
        Task<string?> SoftDeleteCategoryAsync(string id);
    }
}
