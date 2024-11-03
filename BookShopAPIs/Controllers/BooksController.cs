using Bl.Repos.Book;
using Bl.Repos.Category;
using Bl.UnitOfWork;
using BookShopAPIs.Helpers;
using Domains;
using Domains.DTOS;
using Domains.Models;
using Microsoft.AspNetCore.Mvc;

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
                    //AuthorId = book.AuthorId,
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
        public IActionResult GetBooks(int pagenumber = 1, int pagesize = 10)
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
                    Authors = book.TbAuthorBooks.Select(ab => new AuthorDto
                    {
                        AuthorFirstName = ab.TbAuthor.FirstName,
                        AuthorLastName = ab.TbAuthor.LastName
                    }).ToList(), // Mapping multiple authors
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
                    Authors = book.TbAuthorBooks.Select(ab => new AuthorDto
                    {
                        AuthorFirstName = ab.TbAuthor.FirstName,
                        AuthorLastName = ab.TbAuthor.LastName
                    }).ToList(), // Mapping multiple authors
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
        public async Task<IActionResult> UpdateBook(DtoUpdateBook updateBook)
        {
            try
            {
                // Validate model state
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check if the book exists
                var existingBook = await _unitOfWork.book.GetById(updateBook.Id);
                if (existingBook == null)
                {
                    return NotFound($"Book Id {updateBook.Id} Not Found");
                }

                // Mapping the updated values to the existing book
                existingBook.Title = updateBook.Title;
                existingBook.YearPublished = updateBook.YearPublished;
                existingBook.ImageUrl = updateBook.ImageUrl;

                // Update the category
                var existingCategory = await _unitOfWork.Category.GetByCategoryName(updateBook.Category.CategoryName);
                if (existingCategory == null)
                {
                    // If category does not exist, you can create a new one
                    existingCategory = new TbCategory { CategoryName = updateBook.Category.CategoryName };
                    _unitOfWork.Category.Add(existingCategory);
                }
                existingBook.Category = existingCategory; // Update the existing category

                // Clear existing authors and create new relationships
                existingBook.TbAuthorBooks.Clear();


                foreach (var authorDto in updateBook.Authors)
                {
                    // Check if the author already exists
                    var author = await _unitOfWork.author.GetByFullName(authorDto.FirstName, authorDto.LastName);
                    if (author == null)
                    {
                        // If the author doesn't exist, create a new one
                        author = new TbAuthor
                        {
                            FirstName = authorDto.FirstName,
                            LastName = authorDto.LastName
                        };
                        _unitOfWork.author.Add(author);
                    }

                    // Add the relationship to TbAuthorBooks
                    existingBook.TbAuthorBooks.Add(new TbAuthorBook
                    {
                        TbAuthorId = author.Id, // Set the author's ID
                        TbBookId = existingBook.Id // Set the book's ID
                    });

                    // Ensure the author is tracked
                    var trackedAuthor = await _unitOfWork.author.GetByFullName(authorDto.FirstName, authorDto.LastName);
                    if (trackedAuthor != null)
                    {
                        // Add the new relationship
                        existingBook.TbAuthorBooks.Add(new TbAuthorBook
                        {
                            TbAuthorId = trackedAuthor.Id, // Set the author's ID
                            TbBookId = existingBook.Id // Set the book's ID
                        });
                    }
                }

                // Update the book
                _unitOfWork.book.Update(existingBook);
                 _unitOfWork.Save(); // Ensure you save changes

                return Ok(existingBook);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return BadRequest(new { message = "An error occurred while updating the book.", error = ex.Message });
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

