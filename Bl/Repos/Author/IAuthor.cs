using Bl.Repos.Generics;
using Domains.DTOS.ForAuthor;
using Domains.Models;

namespace Bl.Repos.Author
{
    public interface IAuthor : IGeneric<TbAuthor>
    {
        Task<TbAuthor?> GetByFullName(string firstName, string lastName);
        Task<List<TbAuthor>> GetAuthorsByIdsAsync(List<int> authorIds);
        List<AddAuthorDto> MapToDto(IEnumerable<TbAuthor> author);
        Task<TbAuthor?> GetByAuthorId(int authorid);
         Task<AuthorDetailsDto> MapToDetailsDtoAsync(TbAuthor authors);
    }
}
