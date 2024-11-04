using Bl.UnitOfWork;
using Domains.DTOS.ForBorrowing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BookShopAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowingBookController : ControllerBase
    {
        IUnitOfWork _unitOfWork;
        public BorrowingBookController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Get available books
        [HttpGet("AvailableBySearch")]
        public async Task<IActionResult> GetAvailableBooks([FromQuery] string? search)
        {
            try
            {
                var availablebooks = await _unitOfWork.book.GetAvailableBooksAsync(search);

                //Any(): By calling Any() on availablebooks, you're checking whether the list contains any books:
               // If the list is empty(no books were found), Any() will return false,
               // and the condition!availablebooks.Any() will evaluate to true, triggering the NotFound response.

                if (availablebooks == null|| !availablebooks.Any())
                {
                    return Ok("Book Was Borrowed");
                }
                return Ok(availablebooks);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [HttpGet("AllAvailable")]
        public async Task<IActionResult> GetAllAvailableBooks()
        {
            try
            {
                var availablebooks = await _unitOfWork.book.GetAllAvailableBooksAsync();
                if (availablebooks == null)
                {
                    return NotFound("Book Was Borrowed");
                }
                return Ok(availablebooks);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        // Borrow a book
        [HttpPost("BorrowBook")]
        public async Task<IActionResult> BorrowBook([FromBody] BorrowbookRequestDto request)
        {
            try
            {
                var isborrowing = await _unitOfWork.BorrowBook.IsBookBorrowedAsync(request.BookId);
                if (isborrowing)
                    return NotFound("Book is already borrowed.");

                var success = await _unitOfWork.BorrowBook.BorrowBookAsync(request.UserId,request.BookId);
                if (success)
                    return Ok("Book successfully borrowed.");

                return BadRequest("Failed to borrow book.");
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }

        }

        // Get available books
        [HttpGet("GetBorrowBook")]
        public async Task<IActionResult> GetBorrowedBooks()
        {
            try
            {
                var borrowedBooks = await _unitOfWork.BorrowBook.GetBorrowedBooksAsync();
                if (!borrowedBooks.Any())
                {
                    return NotFound("No borrowed books found.");
                }

                return Ok(borrowedBooks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetOverdueBooks")]
        public async Task<IActionResult> GetOverdueBooks()
        {
            try
            {
                var overdueBooks = await _unitOfWork.BorrowBook.GetOverdueBooksAsync();
                if (!overdueBooks.Any())
                {
                    return NotFound("No overdue books found.");
                }

                return Ok(overdueBooks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Return a book
        [HttpPost("ReturnBook")]
        public async Task<IActionResult> ReturnBook([FromBody] BorrowbookRequestDto request)    
        {
            try
            {
                var success = await _unitOfWork.BorrowBook.ReturnBookAsync(request.UserId, request.BookId);
                if (!success)
                    return BadRequest("Failed to borrow book.");



                return Ok("Book successfully Returned.");
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }

        }




    }
}
