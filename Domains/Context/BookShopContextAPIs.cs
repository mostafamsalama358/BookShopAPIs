using System;
using System.Collections.Generic;
using Domains;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domains.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace Domains;

public partial class BookShopContextAPIs : IdentityDbContext<AppUser>
{
    public BookShopContextAPIs(DbContextOptions<BookShopContextAPIs> options)
        : base(options)
    {
    }


    public DbSet<TbBorrowing> TbBorrowings { get; set; }
    public DbSet<TbBook> TbBooks { get; set; }
    public DbSet<TbCategory> TbCategories { get; set; }
    public DbSet<TbUserBook> TbUserBooks { get; set; }
    public DbSet<TbAuthor> TbAuthors { get; set; }
}

