using Domains.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.Repos.BorrowBook
{
    public interface IBorrowBook
    {
        Task<bool> BorrowBookAsync(string userId, int bookId);
        Task<bool> IsBookBorrowedAsync(int bookId);
        Task<bool> ReturnBookAsync(string userId, int bookId);
        //Task<IEnumerable<UserBookDto>> GetUserBorrowedBooksAsync(string userId);
        Task<IEnumerable<BookDetailsDto>> GetBorrowedBooksAsync();
         Task<IEnumerable<BookDetailsDto>> GetOverdueBooksAsync();
    }
}
