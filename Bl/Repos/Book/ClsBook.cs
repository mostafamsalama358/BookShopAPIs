using Bl.Repos.Generics;
using Domains;
using Domains.DTOS.ForBook;
using Domains.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.Repos.Book
{
    public class ClsBook : Generic<TbBook>, IBook
    {
        BookShopContextAPIs _context;
        public ClsBook(BookShopContextAPIs _context) : base(_context)
        {
            this._context = _context;
        }

        public IEnumerable<TbBook> GetBooks()
        {
            var books = _context.TbBooks
                .Include(a=>a.TbAuthorBooks)
                .ThenInclude(a => a.TbAuthor)
                                .Include(b => b.Category)
                                .ToList();
            return books;
        }

        public TbBook? GetBookById(int id)
        {
            return _context.TbBooks.Include(a => a.TbAuthorBooks)
                .ThenInclude(a => a.TbAuthor)
                .Include(b => b.Category)
                .FirstOrDefault(b => b.Id == id);

        }

        public async Task<IEnumerable<BookDetailsDto>> GetAvailableBooksAsync(string? search)
        {
            try
            {
                // Check and update borrowing statuses
                var threeDaysAgo = DateTime.UtcNow.AddSeconds(-3);

                // Get all borrowings that are overdue
                var overdueBorrowings = await _context.TbUserBooks
                    .Where(b => b.ReturnDate == null && b.BorrowDate < threeDaysAgo)
                    .ToListAsync();

                // Update status to overdue for all overdue borrowings
                foreach (var borrowing in overdueBorrowings)
                {
                    borrowing.Status = "Overdue";
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                var query = _context.TbBooks.Where
                (a => !_context.TbUserBooks.Any(b => b.BookId == a.Id && b.ReturnDate == null));
                if (!string.IsNullOrEmpty(search))
                    if (int.TryParse(search, out int bookId))
                    {
                        query = query.Where(a => a.Id == bookId);
                    }
                    else
                        query = query.Where(a => a.Title.Contains(search) /*|| a.Author.FirstName.Contains(search)*/);

                var availablebook = await query/*.Include(a => a.Author)*/.Include(a => a.Category)
                     .Select(a => new BookDetailsDto
                     {
                         Id = a.Id,
                         Title = a.Title,
                         YearPublished = a.YearPublished,
                         //Author = a.Author != null ? new AuthorDto { AuthorFirstName = a.Author.FirstName, AuthorLastName = a.Author.LastName } : null, // Mapping Author
                         Category = a.Category != null ? new CategoryDto { CategoryNmae = a.Category.CategoryName } : null // Mapping Category

                     }).ToListAsync();

                return availablebook;


            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<IEnumerable<BookDetailsDto>> GetAllAvailableBooksAsync()
        {
            try
            {
                // Check and update borrowing statuses
                var threeDaysAgo = DateTime.UtcNow.AddSeconds(-3);

                // Get all borrowings that are overdue
                var overdueBorrowings = await _context.TbUserBooks
                    .Where(b => b.ReturnDate == null && b.BorrowDate < threeDaysAgo)
                    .ToListAsync();

                // Update status to overdue for all overdue borrowings
                foreach (var borrowing in overdueBorrowings)
                {
                    borrowing.Status = "Overdue";
                }

                // Save changes to the database
                await _context.SaveChangesAsync();


                var query = _context.TbBooks.Where
                    (a => !_context.TbUserBooks.Any(b => b.BookId == a.Id && b.ReturnDate == null));

                var availablebook = await query.Include(a => a.Category)
                     .Select(a => new BookDetailsDto
                     {
                         Id = a.Id,
                         Title = a.Title,
                         YearPublished = a.YearPublished,

                         //Ternary Operation
                         // condition ? value_if_true : value_if_false

                         //Author = a.Author != null ? new AuthorDto { AuthorFirstName = a.Author.FirstName, AuthorLastName = a.Author.LastName } : null, // Mapping Author
                         Category = a.Category != null ? new CategoryDto { CategoryNmae = a.Category.CategoryName } : null // Mapping Category

                     }).ToListAsync();

                return availablebook;

            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task UpdateBookAsync(TbBook book, List<int> authorIds)
        {
            // Attach the book entity if needed
            _context.TbBooks.Attach(book);

            // Clear existing authors in the many-to-many relationship
            var existingAuthors = _context.TbAuthorBooks.Where(ab => ab.TbBookId == book.Id);
            _context.TbAuthorBooks.RemoveRange(existingAuthors);

            // Add the new authors based on the provided authorIds
            foreach (var authorId in authorIds)
            {
                var authorBookLink = new TbAuthorBook
                {
                    TbBookId = book.Id,
                    TbAuthorId = authorId
                };
                _context.TbAuthorBooks.Add(authorBookLink);
            }

            // Update other fields in the book
            _context.TbBooks.Update(book);

            // Save changes to persist the updates to the linking table and book details
            await _context.SaveChangesAsync();
        }

        public async Task AddAuthorBookLinkAsync(int bookId, int authorId)
        {
            var authorBookLink = new TbAuthorBook
            {
                TbBookId = bookId,
                TbAuthorId = authorId
            };

            await _context.TbAuthorBooks.AddAsync(authorBookLink);
        }

        //public async Task<object> GetAllAuthors()
        //{
        //    return _context.TbAuthors.Include(a => a.Books).ToListAsync();
        //}


    }
}
