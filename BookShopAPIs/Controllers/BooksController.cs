using Bl.Repos.Book;
using Bl.Repos.Category;
using Bl.UnitOfWork;
using BookShopAPIs.Helpers;
using Domains;
using Domains.DTOS;
using Domains.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class BooksController : ControllerBase
    {

        IUnitOfWork _unitOfWork;
        public BooksController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost("AddNewBook")]
        public async Task<IActionResult> AddBook(AddBookDto book)
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
                // Add the book to the repository
                _unitOfWork.book.Add(tb);
                _unitOfWork.Save();

                // Create the linking entry in TbAuthorBook using the new repository method
                await _unitOfWork.book.AddAuthorBookLinkAsync(tb.Id,book.AuthorId);

                // Save again to persist the link
                 _unitOfWork.Save();


                return Ok(tb);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetBooks")]
        [AllowAnonymous]
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
        [AllowAnonymous]

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
        public async Task<IActionResult> UpdateBook(UpdateBookDto updateBook)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var book =  _unitOfWork.book.GetBookById(updateBook.Id);
                if (book == null)
                    return NotFound("Book not found");

                // Update basic fields
                book.Title = updateBook.Title;
                book.YearPublished = updateBook.YearPublished;

                // Update category
                var category = await _unitOfWork.Category.GetById(book.CategoryId);
                if(category==null)
                    return BadRequest("Invalid category ID");
                book.Category = category;

                //// Update authors
                //var authors = await _unitOfWork.author.GetAuthorsByIdsAsync(updateBook.AuthorIds);
                //if (authors == null || !authors.Any())
                //    return BadRequest("Invalid author IDs");

                //book.TbAuthorBooks.Clear();
                //foreach (var author in authors)
                //{
                //    var authorBook = new TbAuthorBook
                //    {
                //        TbAuthorId = author.Id,
                //        TbBookId = book.Id
                //    };
                //    _unitOfWork.author.Update(authorBook);
                //}


                // Call repository method to update book and its authors
                await _unitOfWork.book.UpdateBookAsync(book, updateBook.AuthorIds);

                return Ok("Book updated successfully");
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

    }
}

