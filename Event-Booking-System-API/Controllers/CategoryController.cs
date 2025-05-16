using Event_Booking_System_API.CategoryService;
using Event_Booking_System_API.CategoryService.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Event_Booking_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategoriesAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("Page number and page size must be greater than zero.");
            }
            try
            {
                var (categories, error) = await _categoryService.GetAllCategoriesAsync(pageNumber, pageSize);
                if (error != null)
                {
                    return NotFound(error);
                }
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetCategoryById/{id}")]
        public async Task<IActionResult> GetCategoryByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Category ID cannot be null or empty.");
            }
            try
            {
                var (category, error) = await _categoryService.GetCategoryByIdAsync(id);
                if (error != null)
                {
                    return NotFound(error);
                }
                return Ok(category);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategoryAsync([FromBody] CategoryRequest categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest("Category data is null.");
            }
            try
            {
                var (addedCategory, error) = await _categoryService.AddCategoryAsync(categoryDto);
                if (error != null)
                {
                    return BadRequest(error);
                }
                return Ok(addedCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("UpdateCategory/{id}")]
        public async Task<IActionResult> UpdateCategoryAsync(string id, [FromBody] CategoryRequest categoryDto)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Category ID cannot be null or empty.");
            }
            if (categoryDto == null)
            {
                return BadRequest("Category update data is null.");
            }
            try
            {
                var error = await _categoryService.UpdateCategoryAsync(id, categoryDto);
                if (error != null)
                {
                    if (error.Contains("not found"))
                        return NotFound(error);
                    return BadRequest(error);
                }
                return Ok(new { Message = "Category updated successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategoryAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Category ID cannot be null or empty.");
            }
            try
            {
                var error = await _categoryService.DeleteCategoryAsync(id);
                if (error != null)
                {
                    return NotFound(error);
                }
                return Ok(new { Message = "Category deleted successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("SoftDeleteCategory/{id}")]
        public async Task<IActionResult> SoftDeleteCategoryAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Category ID cannot be null or empty.");
            }
            try
            {
                var error = await _categoryService.SoftDeleteCategoryAsync(id);
                if (error != null)
                {
                    return NotFound(error);
                }
                return Ok(new { Message = "Category soft deleted successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
