using Bl.Repos.Generics;
using Domains;
using Domains.DTOS;
using Domains.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.Repos.BorrowBook
{
    public class BorrowBook(BookShopContextAPIs context) : Generic<TbBorrowing>(context) , IBorrowBook
    {
        readonly BookShopContextAPIs _context = context;

        public async Task<bool> BorrowBookAsync(string userId, int bookId)
        {
            var userbook = new TbUserBook
            {
                BookId = bookId,
                UserId = userId,
                BorrowDate = DateTime.UtcNow,
                Status = status.Borrowed.ToString()
            };

            _context.TbUserBooks.Add(userbook);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsBookBorrowedAsync(int bookId)
        {
            return await _context.TbUserBooks.AnyAsync(a=>a.BookId == bookId && a.ReturnDate == null);
        }

        public async Task<bool> ReturnBookAsync(string userId, int bookId)
        {
            var userBook = await _context.TbUserBooks
                .FirstOrDefaultAsync(ub => ub.UserId == userId && ub.BookId == bookId && ub.ReturnDate == null);

            if (userBook == null)
            {
                return false;  // No active borrowing found for this user and book
            }

            userBook.ReturnDate = DateTime.UtcNow;
            userBook.Status = status.Returned.ToString();
            return await _context.SaveChangesAsync() > 0;
        }

        //public async Task<IEnumerable<UserBookDto>> GetUserBorrowedBooksAsync(string userId)
        //{
        //    var borrowedBooks = await _context.TbUserBooks
        //        .Include(b => b.Book) // Include book details
        //        .Where(b => b.UserId == userId)
        //        .Select(b => new UserBookDto
        //        {
        //            Id = b.BookId,
        //            Title = b.Book.Title,
        //            Status = b.Status, // Include the status
        //            BorrowDate = b.BorrowDate,
        //            ReturnDate = b.ReturnDate
        //        })
        //        .ToListAsync();

        //    return borrowedBooks;
        //}

        public async Task<IEnumerable<BookDetailsDto>> GetBorrowedBooksAsync()
        {
            var query = _context.TbUserBooks
                .Where(b => b.ReturnDate == null )
                .Include(b => b.Book).ThenInclude(b => b.Author)
                .Include(b => b.Book).ThenInclude(b => b.Category);

            return await query.Select(b => new BookDetailsDto
            {
                Id = b.Book.Id,
                Title = b.Book.Title,
                YearPublished = b.Book.YearPublished,
                Author = new AuthorDto
                {
                    AuthorFirstName = b.Book.Author.FirstName,
                    AuthorLastName = b.Book.Author.LastName
                },
                Category = new CategoryDto
                {
                    CategoryNmae = b.Book.Category.CategoryName
                }
            }).ToListAsync();
        }

        public async Task<IEnumerable<BookDetailsDto>> GetOverdueBooksAsync()
        {
            var overdueDate = DateTime.UtcNow.AddSeconds(3);
            var query = _context.TbUserBooks
                .Where(b => b.ReturnDate == null && b.BorrowDate < overdueDate)
                .Include(b => b.Book).ThenInclude(b => b.Author)
                .Include(b => b.Book).ThenInclude(b => b.Category);

            return await query.Select(b => new BookDetailsDto
            {
                Id = b.Book.Id,
                Title = b.Book.Title,
                YearPublished = b.Book.YearPublished,
                Author = new AuthorDto
                {
                    AuthorFirstName = b.Book.Author.FirstName,
                    AuthorLastName = b.Book.Author.LastName
                },
                Category = new CategoryDto
                {
                    CategoryNmae = b.Book.Category.CategoryName
                }
            }).ToListAsync();
        }


    }
}
