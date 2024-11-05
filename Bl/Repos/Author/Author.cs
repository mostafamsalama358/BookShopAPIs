using Bl.Repos.Generics;
using Domains;
using Domains.DTOS.ForAuthor;
using Domains.DTOS.ForCategories;
using Domains.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.Repos.Author
{
    public class Author : Generic<TbAuthor>, IAuthor
    {
        private readonly BookShopContextAPIs context;
        public Author(BookShopContextAPIs _context) : base(_context)
        {
            context = _context;
        }

        public async Task<TbAuthor?> GetByFullName(string firstName, string lastName)
        {
            return await context.TbAuthors
                .FirstOrDefaultAsync(a => a.FirstName == firstName && a.LastName == lastName);
        }
        public async Task<List<TbAuthor>> GetAuthorsByIdsAsync(List<int> authorIds)
        {
            return await context.TbAuthors
                .Where(author => authorIds.Contains(author.Id))
                .ToListAsync();
        }
        public List<AddAuthorDto> MapToDto(IEnumerable<TbAuthor> author)
        {
            return author.Select(a => new AddAuthorDto
            {
                FirstName = a.FirstName,
                LastName = a.LastName,
                Biography = a.Biography
            }).ToList();
        }
        public async Task <TbAuthor?> GetByAuthorId(int authorid)
        {
            try
            {
                return await context.TbAuthors.Include(a => a.TbAuthorBooks).ThenInclude(a => a.TbBook)
                             .FirstOrDefaultAsync(a => a.Id == authorid);
            }
            catch (Exception)
            {
                return new TbAuthor();
            }
        }

        public async Task<AuthorDetailsDto> MapToDetailsDtoAsync(TbAuthor authors)
        {
            return await Task.FromResult(new AuthorDetailsDto
            {
                FirstName = authors.FirstName,
                LastName = authors.LastName,
                Biography = authors.Biography,
                Bookdto = authors.TbAuthorBooks.Select(b => new AuthorBookdto
                {
                    Title = b.TbBook.Title,
                    YearPublished = b.TbBook.YearPublished
                    // Add more properties as needed
                }).ToList()
            });
        }
    }
}
