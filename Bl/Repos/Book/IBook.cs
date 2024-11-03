using Bl.Repos.Generics;
using Domains;
using Domains.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.Repos.Book
{
    public interface IBook : IGeneric<TbBook>
    {
        IEnumerable<TbBook> GetBooks();
        public TbBook? GetBookById(int id);
        Task<IEnumerable<BookDetailsDto>> GetAvailableBooksAsync(string? search);
         Task<IEnumerable<BookDetailsDto>> GetAllAvailableBooksAsync();
    }
}
