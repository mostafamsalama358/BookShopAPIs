using Bl.Repos.Book;
using Bl.UnitOfWork;
using Domains;
using Domains.DTOS.ForBook;
using Domains.DTOS.ForCategories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookShopAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        IUnitOfWork _unitOfWork;
        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost("AddCategory")]
        public IActionResult AddCategories(AddCategoryDto categoryDto)
        {
            try
            {
                if (categoryDto == null)
                    return NotFound();
                var category = new TbCategory
                {
                    CategoryName = categoryDto.CategoryName
                };
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();

                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            try
            {
                var category = _unitOfWork.Category.GetByCategorId(id);
                if (category == null)
                    return NotFound($"Category {id} Not Found");

                var categoryDto = _unitOfWork.Category.MapToDto(category); // Map to DTO
                return Ok(categoryDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpGet("GetAllCategory")]
        public IActionResult GetAllCategory()
        {
            try
            {
                var category = _unitOfWork.Category.GetAll();

                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(int id,UpdateCategoryDto updateCategory)
        {
            try
            {
                // Validate model state
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var existedCategory = await _unitOfWork.Category.GetById(id);
                if (existedCategory == null)
                {
                    return NotFound($"Category Id {id} Not Found");
                }
                existedCategory.CategoryName = updateCategory.CategoryName ?? existedCategory.CategoryName;

                //_unitOfWork.Category.Update(existedCategory);
                _unitOfWork.Save();
                return Ok(updateCategory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                // Validate model state
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var existedCategory = await _unitOfWork.Category.GetById(id);
                if (existedCategory == null)
                    return NotFound($"Category Id {id} Not Found");

                _unitOfWork.Category.Delete(id);
                _unitOfWork.Save();
                return Ok(existedCategory);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
