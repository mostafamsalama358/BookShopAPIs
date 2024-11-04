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
        public async Task<IActionResult> AddCategories(AddCategoryDto categoryDto)
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
        public async Task<IActionResult> GetCategory(int id)
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
    }
}
