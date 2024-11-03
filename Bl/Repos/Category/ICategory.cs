using Bl.Repos.Generics;
using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.Repos.Category
{
    public interface ICategory : IGeneric<TbCategory>
    {
        Task<TbCategory?> GetByCategoryName(string categoryName);
    }
}
