using DB_Layer.UOW;
using Event_Booking_System_API.CategoryService.DTOs;
using Event_Booking_System_API.CategoryService.Mappers;

namespace Event_Booking_System_API.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(IEnumerable<CategoryResponse>?, string?)> GetAllCategoriesAsync(int pageNumber, int pageSize)
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync(pageNumber, pageSize);
            if (categories == null || !categories.Any())
                return (null, "No Categories Found");

            try
            {
                var categoryResponses = categories.ToCategoryResponses();
                return (categoryResponses, null);
            }
            catch (Exception ex)
            {
                // Log exception (ex)
                return (null, $"Error retrieving categories: {ex.Message}");
            }
        }

        public async Task<(GetCategoryResponse?, string?)> GetCategoryByIdAsync(string id)
        {
            var categoryEntity = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (categoryEntity == null)
                return (null, $"Category with ID {id} not found.");

            try
            {
                var categoryResponse = categoryEntity.ToGetCategoryResponse();
                return (categoryResponse, null);
            }
            catch (Exception ex)
            {
                // Log exception (ex)
                return (null, $"Error retrieving category: {ex.Message}");
            }
        }

        public async Task<(AddCategoryResponse?, string?)> AddCategoryAsync(CategoryRequest categoryDto)
        {
            try
            {
                var categoryEntity = categoryDto.ToCategory();
                await _unitOfWork.CategoryRepository.AddAsync(categoryEntity);
                await _unitOfWork.SaveChangesAsync();

                var response = categoryEntity.ToAddCategoryResponse();
                return (response, null);
            }
            catch (Exception ex)
            {
                // Log exception (ex)
                return (null, $"Error adding category: {ex.Message}");
            }
        }

        public async Task<string?> UpdateCategoryAsync(string id, CategoryRequest categoryDto)
        {
            var categoryEntity = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (categoryEntity == null)
                return $"Category with ID {id} not found.";

            try
            {
                categoryEntity.Name = categoryDto.Name;
                
                await _unitOfWork.CategoryRepository.UpdateAsync(categoryEntity);
                await _unitOfWork.SaveChangesAsync();
                return null; // Success
            }
            catch (Exception ex)
            {
                // Log exception (ex)
                return $"Error updating category: {ex.Message}";
            }
        }

        public async Task<string?> DeleteCategoryAsync(string id)
        {
            var categoryEntity = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (categoryEntity == null)
                return $"Category with ID {id} not found.";

            try
            {
                await _unitOfWork.CategoryRepository.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();
                return null; // Success
            }
            catch (KeyNotFoundException knfEx)
            {
                return knfEx.Message;
            }
            catch (Exception ex)
            {
                // Log exception (ex)
                return $"Error deleting category: {ex.Message}";
            }
        }

        public async Task<string?> SoftDeleteCategoryAsync(string id)
        {
            var categoryEntity = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (categoryEntity == null)
                return $"Category with ID {id} not found.";

            try
            {
                await _unitOfWork.CategoryRepository.SoftDeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();
                return null; // Success
            }
            catch (KeyNotFoundException knfEx)
            {
                return knfEx.Message;
            }
            catch (Exception ex)
            {
                // Log exception (ex)
                return $"Error soft-deleting category: {ex.Message}";
            }
        }
    }
}
