using Bl.Repos.Generics;
using Domains;
using Domains.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                .FirstOrDefaultAsync(a => a.CategoryName == categoryName );
        }
    }
}
