
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Domains;
using Bl.Repos.Account;
using Bl.Repos.Book;
using Bl.Repos.BorrowBook;
using Bl.Repos.Author;
using Bl.Repos.Category;

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

        public IAuthor author { get; }

        public ICategory Category { get; }

        public UnitOfWork(BookShopContextAPIs ctx
           ,IConfiguration _configuration,
          RoleManager<IdentityRole> roleManager,UserManager<AppUser> userManager, IAccount password, IBook book, IAuthor author , ICategory category
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
            this.author = author;
            Category = category;
        }

        public bool Save()
        {
            _context.SaveChanges();
            return true;
        }
    }
}
