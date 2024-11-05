using Bl.Repos.Category;
using Bl.UnitOfWork;
using Domains;
using Domains.DTOS.ForAuthor;
using Domains.DTOS.ForBook;
using Domains.DTOS.ForCategories;
using Domains.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookShopAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class AuthorsController : ControllerBase
    {
        IUnitOfWork _unitOfWork;
        public AuthorsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost("AdNewAuthor")]

        public IActionResult AddAuthors(AddAuthorDto cauthoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var newauthor = new TbAuthor
                {
                    FirstName = cauthoryDto.FirstName,
                    LastName = cauthoryDto.LastName,
                    Biography = cauthoryDto.Biography
                };

                _unitOfWork.author.Add(newauthor);
                _unitOfWork.Save();
                return Ok(newauthor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetAllAuthors")]
        [AllowAnonymous]

        public IActionResult GetAllAuthors()
        {
            try
            {
                var author = _unitOfWork.author.GetAll();

                // Use the external mapping method
                var authorDtos = _unitOfWork.author.MapToDto(author);

                return Ok(authorDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAuthorsById")]
        [AllowAnonymous]
        public async Task< IActionResult> GetAllAuthorsById(int id)
        {
            try
            {
                var author = await _unitOfWork.author.GetByAuthorId(id);
                if (author == null)
                    return NotFound($"Author {id} Not Found");
                // Use the external mapping method
                var authorDtos = await _unitOfWork.author.MapToDetailsDtoAsync(author);

                return Ok(authorDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateAuthor")]
        public async Task<IActionResult> UpdateAuthorsById(int id,UpdateAuthorDto updateAuthor)
        {
            try
            {
                // Validate model state
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existedAuthor = await _unitOfWork.author.GetById(id);
                if (existedAuthor == null)
                    return NotFound($"Author {id} Not Found");

                existedAuthor.FirstName = updateAuthor.FirstName ?? existedAuthor.FirstName;
                existedAuthor.LastName = updateAuthor.LastName ?? existedAuthor.LastName;
                existedAuthor.Biography = updateAuthor.Biography ?? existedAuthor.Biography;

                _unitOfWork.author.Update(existedAuthor);
                _unitOfWork.Save();
                return Ok(existedAuthor);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteAuthor")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                // Validate model state
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var existedAuthor = await _unitOfWork.author.GetById(id);
                if (existedAuthor == null)
                    return NotFound($"Author Id {id} Not Found");

                _unitOfWork.author.Delete(id);
                _unitOfWork.Save();
                return Ok(existedAuthor);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
