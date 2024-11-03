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
    public DbSet<TbAuthorBook> TbAuthorBooks { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<TbAuthorBook>()
            .HasKey(ab => new { ab.TbAuthorId, ab.TbBookId });

        modelBuilder.Entity<TbAuthorBook>()
            .HasOne(ab => ab.TbAuthor)
            .WithMany(a => a.TbAuthorBooks)
            .HasForeignKey(ab => ab.TbAuthorId);

        modelBuilder.Entity<TbAuthorBook>()
            .HasOne(ab => ab.TbBook)
            .WithMany(b => b.TbAuthorBooks)
            .HasForeignKey(ab => ab.TbBookId);
    }
}

