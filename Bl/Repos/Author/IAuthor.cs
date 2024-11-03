using Bl.Repos.Generics;
using Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.Repos.Author
{
    public interface IAuthor : IGeneric<TbAuthor> 
    {
        Task<TbAuthor?> GetByFullName(string firstName, string lastName);
        Task<List<TbAuthor>> GetAuthorsByIdsAsync(List<int> authorIds);
    }
}
