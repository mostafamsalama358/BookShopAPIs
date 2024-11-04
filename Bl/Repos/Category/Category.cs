using Bl.Repos.Generics;
using Domains;
using Domains.DTOS.ForCategories;
using Microsoft.EntityFrameworkCore;

namespace Bl.Repos.Category
{
    public class Category : Generic<TbCategory>, ICategory
    {
        private readonly BookShopContextAPIs context;
        public Category(BookShopContextAPIs _context) : base(_context)
        {
            context = _context;
        }

        public async Task<TbCategory?> GetByCategoryName(string categoryName)
        {
            return await context.TbCategories
                .FirstOrDefaultAsync(a => a.CategoryName == categoryName);
        }
        public TbCategory? GetByCategorId(int categoryid)
        {
            try
            {
                return context.TbCategories.Include(a => a.Books).ThenInclude(a=>a.TbAuthorBooks).ThenInclude(a=>a.TbAuthor)
                             .FirstOrDefaultAsync(a => a.Id == categoryid).Result;
            }
            catch (Exception)
            {
                return new TbCategory();
            }
        }
        public CategoryDetailsDto MapToDto(TbCategory category)
        {
            return new CategoryDetailsDto
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                Bookdtos = category.Books.Select(b => new Bookdto
                {
                    Id = b.Id,
                    Title = b.Title,
                    YearPublished = b.YearPublished,
                  Authors = b.TbAuthorBooks.Select(a=>new Authordto
                  {
                      AuthorFirstName = a.TbAuthor.FirstName,
                      AuthorLastName = a.TbAuthor.LastName
                  }).ToList(),
                }).ToList()
            };
        }

    }
}
