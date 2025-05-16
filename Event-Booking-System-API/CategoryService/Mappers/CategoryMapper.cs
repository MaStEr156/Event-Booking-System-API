using DB_Layer.Entities;
using Event_Booking_System_API.CategoryService.DTOs;

namespace Event_Booking_System_API.CategoryService.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryResponse ToCategoryResponse(this Category category)
        {
            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
            };
        }

        public static IEnumerable<CategoryResponse> ToCategoryResponses(this IEnumerable<Category> categories)
        {
            return categories.Select(c => c.ToCategoryResponse());
        }

        public static GetCategoryResponse ToGetCategoryResponse(this Category category)
        {
            return new GetCategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                EventsCount = category.Events?.Count ?? 0
            };
        }

        public static Category ToCategory(this CategoryRequest categoryRequest)
        {
            return new Category
            {
                Name = categoryRequest.Name,
                IsDeleted = false
            };
        }

        public static AddCategoryResponse ToAddCategoryResponse(this Category category)
        {
            return new AddCategoryResponse
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
} 