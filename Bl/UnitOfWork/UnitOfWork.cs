
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Domains;
using Bl.Repos.Account;
using Bl.Repos.Book;
using Bl.Repos.BorrowBook;

namespace Bl.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
       private readonly  BookShopContextAPIs _context;
        public IConfiguration configuration { get; }
        public RoleManager<IdentityRole> RoleManager { get; }
        public UserManager<AppUser> UserManager { get; }
        public IAccount password { get; }

        public IBook book { get; }

        public IBorrowBook BorrowBook { get; }

        public IEmailServices EmailServices { get; }

        public UnitOfWork(BookShopContextAPIs ctx
           ,IConfiguration _configuration,
          RoleManager<IdentityRole> roleManager,UserManager<AppUser> userManager, IAccount password, IBook book
            , IBorrowBook borrowBook, IEmailServices emailServices)
        {
            _context = ctx;
            configuration = _configuration;
            RoleManager = roleManager;
            UserManager = userManager;
            this.password = password;
            this.book = book;
            this.BorrowBook = borrowBook;
            EmailServices = emailServices;

        }

        public bool Save()
        {
            _context.SaveChanges();
            return true;
        }
    }
}
