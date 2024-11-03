
using Bl.Repos.Book;
using Bl.UnitOfWork;
using BookShopAPIs.Helpers;
using Domains;
using Domains.DTOS;
using Domains.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookShopAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        IUnitOfWork _unitOfWork;
        public BooksController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost("AddNewBook")]
        public async Task<IActionResult> AddBook(DtoAddBook book)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                TbBook tb = new TbBook
                {
                    Title = book.Title,
                    YearPublished = book.YearPublished, // Mapping YearPublished
                    ImageUrl = book.ImageUrl,
                    AuthorId = book.AuthorId,
                    CategoryId = book.CategoryId
                };
                _unitOfWork.book.Add(tb);
                _unitOfWork.Save();
                return Ok(tb);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetBooks")]
        public IActionResult GetBooks(int pagenumber=1 , int pagesize = 10 )
        {
            try
            {
                
                var books = _unitOfWork.book.GetBooks();
                if (!books.Any())  // If the list is empty
                {
                    return Ok("No books found."); 
                }

                // Calculate pagination values
                var Totalcount = books.Count();
                var paginatedBooks = books
                    .Skip((pagenumber - 1) * pagesize)
                    .Take(pagesize)
                    .ToList();


                // Map paginated books to BookDetailsDto
                var processedBooks = paginatedBooks.Select(book => new BookDetailsDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    YearPublished = book.YearPublished, // Mapping YearPublished
                    ImageUrl = book.ImageUrl,
                    Author = book.Author != null ? new AuthorDto { AuthorFirstName = book.Author.FirstName , AuthorLastName= book.Author.LastName } : null, // Mapping Author
                    Category = book.Category != null ? new CategoryDto { CategoryNmae = book.Category.CategoryName } : null // Mapping Category
                    }).ToList();


                // Create a PaginatedResponse<BookDetailsDto> instance
                var respone = new PaginatedResponse<BookDetailsDto>
                {
                    Items = processedBooks,
                    TotalCount = Totalcount,
                    PageSize = pagesize,
                    CurrentPage = pagenumber
                };



                return Ok(respone);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetBookById")]
        public IActionResult GetBookById(int id)
        {
            try
            {
                var book = _unitOfWork.book.GetBookById(id);
                if (book == null)
                    return NotFound($"Book {id} Not Found");

                var bookDetailsDto = new BookDetailsDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    YearPublished = book.YearPublished,
                    ImageUrl = book.ImageUrl,
                    Author = book.Author != null ? new AuthorDto { AuthorFirstName = book.Author.FirstName + " " ,AuthorLastName= book.Author.LastName } : null,
                    Category = book.Category != null ? new CategoryDto { CategoryNmae = book.Category.CategoryName } : null
                };

                return Ok(bookDetailsDto); // Return 200 OK with the book details
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateBook")]
        public async Task <IActionResult> UpdateBook( DtoAddBook updatebook)
        {
            try
            {
                // Validate model state
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var existedbook = await _unitOfWork.book.GetById(updatebook.Id);
                if (existedbook == null)
                    return NotFound($"Book Id {updatebook.Id} Not Found");

                //Mapping
                existedbook.Title = updatebook.Title ?? existedbook.Title;
                existedbook.YearPublished = updatebook.YearPublished;
                existedbook.AuthorId = updatebook.AuthorId;
                existedbook.CategoryId = updatebook.CategoryId;

                await _unitOfWork.book.Update(existedbook);
                _unitOfWork.Save();

                return Ok(existedbook);

            }
            catch (Exception)
            {
                return BadRequest(ModelState);
            }
            

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var book = await _unitOfWork.book.GetById(id);
                if (book == null)
                    return BadRequest();

                _unitOfWork.book.Delete(id);
                _unitOfWork.Save();
                return Ok(book);

            }
            catch (Exception ex)
            {

                return NotFound(ex.Message);

            }
        }

        //[HttpGet("Authors")]
        //public async Task<IActionResult> GetAllAuthors()
        //{
        //    var books = _unitOfWork.GetBooks();
        //    if (!books.Any())  // If the list is empty
        //    {
        //        return Ok("No books found.");
        //    }

        //    // Calculate pagination values
        //    var Totalcount = books.Count();
            
        //}
    }
}
