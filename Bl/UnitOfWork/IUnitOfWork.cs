
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Domains;
using Bl.Repos.Account;
using Bl.Repos.Book;
using Bl.Repos.BorrowBook;

namespace Bl.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IConfiguration configuration { get; }
        public RoleManager<IdentityRole> RoleManager { get; }
        public UserManager<AppUser> UserManager { get; }

        public IEmailServices EmailServices { get; }
        public IAccount password { get; }
        public IBook book { get; }
        public IBorrowBook BorrowBook { get; }
        public bool Save();
    }
}
