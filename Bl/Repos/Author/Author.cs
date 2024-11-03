using Bl.Repos.Generics;
using Domains;
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

    }
}
